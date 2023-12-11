#nullable enable
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class PlayerController : MonoBehaviour
    {
        private const string KEYBOARD = "Keyboard&Mouse";
        private const string GAMEPAD = "Gamepad";
        [Header("Game Objects")]
        [SerializeField] private CharacterController character;
        [SerializeField] private Camera lookCamera;
        [SerializeField] private Animator animator;
        [Header("Settings")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Settings settings;

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction attackAction;
        private InputAction dashAction;
        private Vector3 dashDirection;

        private bool isDashing = false;
        private float dashTimer = 0;

        private RaycastHit[] raycastHits = new RaycastHit[1];

        void Awake()
        {
            character = character != null ? character : GetComponent<CharacterController>();
            playerInput = playerInput != null ? playerInput : GetComponent<PlayerInput>();

            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            attackAction = playerInput.actions["Attack"];
            dashAction = playerInput.actions["Dash"];
            dashAction.performed += Dash;
        }


        public void UpdateBehaviour()
        {
            if (Game.Instance.State.Playing)
            {
                Vector2 moveDirection = moveAction.ReadValue<Vector2>();
                UpdateDash();
                Movement(moveDirection);
                Animate(moveDirection, isDashing);
            }
        }

        private void UpdateDash()
        {
            if (isDashing)
            {
                if (Time.time - dashTimer >= settings.dashTime)
                {
                    isDashing = false;
                }
            }
        }

        private void Dash(InputAction.CallbackContext context)
        {
            if (!isDashing)
            {
                dashTimer = Time.time;
                dashDirection = transform.forward;
                isDashing = true;
            }
        }

        // private void Look(Vector2 moveDirection)
        // {

        //     // if (fireAction.IsPressed())
        //     // {
        //     //     switch (playerInput.currentControlScheme)
        //     //     {
        //     //         case GAMEPAD:
        //     //             var lookDirection = lookAction.ReadValue<Vector2>().normalized;
        //     //             if (lookDirection.sqrMagnitude >= Mathf.Epsilon)
        //     //             {
        //     //                 var rotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.y), Vector3.up);
        //     //                 transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 45, 0);
        //     //             }
        //     //             break;
        //     //         case KEYBOARD:
        //     //             var mousePosition = lookAction.ReadValue<Vector2>();
        //     //             Ray ray = lookCamera.ScreenPointToRay(mousePosition);

        //     //             if (Physics.RaycastNonAlloc(ray, raycastHits, 100, settings.aimMask) > 0)
        //     //             {
        //     //                 var rotation = Quaternion.LookRotation(raycastHits[0].point - this.transform.position, Vector3.up);
        //     //                 transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        //     //             }

        //     //             break;
        //     //     }

        //     // }
        //     //else 
        //     if (moveDirection.sqrMagnitude >= Mathf.Epsilon)
        //     {
        //         var rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y), Vector3.up);
        //         transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 45, 0);
        //     }
        //     //transform.Rotate()
        // }

        private void Movement(Vector2 moveDirection)
        {
            if (moveDirection.sqrMagnitude >= Mathf.Epsilon)
            {
                var rotatedMovement = GetRotatedMovement(moveDirection);
                transform.forward = rotatedMovement;

                if (attackAction.IsInProgress())
                {
                    return; // Don't move if holding down fire
                }
                var movement = (rotatedMovement * settings.moveSpeed) + (isDashing ? dashDirection * settings.dashSpeed : Vector3.zero);

                character.Move(movement * Time.deltaTime);
            }
        }

        private static Vector3 GetRotatedMovement(Vector2 moveDirection)
        {
            return Quaternion.Euler(0, 45, 0) * new Vector3(moveDirection.x, 0, moveDirection.y);
        }

        private void Animate(Vector2 moveDirection, bool IsDashing)
        {
            animator.SetFloat("speed", moveDirection.magnitude);
            animator.SetBool("dashing", IsDashing);
        }
    }
}