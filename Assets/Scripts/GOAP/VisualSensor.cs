using System;
using HackedDesign;

using UnityEngine;

namespace HackedDesign
{

    [RequireComponent(typeof(CircleCollider2D))]
    public class VisualSensor : MonoBehaviour, ISensor
    {
        [SerializeField] protected float detectionRadius = 5f;
        [SerializeField] protected float timerInterval = 1f;
        [SerializeField] protected LayerMask layerMask;

        CircleCollider2D detectionRange;

        public event Action OnTargetChanged = delegate { };

        public Vector3 TargetPosition => target ? target.transform.position : Vector3.zero;
        public bool IsTargetInRange => TargetPosition != Vector3.zero;

        GameObject target;
        Vector3 lastKnownPosition;
        CountdownTimer timer;

        void Awake()
        {
            detectionRange = GetComponent<CircleCollider2D>();
            detectionRange.isTrigger = true;
            detectionRange.radius = detectionRadius;
        }

        void Start()
        {
            timer = new CountdownTimer(timerInterval);
            timer.OnTimerStop += () =>
            {
                UpdateTargetPosition(target.OrNull());
                timer.Start();
            };
            timer.Start();
        }

        void Update()
        {
            timer.Tick(Time.deltaTime);
        }

        void UpdateTargetPosition(GameObject target = null)
        {
            if (target != null)
            {
                Debug.Log("Update target position", this);
            }
            this.target = target;
            if (IsTargetInRange && CanSeeTarget() && (lastKnownPosition != TargetPosition || lastKnownPosition != Vector3.zero))
            {
                lastKnownPosition = TargetPosition;
                OnTargetChanged.Invoke();
            }
        }

        private bool CanSeeTarget()
        {
            if(this.target != null)
            {
                return Physics2D.Linecast(this.transform.position, this.target.transform.position, layerMask);
            }

            return false;
        }
       

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.tag);
            if (!other.CompareTag("Player")) return;
            UpdateTargetPosition(other.gameObject);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            UpdateTargetPosition();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = IsTargetInRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}