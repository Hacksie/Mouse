using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class CharController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private PhysicsController body = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private CapsuleCollider2D collider = null;

        [Header("Settings")]
        [SerializeField] private CharacterSettings settings = null;

        public WeaponType currentWeapon = WeaponType.Unarmed;

        private float movement;

        private bool slideTriggered = false;
        private bool jumpTriggered = false;
        private bool jumpFlag = false;

        private float nextAttackTimer = 0;

        public bool Attacking { get; set; }
        public bool Crouched { get; set; }

        public float Movement
        {
            get => movement;
            set
            {
                movement = value;
                UpdateSpriteDirection(value);
                //DesiredVelocity = new Vector2(movement, 0.0f) * Mathf.Max((Attacking ? (slideTriggered ? settings.slideSpeed : settings.runSpeed) : settings.walkSpeed) - body.Friction, 0f);
            }
        }

        public Vector2 DesiredVelocity
        {
            get
            {
                // if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                // {
                //     return Vector2.zero;
                // }
                // else 
                // {
                    return new Vector2(movement, 0.0f) * Mathf.Max((Attacking ? (slideTriggered ? settings.slideSpeed : settings.runSpeed) : settings.walkSpeed) - body.Friction, 0f);
                //}
                
            }
        }

        public bool Jump
        {
            get => jumpFlag;
            set
            {
                if(!jumpFlag && value)
                {
                    jumpTriggered = true;
                }
                jumpFlag = value;

            }
        }
        public bool JumpHoldFlag { get; set; }

        void Awake()
        {
            body = GetComponent<PhysicsController>();
            animator = GetComponent<Animator>();
        }

        public void Reset()
        {
            this.animator.Play("Idle");
            UpdateSpriteDirection(1.0f);
            Crouched = false;
            Stop();
        }

        public void Stop()
        {
            body.Stop();
        }

        public void Attack()
        {
            //if (body.OnGround && Time.time >= nextAttackTimer)
            //{
                nextAttackTimer = Time.time + settings.attackRate;
                animator.SetFloat("attackRandom", Random.value);
                animator.SetTrigger("attack");
            //}

        }

        public void Roll()
        {
            if (Mathf.Abs(Movement) >= Mathf.Epsilon)
            {
                animator.SetTrigger("roll");
            }

        }

        public void Slide()
        {
            if (Mathf.Abs(Movement) >= Mathf.Epsilon)
            {
                animator.SetTrigger("slide");
            }
        }

        public void UpdateSpriteDirection(float movementDirection)
        {
            if (Mathf.Abs(movementDirection) >= Mathf.Epsilon)
            {
                transform.right = new Vector3(movementDirection, 0, 0);
            }
        }

        public void UpdateBehavior()
        {
            if (this.jumpTriggered && Jump)
            {
                Debug.Log("Jump");
                animator.SetTrigger("jump");
            }

            jumpTriggered = false;
            
        }

        public void FixedUpdateBehaviour()
        {
            body.FixedMovement(DesiredVelocity, Jump, JumpHoldFlag);
            Jump = false;
            JumpHoldFlag = false;
            slideTriggered = false;
        }

        public void LateUpdateBehaviour()
        {

            Animate();

            
        }

        private void Animate()
        {
            var desiredVelocityMagnitude = DesiredVelocity.magnitude;
            animator.SetFloat("weapon", (float)currentWeapon);
            animator.SetBool("running", Attacking && desiredVelocityMagnitude > 0);
            animator.SetBool("crouched", Crouched);
            animator.SetBool("grounded", body.OnGround);
            animator.ResetTrigger("slide");
            animator.ResetTrigger("roll");
        }
    }

    public enum WeaponType
    {
        Unarmed,
        PPK,
        Mateba
    }
}