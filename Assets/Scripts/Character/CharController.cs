using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
//using static UnityEngine.Rendering.DebugUI;

namespace HackedDesign
{
    public class CharController : MonoBehaviour
    {
        [Header("Actions")]
        [SerializeField] public UnityEvent dieActions;
        [SerializeField] public UnityEvent hitActions;
        [Header("Game Objects")]
        [SerializeField] private OperatingSystem operatingSystem;
        //[SerializeField] private CharacterData characterData;
        [SerializeField] private PhysicsController body = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private new Collider2D collider = null;

        [SerializeField] private Transform bulletStart;

        [Header("Settings")]
        [SerializeField] private CharacterSettings settings = null;
        [Header("State")]
        [SerializeField] private CharacterState state = CharacterState.Idle;

        private float movement;

        private bool jumpTriggered = false;
        private bool jumpFlag = false;
        private float nextAttackTimer = int.MinValue;

        public bool IsAttacking { get => animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Shoot") || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Melee"); }
        public bool Crouched { get; set; }


        public float Movement
        {
            get => movement;
            set
            {
                movement = value;
            }
        }

        public Vector2 DesiredVelocity
        {
            get
            {
                var speed = state switch
                {
                    CharacterState.Idle => Crouched ? settings.crouchSpeed : settings.walkSpeed,
                    CharacterState.Battle => Crouched ? settings.crouchSpeed : settings.runSpeed,
                    _ => 0,
                };
                return new Vector2(movement, 0.0f) * Mathf.Max(speed - body.Friction, 0f);
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
        //public CharacterData CharacterData { get => characterData; set => characterData = value; }
        public CharacterState State { get => state; set => state = value; }
        public OperatingSystem OperatingSystem { get => operatingSystem; set => operatingSystem = value; }

        void Awake()
        {
            this.AutoBind<OperatingSystem>(ref operatingSystem);
            this.AutoBind<PhysicsController>(ref body);
            this.AutoBind(ref collider);
            if (bulletStart == null)
            {
                bulletStart = transform;
            }
            operatingSystem.changeActions += Hit;
            //animator = GetComponent<Animator>();
        }

        private void Hit()
        {
            // Play hit animation
            if (operatingSystem.Health <= 0)
            {
                Debug.Log(transform.name + " is dead");
                Die();
            }
            else
            {
                hitActions?.Invoke();
            }
        }

        public void Reset()
        {
            //this.animator.Play("Idle");
            UpdateSpriteDirection(1.0f, 1.0f);
            Crouched = false;
            Stop();
        }

        public void Stop()
        {
            body.Freeze();
        }


        public void Attack(Vector3 target)
        {
            if (state != CharacterState.Battle)
            {
                return;
            }

            if (Time.time >= nextAttackTimer)
            {
                if (operatingSystem.HasAmmo)
                {
                    operatingSystem.DecreaseAmmo();
                    Shoot(target);
                    animator.SetTrigger("basicAttack");

                }
                else
                {
                    Melee(target);
                    animator.SetTrigger("melee");

                }

                nextAttackTimer = Time.time + settings.attackRate;
            }

        }

        private void Shoot(Vector3 target)
        {
            if (target != null)
            {
                Debug.DrawLine(bulletStart.position, bulletStart.position + ((target - bulletStart.position).normalized * operatingSystem.settings.shootDistance), Color.red, 1);
                var result = Physics2D.Raycast(bulletStart.position, target - bulletStart.position, operatingSystem.settings.shootDistance);

                //Physics2D.Raycast()

                if (result && (result.transform.CompareTag("Enemy") || result.transform.CompareTag("Glass") || result.transform.CompareTag("Interactable")))
                {
                    if (result.transform.CompareTag("Glass"))
                    {
                        var glass = result.transform.GetComponent<BreakGlass>();
                        if (glass != null)
                        {
                            glass.Break(bulletStart.position);
                        }
                    }

                    if (result.transform.CompareTag("Enemy"))
                    {
                        var enemy = result.transform.GetComponent<Enemy>();
                        var damage = Random.Range(operatingSystem.settings.minBulletDamage, operatingSystem.settings.maxBulletDamage);
                        enemy.TakeDamage(damage, result.point);
                    }

                    Debug.Log(result.transform.name);
                }
            }
        }

        private void Melee(Vector3 target)
        {
            Debug.DrawLine(bulletStart.position, bulletStart.position + ((target - bulletStart.position).normalized * operatingSystem.settings.meleeDistance), Color.magenta, 1);
            var result = Physics2D.Raycast(bulletStart.position, target - bulletStart.position, operatingSystem.settings.meleeDistance);
            if (result && (result.transform.CompareTag("Enemy") || result.transform.CompareTag("Glass") || result.transform.CompareTag("Interactable")))
            {
                if (result.transform.CompareTag("Glass"))
                {
                    var glass = result.transform.GetComponent<BreakGlass>();
                    if (glass != null)
                    {
                        glass.Break(bulletStart.position);
                    }
                }

                Debug.Log(result.transform.name);
            }
        }

        public void Roll()
        {
            if (Mathf.Abs(Movement) >= Mathf.Epsilon)
            {
                animator.SetTrigger("roll");
            }
        }

        public void UpdateSpriteDirection(float movementDirection, float facingDirection)
        {
            if (Mathf.Abs(movementDirection) >= Mathf.Epsilon)
            {
                transform.right = new Vector3(movementDirection, 0, 0);
            }
            else if (Mathf.Abs(facingDirection) >= Mathf.Epsilon)
            {
                transform.right = new Vector3(facingDirection, 0, 0);
            }
        }

        public void UpdateBehavior(float movement, float facing)
        {
            switch (state)
            {
                case CharacterState.Dead:
                    animator.ResetTrigger("die");
                    return;

            }


            Movement = movement;

            UpdateSpriteDirection(Movement, facing);

            if (this.jumpTriggered && Jump)
            {
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
            switch (state)
            {
                case CharacterState.Dead:
                    return;

            }

            body.FixedMovement(DesiredVelocity, Jump, JumpHoldFlag);
            Jump = false;
            JumpHoldFlag = false;
        }

        public void LateUpdateBehaviour()
        {
            Animate();
        }

        public void Die()
        {
            //Dead = true;
            Debug.Log("char dead");
            animator.SetBool("dead", true);
            animator.SetTrigger("dying");
            state = CharacterState.Dead;
            dieActions?.Invoke();
            body.Freeze();
            collider.enabled = false;
        }


        private void Animate()
        {
            //Debug.Log(state);
            switch(state)
            {
                case CharacterState.Dead:
                    animator.SetBool("dead", true);
                    break;
                default:
                    animator.ResetTrigger("roll");
                    animator.ResetTrigger("melee");
                    animator.ResetTrigger("basicAttack");
                    animator.ResetTrigger("strongAttack");
                    animator.SetBool("dead", false);
                    animator.SetBool("crouched", Crouched);
                    animator.SetBool("grounded", body.OnGround);
                    animator.SetBool("hang", !body.OnGround && body.OnWall);
                    animator.SetFloat("battleMode", state == CharacterState.Battle ? 1 : 0);
                    animator.SetFloat("velocityY", body.Body.linearVelocityY);
                    animator.SetFloat("velocityX", DesiredVelocity.magnitude);
                    animator.SetFloat("fallingTime", body.FallTime);
                    break;
            }
        }
    }

    public enum CharacterState
    {
        Idle,
        Battle,
        Dead
    }
}