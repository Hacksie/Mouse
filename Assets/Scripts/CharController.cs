using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

namespace HackedDesign
{
    public class CharController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private PhysicsController body = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private CapsuleCollider2D collider = null;
        [SerializeField] Renderer renderer;

        [Header("Settings")]
        [SerializeField] private CharacterSettings settings = null;
        [SerializeField] private Material defaultMaterial = null;
        [SerializeField] private Material thermopticMaterial = null;
        [SerializeField] private bool camo = false;
        //[SerializeField] private UnityAction dieAction = null;
        //[SerializeField] private LayerMask doors = 0;

        public WeaponType currentWeapon = WeaponType.Unarmed;

        private float movement;

        private bool slideTriggered = false;
        private bool jumpTriggered = false;
        private bool jumpFlag = false;


        private float nextAttackTimer = int.MinValue;

        public bool IsAttacking { get => animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Punch") || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Kick") || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Sword") || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Combo") || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Gun"); }

        public bool Running { get; set; }
        public bool Crouched { get; set; }

        public bool Dead { get; private set; }

        public bool Camo { get { return camo; } set { camo = value; renderer.material = camo ? thermopticMaterial : defaultMaterial; } }


        public float Movement
        {
            get => movement;
            set
            {
                movement = value;
                
                //DesiredVelocity = new Vector2(movement, 0.0f) * Mathf.Max((Running ? (slideTriggered ? settings.slideSpeed : settings.runSpeed) : settings.walkSpeed) - body.Friction, 0f);
            }
        }

        public Vector2 DesiredVelocity
        {
            get
            {
                var speed = Crouched ? settings.crouchSpeed : (Running ? settings.runSpeed : settings.walkSpeed);

                // if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                // {
                //     return Vector2.zero;
                // }
                // else 
                // {
                return new Vector2(movement, 0.0f) * Mathf.Max(speed - body.Friction, 0f);
                //}

            }
        }

        public bool Jump
        {
            get => jumpFlag;
            set
            {
                if (!jumpFlag && value)
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
            renderer = GetComponent<Renderer>();
            //animator = GetComponent<Animator>();
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
            body.Freeze();
        }

        public void Go()
        {
            Dead = false;
            body.Unfreeze();
        }

        public void Attack()
        {
            //if (body.OnGround && Time.time >= nextAttackTimer)
            //{

            if (Time.time >= nextAttackTimer)
            {
                switch (currentWeapon)
                {
                    case WeaponType.Unarmed:
                        animator.SetFloat("meleeRng", Random.Range(0, 6));
                        animator.SetTrigger("meleeAttack");
                        break;
                    case WeaponType.Sword:
                        animator.SetFloat("swordRng", Random.Range(0, 5));
                        animator.SetTrigger("swordAttack");
                        break;
                    case WeaponType.Pistol:
                        animator.SetTrigger("pistolAttack");
                        break;
                }

                nextAttackTimer = Time.time + settings.attackRate;


            }

        }

        public void Roll()
        {
            //if (Mathf.Abs(Movement) >= Mathf.Epsilon)
            //{
                animator.SetTrigger("roll");
            //}
        }

        public void Slide()
        {
            //if (Mathf.Abs(Movement) >= Mathf.Epsilon)
            //{
                animator.SetTrigger("slide");
            //}
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
            if (Dead)
            {
                return;
            }

            UpdateSpriteDirection(Movement);

            if (this.jumpTriggered && Jump)
            {
                //SetAnimationState(new CurrentAnimation() {  state = "Jump", allowOverride = true });
                animator.SetTrigger("jump");
            }

            if (body.climbingLedge)
            {
                animator.ResetTrigger("jump");
            }

            animator.SetBool("ledgeStart", body.climbingLedge);

            jumpTriggered = false;


        }

        public void FixedUpdateBehaviour()
        {
            if (Dead)
            {
                return;
            }



            //if(body.LedgeEdgeStart())
            //{
            //    Debug.Log(body.LedgeEdgeStart());

            //    animator.SetTrigger("ledgeClimb");
            //    return;
            //}

            body.FixedMovement(DesiredVelocity, Jump, JumpHoldFlag);
            Jump = false;
            JumpHoldFlag = false;
            slideTriggered = false;
        }

        public void LateUpdateBehaviour()
        {

            Animate();
        }

        public void Die()
        {
            Dead = true;
            animator.SetTrigger("die");
            //dieAction.Invoke();
        }


        private void Animate()
        {

            var desiredVelocityMagnitude = DesiredVelocity.magnitude;
            //animator.SetBool("runToggle", Running && desiredVelocityMagnitude > 0.01f);
            animator.SetBool("crouched", Crouched);
            animator.SetBool("grounded", body.OnGround);
            animator.SetBool("hang", !body.OnGround && body.OnWall);
            animator.ResetTrigger("slide");
            animator.ResetTrigger("roll");
            animator.ResetTrigger("meleeAttack");
            animator.ResetTrigger("swordAttack");
            animator.ResetTrigger("pistolAttack");
            animator.SetBool("dead", Dead);
            animator.SetFloat("velocityY", body.Body.linearVelocityY);
            animator.SetFloat("velocityX", desiredVelocityMagnitude);
            animator.SetFloat("fallingTime", body.FallTime);
            //animator.SetBool("ledgeEnd", body.LedgeEdgeEnd());
        }
    }

    public enum WeaponType
    {
        Unarmed,
        Sword,
        Pistol
    }
}