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
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private CharController character = null;
        [SerializeField] private DoorUser doorUser = null;
        [SerializeField] private bool runToggle = false;
        [SerializeField] private bool crouchToggle = false;
        //[SerializeField] private Camera cam = null;



        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction crouchAction;
        private InputAction runAction;
        private InputAction slideAction;
        private InputAction rollAction;
        private InputAction camoAction;
        private InputAction attackAction;
        private InputAction menuAction;
        private InputAction doorUpAction;
        private InputAction doorDownAction;
        private InputAction selectWeapon1Action;
        private InputAction selectWeapon2Action;
        private InputAction selectWeapon3Action;

        private bool shoot = false;

        void Awake()
        {
            BindInputs();

            doorUser = GetComponent<DoorUser>();
        }

        private void BindInputs()
        {
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            crouchAction = playerInput.actions["Crouch"];
            runAction = playerInput.actions["Run"];
            rollAction = playerInput.actions["Roll"];
            attackAction = playerInput.actions["Attack"];
            camoAction = playerInput.actions["Camo"];
            menuAction = playerInput.actions["Menu"];
            doorUpAction = playerInput.actions["Door Up"];
            doorDownAction = playerInput.actions["Door Down"];
            selectWeapon1Action = playerInput.actions["Select Weapon 1"];
            selectWeapon2Action = playerInput.actions["Select Weapon 2"];
            selectWeapon3Action = playerInput.actions["Select Weapon 3"];
            menuAction.performed += MenuEvent;
            selectWeapon1Action.performed += SelectWeapon1Event;
            selectWeapon2Action.performed += SelectWeapon2Event;
            selectWeapon3Action.performed += SelectWeapon3Event;
            camoAction.performed += CamoEvent;
            doorUpAction.performed += DoorUpEvent;
            doorDownAction.performed += DoorDownEvent;
        }

        void Start()
        {
            Reset();
        }

        public void Ping()
        {
            var results = Physics2D.OverlapCircleAll(this.transform.position, 10);

            foreach (var result in results)
            {
                if((result.CompareTag("Interactable") || result.CompareTag("Player") || result.CompareTag("Enemy")))
                {
                    var hl = result.GetComponent<Highlightable>();
                    if(hl)
                    {
                        hl.Ping();
                    }
                }
            }
        }

        public void Reset()
        {
            character.currentWeapon = WeaponType.Unarmed; // WeaponType.Sword;
            //this.transform.position = new Vector3(0, 0f, 0); // 0.275f 
            character.Running = runToggle;
            Stop();
            character.Reset();
        }

        public void Stop()
        {
            character.Stop();
            shoot = false;
        }

        public void Go()
        {
            character.Go();
        }

        public void CamoEvent(InputAction.CallbackContext context)
        {
            character.Camo = !character.Camo;
        }

        public void MenuEvent(InputAction.CallbackContext context)
        {
            Game.Instance.CurrentState.Menu();
        }

        public void SelectWeapon1Event(InputAction.CallbackContext context)
        {
            character.currentWeapon = WeaponType.Unarmed;

        }

        public void SelectWeapon2Event(InputAction.CallbackContext context)
        {
            character.currentWeapon = WeaponType.Sword;

        }

        public void SelectWeapon3Event(InputAction.CallbackContext context)
        {
            character.currentWeapon = WeaponType.Pistol;
        }

        public void DoorUpEvent(InputAction.CallbackContext context)
        {
            doorUser?.TryGoUpDoor();
        }

        public void DoorDownEvent(InputAction.CallbackContext context)
        {
            doorUser?.TryGoDownDoor();
        }


        public void UpdateBehavior()
        {
            character.Movement = 0;
            //character.Jump = false;
            character.Running = runToggle;

            var movement = moveAction.ReadValue<float>();
            if (character.Dead)
                return;

            if (character.IsAttacking)
            {
                //Debug.Log("Is attacking");
            }
            else if (this.attackAction.triggered)
            {
                character.Attack();
            }
            else if (this.crouchAction.triggered)
            {
                crouchToggle = !crouchToggle;
            }
            else if (this.runAction.triggered)
            {
                runToggle = !runToggle;
            }
            else if (this.rollAction.triggered)
            {
                character.Roll();
            }
               
            else
            {
                character.Jump |= this.jumpAction.triggered;
                character.JumpHoldFlag |= this.jumpAction.IsPressed();
                character.Movement = movement;
            }

            
            character.Crouched = crouchToggle;

            character.UpdateBehavior();

            //if (this.slideAction.triggered)
            //{
            //    character.Slide();
            //}
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