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
        [SerializeField] private new Collider2D collider2D;
        [SerializeField] private Transform pivot;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float lineInnerRadius = 1f;
        [SerializeField] private float targetRadius = 20f;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private TargetPresenter targetPresenter;

        private Interactable currentTarget;

        private readonly List<Interactable> currentInteractables = new();

        private void Awake()
        {
            this.AutoBind(ref collider2D);
        }

        public void TriggerInteract()
        {
            if(currentTarget)
            {
                currentTarget.TriggerInteract();
            }
            /*
            if (currentInteractables.Count > 0)
            {
                currentInteractables[0].TriggerInteract();
            }*/
        }

        public void ClearAllInteractors()
        {
            currentInteractables.Clear();
            currentTarget = null;
        }

        public void UpdateInteractors()
        {
            /*
            foreach (var interactable in currentInteractables)
            {
                interactable.Interact(false);
                //targetPresenter.Repaint();
            }
            currentInteractables.Clear();

            List<Collider2D> contacts = new List<Collider2D>();

            collider2D.GetContacts(contacts);

            foreach (var contact in contacts)
            {
                if (contact.CompareTag("Player"))
                {
                    continue;
                }

                if (contact.TryGetComponent<Interactable>(out var interactable))
                {
                    interactable.Interact(true);
                    currentInteractables.Add(interactable);
                    
                }
            }

            RepaintUI();*/
        }

        public void ShowTarget(Vector3 direction, bool aiming, bool hasPistol)
        {
            var hit = Physics2D.Linecast(pivot.position, pivot.position + (direction.normalized * targetRadius), targetMask);
            UpdateHover(hit);

            if (aiming)
            {
                DrawAimLine(hit, direction);
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
            if (animator)
            {
                animator.SetFloat("aiming", flag? 1 : 0);
            }
        }

        private void RepaintUI()
        {
            if(currentInteractables.Count > 0)
            {
                targetPresenter.Repaint(currentInteractables[0]);
            }
            else if(currentTarget)
            {
                targetPresenter.Repaint(currentTarget);
            }
            else
            {
                targetPresenter.Repaint();
            }
        }


        private void DrawAimLine(RaycastHit2D hit, Vector3 direction)
        {
            lineRenderer.positionCount = 2;

            var startPosition = pivot.position + (direction.normalized * lineInnerRadius);

            lineRenderer.SetPositions(new Vector3[2] { startPosition, hit ? hit.point : startPosition + (direction.normalized * targetRadius) });
        }


        private void UpdateHover(RaycastHit2D hit)
        {

            if (hit && hit.collider.gameObject.TryGetComponent<Interactable>(out var highlight))
            {
                ClearHighlightable();
                //currentInteractables.Add(highlight);
                currentTarget = highlight;
                highlight.Target(true);
                RepaintUI();
                //TargetAction?.Invoke(highlight);

            }
            else
            {
                if (currentTarget)
                {
                    ClearHighlightable();
                    RepaintUI();
                    //UntargetAction?.Invoke();
                }
            }
        }

        private void ClearHighlightable()
        {
            if (currentTarget)
            {
                currentTarget.Target(false);
                currentTarget = null;
            }
        }
    }
}
