#nullable enable
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    [RequireComponent(typeof(CharController))]
    public class EnemyController : MonoBehaviour, IAi
    {
        [Header("Actions")]
        [SerializeField] private UnityAction hitBehaviour;
        [SerializeField] private UnityAction deathBehaviour;
        [Header("Game Objects")]
        [SerializeField] private CharController character;
        [SerializeField] private StatusIcon characterStatusIcon;
        [SerializeField] private Transform? aimPivot = null;
        [Header("Settings")]
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private LayerMask lineOfSightMask;
        [SerializeField] private LayerMask movementMask;
        [SerializeField] private EnemySettings enemySettings;

        private PlayerController? player = null;

        private IEnemyState currentState;

        public IEnemyState CurrentState
        {
            get => this.currentState;
            set
            {
                currentState?.End();
                currentState = value;
                currentState?.Begin();
            }
        }

        public StatusIcon Icon => characterStatusIcon;

        public bool HasSeenPlayer
        {
            get; private set;
        }

        public Vector3 LastKnownPlayerPosition { get; private set; }

        public bool CanSeePlayer { get; private set; }

        public bool CanHearPlayer { get; private set; }

        public bool HasBeenAlerted { get; private set; }

        public bool HasSeenDeadEnemies { get; private set; } = false;

        public bool PlayerInFrontOfUs
        {
            get
            {
                var facing = this.player != null ? this.player.transform.position.x <= this.transform.position.x ? -1 : 1 : -1;

                return Mathf.Sign(facing) == Mathf.Sign(transform.right.x);
            }
        }

        public EnemySettings EnemySettings { get => enemySettings; private set => enemySettings = value; }
        public CharController Character { get => character; private set => character = value; }

        public bool WallInFront
        {
            get
            {
                var boxA =  new Vector2(transform.position.x + (transform.right.x * 0.25f), transform.position.y + 0.25f);
                var boxB = new Vector2(boxA.x + (transform.right.x * 0.5f), boxA.y + (2f - 0.5f));
#if UNITY_EDITOR
                if (Application.isPlaying && Application.isEditor)
                {
                    Debug.DrawLine(boxA, boxB, Color.green);
                }
#endif
                return Physics2D.OverlapArea(boxA, boxB, movementMask);
            }
        }

        public bool DropInFront
        {
            get
            {
                if(character.Body == null || (character.Body != null && character.Body.Flying))
                {
                    return false;
                }

                var boxA = new Vector2(transform.position.x + (transform.right.x * 0.25f), transform.position.y);
                var boxB = new Vector2(boxA.x + (transform.right.x * 0.5f), boxA.y - 0.25f);
#if UNITY_EDITOR
                if (Application.isPlaying && Application.isEditor)
                {
                    Debug.DrawLine(boxA, boxB, Color.green);
                }
#endif
                return !Physics2D.OverlapArea(boxA, boxB, movementMask);
            }
        }

        public UnityAction HitBehaviour { get => this.hitBehaviour; set => this.hitBehaviour = value; }
        public UnityAction DeathBehaviour { get => this.deathBehaviour; set => this.deathBehaviour = value; }
        public EnemyType EnemyType { get => this.enemyType; set => this.enemyType = value; }

        void Awake()
        {
            this.AutoBind(ref character);
            this.characterStatusIcon = GetComponentInChildren<StatusIcon>();
            Character.DieActions.AddListener(Die);
            Character.HitActions.AddListener(Hit);
            CurrentState = new EnemyIdleState(this);
        }

        private void Start()
        {
            this.player = Game.Instance.Player;
            Reset();
        }

        private void UpdateDetect()
        {
            if(!this.player.EnsureNotNull(this,nameof(this.player)) || !this.player.Character.EnsureNotNull(this, nameof(this.player.Character)))
            {
                return;
            }

            if(!aimPivot.EnsureNotNull(this, nameof(aimPivot)))
            {
                Debug.LogError("aimPivot is null");
                return;
            }

            CanHearPlayer = this.player.Character.CanHear(aimPivot.position);

            var hit = this.player.Character.CanSee(aimPivot.position, EnemySettings.MaxVisualRange, lineOfSightMask);

            if(hit.HasValue && hit.Value.transform != null && hit.Value.transform.CompareTag(Tags.Player))
            {
                CanSeePlayer = true;
                HasSeenPlayer = true;
                LastKnownPlayerPosition = hit.Value.point;
            }
            else
            {
                CanSeePlayer = false;
            }

            if (!HasSeenDeadEnemies)
            {
                var hits = Physics2D.OverlapCircleAll(aimPivot.position, 5f, lineOfSightMask);
                foreach (var h in hits)
                {
                    if (h.CompareTag(Tags.Enemy) && h.TryGetComponent<IAi>(out var ai) && !ai.CurrentState.IsAlive)
                    {
                        Debug.Log("has seen dead enemies", this);
                        HasSeenDeadEnemies = true;
                        break;
                    }
                }
            }
        }

        
        public void Reset()
        {
            Character.Reset();
            Character.SetBattleState();
        }

        public void UpdateBehaviour()
        {
            Character.OperatingSystem.UpdateBehaviour();
        }

        public void FixedUpdateBehaviour()
        {
            UpdateDetect();

            if(Game.Instance.Player.Character.IsDead)
            {
                Character.SetMovement(0, 0);
                return;
            }

            CurrentState.UpdateBehaviour(new AiContext()
            {
                name = this.name,
                position = transform.position,
                canHearPlayer = CanHearPlayer || HasBeenAlerted,
                canSeePlayer = CanSeePlayer,
                hasSeenPlayer = HasSeenPlayer,
                hasSeenDeadEnemies = HasSeenDeadEnemies,
                facing = Mathf.RoundToInt(Character.transform.right.x),
                settings = EnemySettings,
                playerInFrontOfUs = PlayerInFrontOfUs,
                lastKnownPlayerPosition = LastKnownPlayerPosition,
                wallInFront = WallInFront,
                dropInFront = DropInFront,
                bullets = character.OperatingSystem.Ammo,
                flying = character.Body ? character.Body.Flying : false,
            });

            Character.Physics();
        }

        public void LateUpdateBehaviour() => Character.Animate();

        public float DistanceToPlayer() => this.player != null ? (this.player.transform.position - transform.position).magnitude : int.MaxValue;

        public void Alert(Vector3 position)
        {
            LastKnownPlayerPosition = position;
            HasBeenAlerted = true;
        }

        private void Hit()
        {
            //Debug.Log("took a hit", this);
        }

        private void Die() => CurrentState = new EnemyDeadState();
    }

    public struct AiContext
    {
        public string name;
        public Vector3 position;
        public bool canSeePlayer;
        public bool canHearPlayer;
        public bool hasSeenPlayer;
        public bool playerInFrontOfUs;
        public bool hasSeenDeadEnemies;
        public int facing;
        public Vector3 lastKnownPlayerPosition;
        public bool wallInFront;
        public bool dropInFront;
        public int bullets;
        public bool flying;
        public EnemySettings settings;
    }

    public interface IAi
    {
        IEnemyState CurrentState { get; set; }
        CharController Character { get; }
        StatusIcon Icon { get; }

        bool WallInFront { get; }

        bool DropInFront { get; }

        void Alert(Vector3 position);
    }
}
