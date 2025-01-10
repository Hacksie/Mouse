using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Camera mainCam;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private CharController character = null;
        [SerializeField] private Thermoptic camo = null;
        [SerializeField] private bool runToggle = false;
        [SerializeField] private bool crouchToggle = false;

        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction crouchAction;
        private InputAction rollAction;
        private InputAction attackAction;
        private InputAction menuAction;
        private InputAction mousePosAction;
        private InputAction lookPosAction;

        public CharController Character { get => character; set => character = value; }

        void Awake()
        {
            BindInputs();

            this.AutoBind(ref character);
            this.AutoBind(ref camo);
            character.dieActions.AddListener(Die);
        }

        private void BindInputs()
        {
            menuAction = playerInput.actions["Menu"];
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            crouchAction = playerInput.actions["Crouch"];
            rollAction = playerInput.actions["Roll"];
            attackAction = playerInput.actions["Attack"];
            
            mousePosAction = playerInput.actions["Mouse Position"];
            lookPosAction = playerInput.actions["Look"];
            menuAction.performed += MenuEvent;
        }

        void Start()
        {
            Reset();
        }

        // FIXME: Move the PU stuff elsewhere
        public void Ping()
        {
            var results = Physics2D.OverlapCircleAll(this.transform.position, 10);

            foreach (var result in results)
            {
                if ((result.CompareTag("Interactable") || result.CompareTag("Player") || result.CompareTag("Enemy")))
                {
                    var hl = result.GetComponent<Interactable>();
                    if (hl)
                    {
                        hl.Ping();
                    }
                }
            }
        }

        public void Reset()
        {
            Stop();
            character.Reset();
        }

        public void Stop()
        {
            character.Stop();
        }

        public void Idle()
        {
            character.State = CharacterState.Idle;
        }

        public void Battle()
        {
            character.State = CharacterState.Battle;
        }

        public void MenuEvent(InputAction.CallbackContext context)
        {
            Debug.Log("Menu Event");
            Game.Instance.CurrentState.Menu();
        }

        public void Die()
        {
            Game.Instance.SetDeath();
            //character.Die();
        }

        public void UpdateBehavior()
        {
            Vector3 targetPos;
            if(playerInput.currentControlScheme == "Gamepad")
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




            float movement = 0;

            if (this.crouchAction.triggered)
            {

                crouchToggle = !crouchToggle;
                character.Crouched = crouchToggle;
            }

            if (character.IsAttacking)
            {
                //Debug.Log("Is attacking");
            }
            else if (this.attackAction.triggered && Game.Instance.CurrentState.Battle)
            {
                character.Attack(targetPos);
            }
            else if (this.rollAction.triggered)
            {
                character.Roll();
            }

            else
            {
                movement = moveAction.ReadValue<float>();
                character.Jump |= this.jumpAction.triggered;
                character.JumpHoldFlag |= this.jumpAction.IsPressed();
            }


            character.UpdateBehavior(movement, Mathf.Clamp(targetPos.x - transform.position.x, -1, 1));

        }

        public void FixedUpdateBehaviour()
        {
            character.FixedUpdateBehaviour();
        }

        public void LateUpdateBehaviour()
        {
            character.LateUpdateBehaviour();
        }
    }
}