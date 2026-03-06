using Unity.Cinemachine;
using UnityEngine;

namespace HackedDesign
{
    public class Parallax : MonoBehaviour
    {
        private Vector2 startPosition;

        [SerializeField] private Camera mainCamera = null;
        [SerializeField] private float parallaxEffect = 0.0f;

        CinemachinePixelPerfect pixelPerfect;

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                Debug.LogWarning("Main camera not set", this);
            }
            startPosition = transform.position;

            pixelPerfect = mainCamera.GetComponent<CinemachinePixelPerfect>();
        }

        void FixedUpdate()
        {
            Vector2 dist = new Vector2(mainCamera.transform.position.x * parallaxEffect, mainCamera.transform.position.y * parallaxEffect);

            transform.position = startPosition + dist;
        }
    }
}