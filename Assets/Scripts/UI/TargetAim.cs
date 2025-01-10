using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HackedDesign
{
    public class TargetAim : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private Transform target;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private PlayerInput playerInput = null;
        [SerializeField] private float radius = 2f;
        [SerializeField] UnityEngine.UI.Text nametagLabel;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float targetRadius = 20f;

        private InputAction mousePosAction;
        private InputAction lookPosAction;

        private Interactable currentHighlightable;

        void Awake()
        {
            mousePosAction = playerInput.actions["Mouse Position"];
            lookPosAction = playerInput.actions["Look"];
        }


        private void Update()
        {
            Vector3 direction;
            //Debug.Log(playerInput.currentControlScheme);
            if(playerInput.currentControlScheme == "Gamepad" )
            {
                var lookPos = lookPosAction.ReadValue<Vector2>();
                direction = lookPos.normalized;
            }
            else
            {
                var mousePos = mousePosAction.ReadValue<Vector2>();
                var worldPos = uiCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
                worldPos = new Vector3(worldPos.x, worldPos.y, pivot.position.z);

                direction = (worldPos - pivot.position).normalized;
            }


            target.position = pivot.position + (direction.normalized * radius);
            target.right = direction.normalized;

            UpdateHover(direction);
        }

        private void UpdateHover(Vector3 direction)
        {
            

            if (currentHighlightable)
            {
                currentHighlightable.Show(false);
                nametagLabel.text = "";
            }

            var result = Physics2D.Raycast(pivot.position, direction, targetRadius, targetMask);

            if (result && (result.collider.CompareTag("Interactable") || result.collider.CompareTag("Player") || result.collider.CompareTag("Enemy")))
            {
                nametagLabel.text = result.collider.name.ToString();
                var highlight = result.collider.gameObject.GetComponent<Interactable>();
                if (highlight != null)
                {
                    currentHighlightable = highlight;
                    currentHighlightable.Show(true);
                }
            }
        }
    }
}
