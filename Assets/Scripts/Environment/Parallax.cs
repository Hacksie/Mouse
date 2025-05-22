using UnityEngine;

namespace HackedDesign
{
    public class Parallax : MonoBehaviour
    {
        private Vector2 startPosition;

        [SerializeField] private Camera mainCamera = null;
        [SerializeField] private float parallaxEffect = 0.0f;

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                Debug.LogWarning("cam not set", this);
            }
            startPosition = transform.position;
        }

        void Update()
        {
            Vector2 dist = new Vector2(mainCamera.transform.position.x * parallaxEffect, mainCamera.transform.position.y * parallaxEffect);

            transform.position = startPosition + dist;
        }
    }
}