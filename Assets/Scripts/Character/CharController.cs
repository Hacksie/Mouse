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
        [SerializeField] private new CapsuleCollider2D collider = null;

        [SerializeField] private Transform bulletStart;

        [Header("Settings")]
        [SerializeField] private CharacterSettings settings = null;
        [SerializeField] private LayerMask attackMask;
        [Header("State")]
        //[SerializeField] private CharStateIdle idleState;
        //[SerializeField] private CharStateSeated seatedState;
        //[SerializeField] private CharStateDead deadState;
        //[SerializeField] private CharStateBattle battleState;
        [SerializeField] private CharacterState state = CharacterState.Idle;

        //private float movement;

        private bool jumpTriggered = false;
        private bool jumpFlag = false;
        private float nextAttackTimer = int.MinValue;

        public bool IsAttacking { get => Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Shoot") || Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.StartsWith("Melee"); }
        public bool Crouched { get; set; }


        public float Movement {get; private set;}
        public float Climb {  get; private set;}

        public Vector2 DesiredVelocity
        {
            get
            {
                var speed = state switch
                {
                    CharacterState.Idle => Crouched ? Settings.crouchSpeed : Settings.walkSpeed,
                    CharacterState.Battle => Crouched ? Settings.crouchSpeed : Settings.runSpeed,
                    _ => 0,
                };
                return new Vector2(Movement, 0.0f) * Mathf.Max(speed - Body.Friction, 0f);
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
        public Animator Animator { get => animator; set => animator = value; }
        public PhysicsController Body { get => body; set => body = value; }
        public CharacterSettings Settings { get => settings; set => settings = value; }

        void Awake()
        {
            this.AutoBind(ref operatingSystem);
            this.AutoBind(ref body);
            this.AutoBind(ref collider);
            if (bulletStart == null)
            {
                bulletStart = transform;
            }
            operatingSystem.changeActions += Hit;
            //SetStateIdle();
            //animator = GetComponent<Animator>();
        }

        //public void SetStateIdle() => CharState = new CharStateIdle(this);
        //public void SetStateSeated() => CharState = new CharStateSeated(this);
        //public void SetStateDead() => CharState = new CharStateDead(this);
        //public void SetStateBattle() => CharState = new CharStateBattle(this);

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
            //SetStateIdle();

        }

        public void Stop()
        {
            Body.Freeze();
        }


        public void Attack(Vector3 target, bool aiming)
        {
            
            if (state != CharacterState.Battle)
            {
                return;
            }

            if (Time.time >= nextAttackTimer)
            {
                if (operatingSystem.HasAmmo && aiming)
                {
                    operatingSystem.DecreaseAmmo();
                    Shoot(target);
                    Animator.SetTrigger("basicAttack");

                }
                else
                {
                    Melee(target);
                    switch(Random.Range(0,3))
                    {
                        case 0:
                            Animator.SetTrigger("melee");
                            break;
                        case 1:
                            Animator.SetTrigger("punch");
                            break;
                        case 2:
                            Animator.SetTrigger("kick");
                            break;
                    }
                    

                }

                nextAttackTimer = Time.time + Settings.attackRate;
            }

        }

        public void Roll()
        {
            if (Mathf.Abs(Movement) >= Mathf.Epsilon)
            {
                Animator.SetTrigger("roll");
            }
        }



        public void UpdateBehaviour(float movement, float climb, float facing)
        {
            switch (state)
            {
                case CharacterState.Dead:
                    UpdateDeadBehaviour();
                    break;
                case CharacterState.Battle:
                    UpdateBattleBehaviour(movement, climb, facing);
                    break;
                case CharacterState.Idle:
                    UpdateIdleBehaviour(movement, climb, facing);
                    break;
            }
        }

        private void UpdateDeadBehaviour()
        {
            Animator.ResetTrigger("die");
        }

        private void UpdateBattleBehaviour(float movement, float climb, float facing)
        {
            Movement = movement;
            Climb = climb;

            UpdateSpriteDirection(Movement, facing);

            if (this.jumpTriggered && Jump)
            {
                Animator.SetTrigger("jump");
            }

            if (Body.climbingLedge)
            {
                Animator.ResetTrigger("jump");
            }

            Animator.SetBool("ledgeStart", Body.climbingLedge);

            jumpTriggered = false;
        }

        private void UpdateIdleBehaviour(float movement, float climb, float facing)
        {
            Movement = movement;
            Climb = climb;

            UpdateSpriteDirection(Movement, facing);

            /*
            if (this.jumpTriggered && Jump)
            {
                animator.SetTrigger("jump");
            }

            if (body.climbingLedge)
            {
                animator.ResetTrigger("jump");
            }

            animator.SetBool("ledgeStart", body.climbingLedge);

            jumpTriggered = false;*/
        }

        public void FixedUpdateBehaviour()
        {
            switch (state)
            {
                case CharacterState.Dead:
                    return;

            }

            Body.FixedMovement(DesiredVelocity, Climb, Jump, JumpHoldFlag);
            Jump = false;
            JumpHoldFlag = false;
        }

        public void LateUpdateBehaviour()
        {
            Animate();
        }

        public void Die()
        {
            Debug.Log("char dead");
            Animator.SetBool("dead", true);
            Animator.SetTrigger("dying");
            //SetStateDead();
            state = CharacterState.Dead;
            dieActions?.Invoke();
            Body.Freeze();
            collider.enabled = false;
        }

        private void Shoot(Vector3 target)
        {
            if (target != null)
            {
                Debug.DrawLine(bulletStart.position, bulletStart.position + ((target - bulletStart.position).normalized * operatingSystem.settings.shootDistance), Color.red, 1);
                var result = Physics2D.Raycast(bulletStart.position, target - bulletStart.position, operatingSystem.settings.shootDistance, attackMask);

                if (result) 
                {
                    if(result.transform.TryGetComponent<BreakGlass>(out var glass))
                    {
                        glass.Break(bulletStart.position);
                    }

                    if(result.transform.TryGetComponent<Enemy>(out var enemy))
                    {
                        var damage = Random.Range(operatingSystem.settings.minBulletDamage, operatingSystem.settings.maxBulletDamage);
                        enemy.TakeDamage(damage, result.point);
                    }

                    Debug.Log(result.transform.name);
                }
            }
        }

        private void Melee(Vector3 target)
        {
            //FIXME: Don't use a ray to melee. Use a box around the player
            Debug.DrawLine(bulletStart.position, bulletStart.position + (transform.right * operatingSystem.settings.meleeDistance), Color.magenta, 1);
            var result = Physics2D.OverlapCircle(bulletStart.position, operatingSystem.settings.meleeDistance, attackMask);
            
            //var result = Physics2D.Raycast(bulletStart.position, target - bulletStart.position, operatingSystem.settings.meleeDistance, attackMask);
            if (result)
            {
                if(result.transform.TryGetComponent<BreakGlass>(out var glass))
                {
                    glass.Break(bulletStart.position);
                }

                if (result.transform.TryGetComponent<Enemy>(out var enemy))
                {
                    var damage = Random.Range(operatingSystem.settings.minBulletDamage, operatingSystem.settings.maxBulletDamage);
                    enemy.TakeDamage(damage, result.ClosestPoint(bulletStart.position));
                }

                Debug.Log(result.transform.name);
            }
        }

        private void UpdateSpriteDirection(float movementDirection, float facingDirection)
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


        private void Animate()
        {
            switch (state)
            {
                case CharacterState.Dead:
                    Animator.SetBool("dead", true);
                    break;
                case CharacterState.Seated:
                    Animator.SetBool("sit", true);
                    Animator.SetBool("grounded", true);
                    Animator.SetBool("dead", false);
                    break;
                default:
                    Animator.SetBool("sit", false);
                    Animator.ResetTrigger("roll");
                    Animator.ResetTrigger("melee");
                    Animator.ResetTrigger("basicAttack");
                    Animator.ResetTrigger("strongAttack");
                    Animator.SetBool("dead", false);
                    Animator.SetBool("crouched", Crouched);
                    Animator.SetBool("grounded", Body.OnGround);
                    Animator.SetBool("hang", !Body.OnGround && Body.OnWall);
                    Animator.SetFloat("battleMode", state == CharacterState.Battle ? 1 : 0);
                    Animator.SetFloat("velocityY", Body.Body.linearVelocityY);
                    Animator.SetFloat("velocityX", DesiredVelocity.magnitude);
                    Animator.SetFloat("fallingTime", Body.FallTime);
                    break;
            }
        }
    }

    public enum CharacterState
    {
        Seated,
        Idle,
        Battle,
        Dead
    }
}