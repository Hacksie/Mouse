using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class CharController : MonoBehaviour, ICharacterExecute
    {
        [Header("Actions")]
        [SerializeField] public UnityEvent dieActions;
        [SerializeField] public UnityEvent hitActions;
        [Header("Game Objects")]
        [SerializeField] private OperatingSystem operatingSystem;
        [SerializeField] private PhysicsController body = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private new Collider2D collider = null;
        [SerializeField] private Transform head;
        [Header("Settings")]
        [SerializeField] private CharacterSettings settings = null;


        private bool jumpFlag = false;
        private bool knockback = false;

        #region State
        private ICharacterState currentState;

        public ICharacterState CurrentState
        {
            get { return currentState; }
            set
            {
                currentState?.End();
                currentState = value;
                currentState?.Begin();
            }
        }

        public void SetIdleState() { CurrentState = new CharacterIdleState(this, Animator); }
        public void SetBattleState() { CurrentState = new CharacterBattleState(this, Animator); }
        public void SetSitState() { CurrentState = new CharacterSittingState(Animator); }
        public void SetDeadState() { CurrentState = new CharacterDeadState(Animator); }
        #endregion State

        public OperatingSystem OperatingSystem { get => operatingSystem; private set => operatingSystem = value; }
        public Animator Animator { get => animator; set => animator = value; }
        public PhysicsController Body { get => body; set => body = value; }
        public CharacterSettings Settings { get => settings; set => settings = value; }

        public Transform Head { get => head; }

        public bool IsAnimatingAttack { get => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"); }
        public bool IsCrouched { get; private set; }

        public bool JumpHoldFlag { get; set; }
        public bool IsWalking { get; private set; } = false;

        private bool Aiming { get; set; } = false;
        private float DesiredMovement { get => InputDirection * Mathf.Max(CurrentState.CurrentSpeed(new CharacterSpeedContext() { crouched = IsCrouched, crouchSpeed = Settings.CrouchSpeed, walk = IsWalking, walkSpeed = Settings.WalkSpeed, runSpeed = Settings.RunSpeed }), 0f); }
        //private bool JumpTriggered { get; set; }
        private bool Jump
        {
            get => jumpFlag;
            set
            {
                if (!IsDead && !jumpFlag && value)
                {
                    //JumpTriggered = true;
                    Animator.SetTrigger(AnimatorParams.Jump);
                }
                jumpFlag = value;
            }
        }
        private float InputDirection { get; set; }
        private float InputClimb { get; set; }

        void Awake()
        {
            this.AutoBind(ref operatingSystem);
            this.AutoBind(ref body);
            this.AutoBind(ref collider);
            if (head == null)
            {
                head = transform;
            }
            operatingSystem.hitActions += Hit;
            operatingSystem.dieActions += Die;
            SetIdleState();
        }

        #region Commands
        public void SetCrouch(bool flag) => IsCrouched = flag;

        public void SetMovement(float inputDirection, float inputClimb)
        {
            InputDirection = inputDirection;
            InputClimb = inputClimb;
        }

        public void SetJump() => Jump = true;
        public void ClearJump() => Jump = false;

        public void WalkToggle() => IsWalking = !IsWalking;
        public void SetAim(bool flag) => Aiming = flag;
        public void Knockback(Vector3 direction)
        {
            knockback = true;
            body.Knockback(direction, Game.Instance.GameSettings.KnockbackAmount);
            animator.SetTrigger(AnimatorParams.Knockback);
            //operatingSystem.Momentum = 0;
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

        public void TriggerInteract() => animator.SetTrigger(AnimatorParams.Interact);

        public void Roll()
        {
            if (Mathf.Abs(InputDirection) >= Mathf.Epsilon)
            {
                Animator.SetTrigger(AnimatorParams.Roll);
            }
        }

        public void Stop()
        {
            Body.Stop();
        }

        #endregion Commands


        public void Reset()
        {
            Stop();
            OperatingSystem.Reset(Settings);
            animator.SetBool(AnimatorParams.Grounded, true);
            SetCrouch(false);
            UpdateSpriteDirection(1f, 1f);
        }

        public void Physics()
        {
            if (IsDead)
            {
                return;
            }

            if (!knockback)
            {
                Body.FixedMovement(DesiredMovement, InputClimb, Jump, JumpHoldFlag);
            }

            Jump = false;
            JumpHoldFlag = false;
        }

        public void Animate()
        {
            //JumpTriggered = false;

            if (Body.currentlyClimbingLedge)
            {
                Animator.ResetTrigger(AnimatorParams.Jump);
            }
            //Animator.SetBool(AnimatorParams.LedgeStart, Body.currentlyClimbingLedge);
            CurrentState.ResetAnimationTriggers();
            CurrentState.Animate(new CharacterAnimationContext()
            {
                crouched = IsCrouched,
                onGround = Body.OnGround,
                onWall = Body.OnWall,
                velocityY = Body.VelocityY,
                movementMagnitude = Mathf.Abs(DesiredMovement),
                rollOnLand = Body.LastFallTime > 1f,
                aiming = Aiming,
                isClimbingLedge = Body.currentlyClimbingLedge,
            });
        }

        public bool CanHear(Vector3 position)
        {
            var vectorToPlayer = Head.transform.position - position;
            var hearDistance = IsCrouched ? 0f : IsWalking ? 1f : 4f;

            return vectorToPlayer.magnitude < hearDistance;
        }

        public RaycastHit2D? CanSee(Vector3 position, float maxVisualRange, LayerMask lineOfSightMask)
        {
            var vectorToPlayer = Head.transform.position - position;

            if (vectorToPlayer.sqrMagnitude > (maxVisualRange * maxVisualRange)) // Don't bother if they're well out of range
            {
                return null;
            }

            return Physics2D.Raycast(position, vectorToPlayer.normalized, Settings.ShootDistance, lineOfSightMask);
        }


        #region Death
        public bool IsDead { get => !CurrentState.IsAlive; }
        private void Die()
        {
            if (IsDead)
            {
                return;
            }

            Animator.SetBool(AnimatorParams.Dead, true);
            Animator.SetTrigger(AnimatorParams.Dying);
            SetDeadState();
            //state = CharacterState.Dead;
            Body.Stop();
            Body.Freeze();
            collider.enabled = false;
            dieActions?.Invoke();
        }

        public void Splat()
        {
            Animator.SetBool(AnimatorParams.Dead, true);
            Animator.SetTrigger(AnimatorParams.Splat);
            SetDeadState();

            StartCoroutine(EndFall());
        }

        private IEnumerator EndFall()
        {
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorParams.IsDeadTag));
            dieActions?.Invoke();
        }
        #endregion Death

        #region Attack
        public void Attack(Vector3 target, bool aiming) => CurrentState.Attack(new CharacterAttackContext()
        {
            pivot = Head.position,
            target = target,
            aiming = aiming,
            isPlayer = CompareTag(Tags.Player),
            operatingSystem = OperatingSystem,
            settings = Settings,
        });
             

        #endregion Attack

        #region Health

        public void TakeDamage(int minAmount, int maxAmount, Vector3 contact)
        {
            var amount = Random.Range(minAmount, maxAmount);
            FXPool.Instance.Spawn(FXType.Blood, contact, transform.position - contact);
            OperatingSystem.Health -= amount;
        }

        private void Hit() => hitActions?.Invoke();

        #endregion Health

        #region Animation
        public void UpdateSpriteDirection(float movementDirection, float facingDirection)
        {
            if (Body.OnWall)
            {
                transform.right = Body.ContactNormal.x < 0 ? Vector3.right : Vector3.left;
                return;
            }

            if (Mathf.Abs(movementDirection) >= Mathf.Epsilon)
            {
                transform.right = new Vector3(movementDirection, 0, 0);
            }
            else if (Mathf.Abs(facingDirection) >= Mathf.Epsilon)
            {
                transform.right = new Vector3(facingDirection, 0, 0);
            }
        }

        #endregion Animation

        public void ExecuteCommand(ICharacterCommand cmd)
        {
            cmd.Execute(this);
        }

    }

    public struct CharacterAttackContext
    {
        public Vector3 pivot;
        public Vector3 target;
        public bool aiming;
        public bool isPlayer;
        public OperatingSystem operatingSystem;
        public CharacterSettings settings;
    }

    public struct CharacterSpeedContext
    {
        public bool crouched;
        public float crouchSpeed;
        public bool walk;
        public float walkSpeed;
        public float runSpeed;
    }

    public struct CharacterAnimationContext
    {
        public bool crouched;
        public bool onGround;
        public bool onWall;
        public float velocityY;
        public float movementMagnitude;
        public bool rollOnLand;
        public bool aiming;
        public bool isClimbingLedge;
    }

    public interface ICharacterExecute
    {
        public void ExecuteCommand(ICharacterCommand cmd);
    }

    public interface ICharacterCommand
    {
        void Execute(CharController controller);
    }

    public class AimCommand: ICharacterCommand
    {
        private readonly bool state;

        public AimCommand(bool state) => this.state = state;
        public void Execute(CharController controller) => controller.SetAim(state);
    }

    public class InteractCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.TriggerInteract();
    }

    public class FacingCommand: ICharacterCommand
    {
        private readonly float movementDirection;
        private readonly float facingDirection;
        public FacingCommand(float movementDirection, float facingDirection)
        {
            this.movementDirection = movementDirection;
            this.facingDirection = facingDirection;
        }

        public void Execute(CharController controller) => controller.UpdateSpriteDirection(movementDirection, facingDirection);
    }

    public class CrouchCommand: ICharacterCommand
    {
        private readonly bool state;
        public CrouchCommand(bool state) => this.state = state;
        public void Execute(CharController controller) => controller.SetCrouch(state);
    }

    public class WalkToggleCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.WalkToggle();

    }

    public class RolLCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.Roll();
    }

    public class JumpCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.SetJump();
    }

    public class MoveCommand : ICharacterCommand
    {
        private readonly float movementDirection;
        private readonly float climb;
        public MoveCommand(float movementDirection, float climb)
        {
            this.movementDirection = movementDirection;
            this.climb = climb;
        }
        public void Execute(CharController controller) => controller.SetMovement(movementDirection, climb);
    }

    public class KnockbackCommand: ICharacterCommand
    {
        private Vector3 direction;
        public KnockbackCommand(Vector3 direction) => this.direction = direction;
        public void Execute(CharController controller) => controller.Knockback(direction);

    }

    public class StopCommand : ICharacterCommand
    {
        public void Execute(CharController controller) => controller.Stop();
    }
}