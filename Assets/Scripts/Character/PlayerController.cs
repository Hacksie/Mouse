using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Camera mainCam;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private CharController character = null;
        [SerializeField] private Thermoptic camo = null;
        [SerializeField] private Targeter targeter = null;
        [SerializeField] private Transform aimPivot = null;
        [SerializeField] private bool crouchToggle = false;


        private InputAction moveAction;
        private InputAction climbAction;
        private InputAction jumpAction;
        private InputAction crouchToggleAction;
        private InputAction walkToggleAction;
        private InputAction crouchAction;
        private InputAction rollAction;
        private InputAction attackAction;
        private InputAction menuAction;
        private InputAction mousePosAction;
        private InputAction lookPosAction;
        private InputAction interactAction;
        private InputAction selectAction;
        private InputAction aimAction;

        public CharController Character { get => character; set => character = value; }

        void Awake()
        {
            this.AutoBind(ref character);
            this.AutoBind(ref camo);
            character.dieActions.AddListener(Die);
            BindInputs();
        }

        private void BindInputs()
        {
            menuAction = playerInput.actions["Menu"];
            moveAction = playerInput.actions["Move"];
            climbAction = playerInput.actions["Climb"];
            jumpAction = playerInput.actions["Jump"];
            crouchAction = playerInput.actions["Crouch"];
            crouchToggleAction = playerInput.actions["Crouch Toggle"];
            walkToggleAction = playerInput.actions["Walk Toggle"];
            rollAction = playerInput.actions["Roll"];
            attackAction = playerInput.actions["Attack"];
            interactAction = playerInput.actions["Interact"];
            mousePosAction = playerInput.actions["Mouse Position"];
            lookPosAction = playerInput.actions["Look"];
            selectAction = playerInput.actions["OperatingSystem"];
            aimAction = playerInput.actions["Aim"];

            selectAction.performed += SelectEvent;
            menuAction.performed += MenuEvent;
            interactAction.performed += InteractEvent;
        }

        void Start() => Reset();
        public void Reset()
        {
            Stop();
            character.Reset();
            targeter.ShowTarget(Vector3.zero, false, false);
        }

        public void Stop()
        {
            character.ExecuteCommand(new StopCommand());
        }

        public void MenuEvent(InputAction.CallbackContext context)
        {
            Debug.Log("Menu Event", this);
            Game.Instance.CurrentState.Menu();
        }

        public void SelectEvent(InputAction.CallbackContext context)
        {
            Debug.Log("Select Event", this);
            Game.Instance.CurrentState.Select();
        }

        public void InteractEvent(InputAction.CallbackContext context)
        {
            Debug.Log("Trigger interact", this);
            targeter.TriggerInteract();
        }

        public void Die() => Game.Instance.SetDeath();

        public void UpdateSitBehaviour()
        {
            //targeter.UpdateInteractors();
            UpdateAimLine(false);

            if(this.attackAction.triggered)
            {
                targeter.TriggerInteract();
            }
        }

        public void UpdateIdleBehaviour()
        {
            float movement = moveAction.ReadValue<float>(); ;
            float climb = climbAction.ReadValue<float>();

            Vector3 targetPos = GetTargetingPosition();
            //targeter.UpdateInteractors();
            UpdateAimLine(crouchToggle || this.crouchAction.IsPressed());

            character.ExecuteCommand(new FacingCommand(movement, CalcForward(targetPos)));
            character.ExecuteCommand(new MoveCommand(movement, climb));
        }

        public void UpdateBattleBehaviour()
        {
            float movement = 0;
            float climb = 0;

            Vector3 targetPos = GetTargetingPosition();

            UpdateCrouchToggle();
            UpdateWalkToggle();

            character.ExecuteCommand(new CrouchCommand(crouchToggle || this.crouchAction.IsPressed()));

            UpdateAimLine(crouchToggle || this.crouchAction.IsPressed());


            if (character.IsAnimatingAttack) // If we're playing the attacking animation, don't let the player take another action
            {
                //Debug.Log("Attacking");
            }
            else if (this.attackAction.triggered)
            {
                character.Attack(targetPos, IsAiming());
            }
            else if (this.rollAction.triggered)
            {
                character.ExecuteCommand(new RolLCommand());
            }
            else
            {
                movement = moveAction.ReadValue<float>();
                climb = climbAction.ReadValue<float>();

                if (this.jumpAction.triggered)
                {
                    character.ExecuteCommand(new JumpCommand());
                }

                //character.Jump |= this.jumpAction.triggered;
                //character.ExecuteCommand(new JumpCommand(this.jumpAction.triggered));
                character.JumpHoldFlag |= this.jumpAction.IsPressed();
            }

            character.ExecuteCommand(new FacingCommand(movement, CalcForward(targetPos)));
            character.ExecuteCommand(new MoveCommand(movement, climb));
        }

        public void Teleport(Vector3 position)
        {
            transform.position = position;
        }

        private void UpdateCrouchToggle()
        {
            if (this.crouchToggleAction.triggered)
            {
                crouchToggle = !crouchToggle;
            }
        }

        private void UpdateWalkToggle()
        {
            if (this.walkToggleAction.triggered)
            {
                character.ExecuteCommand(new WalkToggleCommand());
            }
        }


        public void FixedUpdateBehaviour()
        {
            character.Physics();
        }

        public void LateUpdateBehaviour()
        {
            character.Animate();
        }

        private bool IsAiming()
        {
            if (playerInput.currentControlScheme == "Gamepad")
            {
                var lookPos = lookPosAction.ReadValue<Vector2>();
                return lookPos.magnitude > 0.1f;
            }
            else
            {
                return aimAction.IsPressed();
            }
        }

        // FIXME: Update for crouched
        private void UpdateAimLine(bool crouched)
        {

            if (playerInput.currentControlScheme == "Gamepad")
            {
                var lookPos = lookPosAction.ReadValue<Vector2>();

                if (lookPos.magnitude > 0.1f)
                {
                    Vector3 direction = CalcGamepadDirection(ref lookPos);
                    targeter.ShowTarget(direction, true, character.OperatingSystem.HasPistol);

                }
            }
            else
            {
                Vector3 direction = CalcMouseDirection();
                targeter.ShowTarget(direction, IsAiming(), character.OperatingSystem.HasPistol);
            }
        }

        private Vector3 CalcGamepadDirection(ref Vector2 lookPos)
        {
            return lookPos.normalized;
        }

        private Vector3 CalcMouseDirection()
        {
            Vector3 worldPos = CalcMousePosition();

            return (worldPos - aimPivot.position).normalized;
        }

        private Vector3 CalcMousePosition()
        {
            var mousePos = mousePosAction.ReadValue<Vector2>();
            var worldPos = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            worldPos = new Vector3(worldPos.x, worldPos.y, aimPivot.position.z);
            return worldPos;
        }

        private Vector3 GetTargetingPosition()
        {
            Vector3 targetPos;
            if (playerInput.currentControlScheme == "Gamepad")
            {
                var look = lookPosAction.ReadValue<Vector2>();
                targetPos = transform.position + new Vector3(look.x, look.y);
            }
            else
            {
                var mousePosScreen = mousePosAction.ReadValue<Vector2>();

                var mousePosScreen3D = new Vector3(mousePosScreen.x, mousePosScreen.y, mainCam.transform.position.z);

                targetPos = mainCam.ScreenToWorldPoint(mousePosScreen3D);
            }

            return targetPos;
        }

        private float CalcForward(Vector3 targetPos) => Mathf.Clamp(targetPos.x - transform.position.x, -1, 1);
    }
}