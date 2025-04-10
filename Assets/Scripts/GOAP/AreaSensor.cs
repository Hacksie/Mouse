using System;
using HackedDesign;

using UnityEngine;

namespace HackedDesign
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class AreaSensor : MonoBehaviour, ISensor
    {
        [SerializeField] protected float timerInterval = 1f;
        [SerializeField] protected LayerMask layerMask;

        BoxCollider2D detectionBox;

        public event Action OnTargetChanged = delegate { };

        public Vector3 TargetPosition => target ? target.transform.position : Vector3.zero;
        public bool IsTargetInRange => TargetPosition != Vector3.zero;

        GameObject target;
        Vector3 lastKnownPosition;
        CountdownTimer timer;

        void Awake()
        {
            detectionBox = GetComponent<BoxCollider2D>();
            detectionBox.isTrigger = true;
            //detectionBox.radius = detectionRadius;
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
            //
            if (!other.CompareTag("Player")) return;
            Debug.Log(other.gameObject.tag);
            UpdateTargetPosition(other.gameObject);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            UpdateTargetPosition();
        }

    }
}