using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace HackedDesign
{
    [RequireComponent(typeof(CharController))]
    public class EnemyController : MonoBehaviour, AI
    {
        [SerializeField] public UnityAction hitBehaviour;
        [SerializeField] public UnityAction deathBehaviour;
        [SerializeField] private CharController character;
        [SerializeField] public LayerMask lineOfSightMask;
        [SerializeField] private EnemySettings enemySettings;
        [SerializeField] private StatusIcon characterStatusIcon;
        [SerializeField] private Transform aimPivot = null;

        private PlayerController player = null;

        private IEnemyState currentState;

        public IEnemyState CurrentState
        {
            get { return currentState; }
            set
            {
                currentState?.End();
                currentState = value;
                currentState?.Begin();
            }
        }

        public StatusIcon Icon { get => characterStatusIcon; }

        public int Facing { get; set; } = 1;

        public bool HasSeenPlayer
        {
            get; private set;
        }

        public Vector3 LastKnownPlayerPosition { get; private set; }


        public bool CanSeePlayer 
        {
            get; private set;
        }

        public bool CanHearPlayer
        {
            get; private set;
            
        }

        public bool PlayerInFrontOfUs
        {
            get
            {
                var facing = this.player.transform.position.x <= this.transform.position.x ? -1 : 1;

                return Mathf.Sign(facing) == Mathf.Sign(transform.right.x);
            }
        }

        public EnemySettings EnemySettings { get => enemySettings; private set => enemySettings = value; }
        public CharController Character { get => character; private set => character = value; }

        void Awake()
        {
            this.AutoBind(ref character);
            this.characterStatusIcon = GetComponentInChildren<StatusIcon>();
            Character.dieActions.AddListener(Die);
            Character.hitActions.AddListener(Hit);
            CurrentState = new EnemyIdleState(this);
        }

        private void Start()
        {
            this.player = Game.Instance.Player;
            Reset();
        }

        private void UpdatePlayerDetect()
        {
            var vectorToPlayer = this.player.Character.Head.transform.position - aimPivot.position;

            CanHearPlayer = this.player.Character.CanHear(aimPivot.position);

            var hit = this.player.Character.CanSee(aimPivot.position, EnemySettings.MaxVisualRange, lineOfSightMask);

            if (hit != null && hit.Value && hit.Value.transform.CompareTag(Tags.Player))
            {
                HasSeenPlayer = true;
                LastKnownPlayerPosition = hit.Value.point;
                CanSeePlayer = true;
            }
            else
            {
                CanSeePlayer = false;
            }
        }


        public void Reset()
        {
            Character.Reset();
            Character.SetBattleState();
        }

        public void UpdateBehaviour()
        {
        }

        public void FixedUpdateBehaviour()
        {
            UpdatePlayerDetect();

            CurrentState.UpdateBehaviour(new AIContext()
            {
                canHearPlayer = CanHearPlayer,
                canSeePlayer = CanSeePlayer,
                hasSeenPlayer = HasSeenPlayer,
                facing = Facing,
                settings = EnemySettings,
                playerInFrontOfUs = PlayerInFrontOfUs,
                lastKnownPlayerPosition = LastKnownPlayerPosition,
            });

            Character.Physics();
        }

        public void LateUpdateBehaviour()
        {
            Character.Animate();
        }

        public float DistanceToPlayer()
        {
            return (this.player.transform.position - transform.position).magnitude;
        }



        private void Hit()
        {
            //Debug.Log("took a hit", this);
        }

        private void Die()
        {
            CurrentState = new EnemyDeadState();
        }
    }

    public struct AIContext
    {
        public bool canSeePlayer;
        public bool canHearPlayer;
        public bool hasSeenPlayer;
        public bool playerInFrontOfUs;
        public int facing;
        public Vector3 lastKnownPlayerPosition;
        public EnemySettings settings;
    }

    public interface AI
    {
        IEnemyState CurrentState { get; set; }
        CharController Character { get; }
        StatusIcon Icon { get; }
    }
}
