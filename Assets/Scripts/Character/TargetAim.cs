using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HackedDesign
{
    public class TargetAim : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private float radius = 2f;
        [SerializeField] UnityEngine.UI.Text nametagLabel;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float targetRadius = 20f;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Animator animator;


        private Interactable currentHighlightable;

        public void ShowAimLine(Vector3 direction)
        {
            var hit = Physics2D.Linecast(pivot.position, pivot.position + (direction.normalized * radius), targetMask);

            DrawAimLine(hit, direction);
            UpdateHover(hit);
            animator?.SetFloat("aiming", 1);
        }
        
        public void HideAimLine()
        {
            lineRenderer.positionCount = 0;
            animator?.SetFloat("aiming", 0);
            ClearHighlightable();
        }


        private void DrawAimLine(RaycastHit2D hit, Vector3 direction)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new Vector3[2] { pivot.position, hit ? hit.point : pivot.position + (direction.normalized * 20) });
        }


        private void UpdateHover(RaycastHit2D hit)
        {
            if (currentHighlightable)
            {
                ClearHighlightable();
            }

            if (hit && hit.collider.CompareTag("Enemy"))
            {
                nametagLabel.text = hit.collider.name.ToString();
                if(hit.collider.gameObject.TryGetComponent<Interactable>(out var highlight))
                {
                    currentHighlightable = highlight;
                    currentHighlightable.Target(true);
                }
            }
        }

        private void ClearHighlightable()
        {
            if (currentHighlightable)
            {
                currentHighlightable.Target(false);
                currentHighlightable = null;
                nametagLabel.text = "";
            }
        }
    }
}
