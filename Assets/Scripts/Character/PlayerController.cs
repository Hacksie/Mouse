using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        [SerializeField] private Interactor interactor = null;
        [SerializeField] private TargetAim targetAim = null;
        //[SerializeField] private bool runToggle = false;
        [SerializeField] private Transform aimPivot = null;
        [SerializeField] private bool crouchToggle = false;


        private InputAction moveAction;
        private InputAction climbAction;
        private InputAction jumpAction;
        private InputAction crouchToggleAction;
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
            BindInputs();

            this.AutoBind(ref character);
            this.AutoBind(ref camo);
            this.AutoBind(ref interactor);
            character.dieActions.AddListener(Die);
        }

        private void BindInputs()
        {
            menuAction = playerInput.actions["Menu"];
            moveAction = playerInput.actions["Move"];
            climbAction = playerInput.actions["Climb"];
            jumpAction = playerInput.actions["Jump"];
            crouchAction = playerInput.actions["Crouch"];
            crouchToggleAction = playerInput.actions["Crouch Toggle"];
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
            targetAim.HideAimLine();
        }

        public void Stop()
        {
            character.Stop();
        }

        public void Idle() { /*character.SetStateIdle();*/ character.State = CharacterState.Idle; }
        public void Battle() { /*character.SetStateBattle();*/ character.State = CharacterState.Battle; }

        public void Sit() { /*character.SetStateSeated();*/ character.State = CharacterState.Seated; }

        public void MenuEvent(InputAction.CallbackContext context)
        {
            Debug.Log("Menu Event");
            Game.Instance.CurrentState.Menu();
        }

        public void SelectEvent(InputAction.CallbackContext context)
        {
            Debug.Log("Select Event");
            Game.Instance.CurrentState.Select();
        }

        public void InteractEvent(InputAction.CallbackContext context)
        {
            Debug.Log("trigger interact", this);
            interactor.TriggerInteract();
        }

        public void Die()
        {
            Game.Instance.SetDeath();
        }

        public void UpdateBehavior()
        {
            interactor.UpdateInteractors();

            switch (character.State)
            {
                case CharacterState.Seated:
                    break;
                case CharacterState.Idle:
                    UpdateIdleBehaviour();
                    break;
                case CharacterState.Battle:
                    UpdateBattleBehaviour();
                    break;
                case CharacterState.Dead:
                    break;
            }
        }

        private void UpdateIdleBehaviour()
        {
            float movement = 0;
            float climb = 0;
            movement = moveAction.ReadValue<float>();
            climb = climbAction.ReadValue<float>();
            //character.Jump |= this.jumpAction.triggered;
            //character.JumpHoldFlag |= this.jumpAction.IsPressed();
            Vector3 targetPos = GetTargetingPosition();
            character.UpdateBehaviour(movement, climb, CalcForward(targetPos));

        }

        private void UpdateBattleBehaviour()
        {
            float movement = 0;
            float climb = 0;

            Vector3 targetPos = GetTargetingPosition();
            

            if (this.crouchToggleAction.triggered)
            {
                crouchToggle = !crouchToggle;
            }

            character.Crouched = crouchToggle || this.crouchAction.IsPressed();


             UpdateAimLine(crouchToggle || this.crouchAction.IsPressed());
            


            if (character.IsAttacking) // If we're playing the attacking animation, don't let the player take another action
            {
            }
            else if (this.attackAction.triggered)
            {
                character.Attack(targetPos, IsAiming());
            }
            else if (this.rollAction.triggered)
            {
                character.Roll();
            }
            else
            {
                movement = moveAction.ReadValue<float>();
                climb = climbAction.ReadValue<float>();
                character.Jump |= this.jumpAction.triggered;
                character.JumpHoldFlag |= this.jumpAction.IsPressed();
            }

            character.UpdateBehaviour(movement, climb, CalcForward(targetPos));
        }

        public void FixedUpdateBehaviour()
        {
            character.FixedUpdateBehaviour();
        }

        public void LateUpdateBehaviour()
        {
            character.LateUpdateBehaviour();
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

        private void UpdateAimLine(bool crouched)
        {
            if (playerInput.currentControlScheme == "Gamepad")
            {
                var lookPos = lookPosAction.ReadValue<Vector2>();
                if (!crouched && lookPos.magnitude > 0.1f)
                {
                    Vector3 direction = CalcGamepadDirection(ref lookPos);
                    targetAim.ShowAimLine(direction);

                }
                else
                {
                    targetAim.HideAimLine();
                }
            }
            else
            {
                if (!crouched && aimAction.IsPressed())
                {
                    Vector3 direction = CalcMouseDirection();

                    targetAim.ShowAimLine(direction);
                }
                else
                {
                    targetAim.HideAimLine();
                }
            }
        }

        private Vector3 CalcGamepadDirection(ref Vector2 lookPos)
        {
            return lookPos.normalized;
        }

        private Vector3 CalcMouseDirection()
        {
            var mousePos = mousePosAction.ReadValue<Vector2>();
            var worldPos = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            worldPos = new Vector3(worldPos.x, worldPos.y, aimPivot.position.z);

            return (worldPos - aimPivot.position).normalized;
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