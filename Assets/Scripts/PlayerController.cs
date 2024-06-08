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
        //[SerializeField] private Camera cam = null;



        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction crouchAction;
        private InputAction slideAction;
        private InputAction rollAction;
        private InputAction attackAction;
        private InputAction menuAction;
        private InputAction selectWeapon1Action;
        private InputAction selectWeapon2Action;
        private InputAction selectWeapon3Action;

        private bool shoot = false;

        void Awake()
        {
            BindInputs();
        }

        private void BindInputs()
        {
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            crouchAction = playerInput.actions["Crouch"];
            //slideAction = playerInput.actions["Slide"];
            rollAction = playerInput.actions["Roll"];
            attackAction = playerInput.actions["Attack"];
            menuAction = playerInput.actions["Menu"];
            selectWeapon1Action = playerInput.actions["Select Weapon 1"];
            selectWeapon2Action = playerInput.actions["Select Weapon 2"];
            selectWeapon3Action = playerInput.actions["Select Weapon 3"];
            menuAction.performed += MenuEvent;
            selectWeapon1Action.performed += SelectWeapon1Event;
            selectWeapon2Action.performed += SelectWeapon2Event;
            selectWeapon3Action.performed += SelectWeapon3Event;
        }

        void Start()
        {
            Reset();
        }

        public void Reset()
        {
            character.currentWeapon = WeaponType.Unarmed;
            //this.transform.position = new Vector3(0, 0f, 0); // 0.275f
            character.Attacking = true;
            character.Reset();
        }

        public void Stop()
        {
            character.Stop();
            shoot = false;
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
            character.currentWeapon = WeaponType.PPK;

        }

        public void SelectWeapon3Event(InputAction.CallbackContext context)
        {
            character.currentWeapon = WeaponType.Mateba;
        }


        public void UpdateBehavior()
        {
            var movement = moveAction.ReadValue<float>();

            character.Crouched = this.crouchAction.IsPressed();
            //character.Jump |= this.jumpAction.triggered;
            //character.JumpHoldFlag |= this.jumpAction.IsPressed();

            character.Movement = movement;


            // if (this.rollAction.triggered)
            // {
            //     character.Roll();
            // }

            // if (this.attackAction.triggered)
            // {
            //     character.Attack();
            // }

            character.UpdateBehavior();

            // if (this.slideAction.triggered)
            // {
            //     character.Slide();
            // }
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