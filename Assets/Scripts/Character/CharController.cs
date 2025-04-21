using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private CharacterState state = CharacterState.Idle;


        private bool jumpTriggered = false;
        private bool jumpFlag = false;
        private float nextAttackTimer = int.MinValue;
        private bool knockback = false;

        private bool inAir = false;

        public CharacterState State { get => state; set => state = value; }

        public bool IsAttacking
        {
            get
            {
                var clipName = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                return clipName == "Shoot" || clipName == "Gun Melee" || clipName == "Punch" || clipName == "Kick";
            }
        }
        public bool Crouched { get; set; }
        public float Movement { get; private set; }
        public float Climb { get; private set; }

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

        public OperatingSystem OperatingSystem { get => operatingSystem; set => operatingSystem = value; }
        public Animator Animator { get => animator; set => animator = value; }
        public PhysicsController Body { get => body; set => body = value; }
        public CharacterSettings Settings { get => settings; set => settings = value; }

        private Vector2 DesiredVelocity
        {
            get
            {
                var speed = state switch
                {
                    CharacterState.Idle => Crouched ? Settings.crouchSpeed : Settings.walkSpeed,
                    CharacterState.Battle => Crouched ? Settings.crouchSpeed : Settings.runSpeed,
                    _ => 0,
                };
                return new Vector2(Movement, 0.0f) * Mathf.Max(speed + operatingSystem.Momentum - Body.Friction, 0f);
            }
        }


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
        }


        public void Knockback(Vector3 direction)
        {
            knockback = true;
            body.Knockback(direction, Game.Instance.GameSettings.KnockbackAmount);
            animator.SetTrigger("knockback");
            operatingSystem.Momentum = 0;
            StartCoroutine(KnockbackPause());
            
        }

        private IEnumerator KnockbackPause()
        {
            yield return new WaitForSeconds(Game.Instance.GameSettings.KnockbackTime);
            Stop();
            StartCoroutine(KnockbackOver());
        }

        private IEnumerator KnockbackOver()
        {
            yield return new WaitForSeconds(Game.Instance.GameSettings.KnockbackFreezeTime);

            knockback = false;
        }

        private void Hit()
        {
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
            animator.SetBool("grounded", true);
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
                if (operatingSystem.HasAmmo && aiming && OperatingSystem.HasPistol)
                {
                    operatingSystem.DecreaseAmmo();
                    operatingSystem.Momentum -= settings.attackMomentumLoss;
                    Shoot(target);
                    Animator.SetTrigger("basicAttack");

                }
                else
                {
                    operatingSystem.Momentum -= settings.attackMomentumLoss;
                    Melee(target);
                    switch (Random.Range(0, 2 + (operatingSystem.HasAmmo && OperatingSystem.HasPistol ? 1 : 0)))
                    {
                        case 0:
                            Animator.SetTrigger("punch");
                            break;
                        case 1:
                            Animator.SetTrigger("kick");
                            break;
                        case 2:
                            Animator.SetTrigger("melee");
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

        public void MovePosition(Vector3 position)
        {
            transform.position = position;
            //Body.Body.MovePosition(position);
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
                case CharacterState.Seated:
                    UpdateSeatedBehaviour(movement, climb, facing);
                    break;
            }
        }

        private void UpdateDeadBehaviour()
        {
            //Animator.ResetTrigger("dead");
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

            if (Body.OnGround && !Crouched)
            {
                operatingSystem.Momentum += Time.deltaTime * settings.baseMomentumFactor;
            }
            else
            {
                operatingSystem.Momentum -= Time.deltaTime * settings.momentumAirLoss;
            }

            Animator.SetBool("ledgeStart", Body.climbingLedge);

            jumpTriggered = false;
        }

        private void UpdateIdleBehaviour(float movement, float climb, float facing)
        {
            Movement = movement;
            Climb = climb;

            UpdateSpriteDirection(Movement, facing);
        }

        private void UpdateSeatedBehaviour(float movement, float climb, float facing)
        {
        }

        public void FixedUpdateBehaviour()
        {
            if(IsDead())
            {
                return;
            }

            if (!knockback)
            {
                Debug.Log(DesiredVelocity);
                Body.FixedMovement(DesiredVelocity, Climb, Jump, JumpHoldFlag);
            }

            Jump = false;
            JumpHoldFlag = false;
        }

        public void LateUpdateBehaviour()
        {
            Animate();
        }

        public bool IsDead() => state == CharacterState.Dead;

        public void Die()
        {
            Debug.Log("char dead");
            Animator.SetBool("dead", true);
            Animator.SetTrigger("dying");
            state = CharacterState.Dead;
            dieActions?.Invoke();
            Body.Freeze();
            //collider.enabled = false;
        }

        public void Splat()
        {
            //collider.enabled = false;
            Animator.SetBool("dead", true);
            Animator.SetTrigger("splat");
            state = CharacterState.Dead;

            StartCoroutine(EndFall());
            //Body.Freeze();

        }

        private IEnumerator EndFall()
        {
            yield return new WaitForSeconds(settings.splatFallTime);
            Debug.Log("End fall");
            Body.Freeze();
            //collider.enabled = false;
            dieActions?.Invoke();
        }

        private void Shoot(Vector3 target)
        {
            if (target != null)
            {
                Debug.DrawLine(bulletStart.position, bulletStart.position + ((target - bulletStart.position).normalized * operatingSystem.settings.shootDistance), Color.red, 1);
                var result = Physics2D.Raycast(bulletStart.position, target - bulletStart.position, operatingSystem.settings.shootDistance, attackMask);

                if (result)
                {
                    if (result.transform.TryGetComponent<BreakGlass>(out var glass))
                    {
                        glass.Break(bulletStart.position);
                    }

                    if (result.transform.TryGetComponent<Enemy>(out var enemy))
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
            var result = Physics2D.OverlapCircle(bulletStart.position, operatingSystem.settings.meleeDistance, attackMask);

            //var result = Physics2D.Raycast(bulletStart.position, target - bulletStart.position, operatingSystem.settings.meleeDistance, attackMask);
            if (result)
            {
                if (result.transform.TryGetComponent<BreakGlass>(out var glass))
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
                    Animator.SetBool("rollOnLand", Body.FallTime > 0.66f);
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