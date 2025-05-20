using HackedDesign.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HackedDesign
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask laserMask;
        [SerializeField] private float lineInnerRadius = 1f;
        [SerializeField] private float targetRadius = 20f;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Animator animator;

        public UnityAction<Interactable> targetChangedAction;

        private Interactable currentTarget;
        private ICharacterExecute charExecute;


        private void Awake()
        {
            charExecute = GetComponent<ICharacterExecute>();
        }

        public bool IsTargetInRange { get => (pivot.position - currentTarget.transform.position).sqrMagnitude < (Game.Instance.GameSettings.InteractDistance * Game.Instance.GameSettings.InteractDistance); }

        public void TriggerInteract()
        {
            if (currentTarget && IsTargetInRange)
            {
                charExecute.ExecuteCommand(new InteractCommand());
                currentTarget.TriggerInteract();
            }
        }


        public void ShowTarget(Vector3 direction, bool aiming, bool hasPistol)
        {
            var hit = Physics2D.Linecast(pivot.position, pivot.position + (direction.normalized * targetRadius), targetMask);
            UpdateHover(hit);

            if (aiming)
            {
                DrawAimLine(direction);
                AnimateAiming(hasPistol);
            }
            else
            {
                lineRenderer.positionCount = 0;
                AnimateAiming(false);
            }
        }

        private void AnimateAiming(bool flag)
        {
            charExecute.ExecuteCommand(new AimCommand(flag));
        }

        private void DrawAimLine(Vector3 direction)
        {
            lineRenderer.positionCount = 2;

            var startPosition = pivot.position + (direction.normalized * lineInnerRadius);

            var hit = Physics2D.Linecast(pivot.position, startPosition + (direction.normalized * targetRadius), laserMask);

            lineRenderer.SetPositions(new Vector3[2] { startPosition, hit ? hit.point : startPosition + (direction.normalized * targetRadius) });
        }


        private void UpdateHover(RaycastHit2D hit)
        {
            if (hit && hit.collider.gameObject.TryGetComponent<Interactable>(out var newTarget))
            {
                ClearHighlightable();
                currentTarget = newTarget;
                currentTarget.Target(true);
                targetChangedAction.Invoke(currentTarget);
            }
            else
            {
                ClearHighlightable();
            }
        }

        private void ClearHighlightable()
        {
            if (currentTarget)
            {
                currentTarget.Target(false);
                currentTarget = null;
                targetChangedAction.Invoke(null);
            }
        }
    }
}
