using UnityEngine;

namespace HackedDesign
{
    public class Parallax : MonoBehaviour
    {
        private Vector2 bounds, startPosition;

        [SerializeField] private Camera mainCamera = null;
        [SerializeField] private float parallaxEffect = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                Debug.LogError("cam not set", this);
            }
            startPosition = transform.position;
            //bounds = GetComponent<SpriteRenderer>().bounds.size;
        }

        void Update()
        {
            Vector2 temp = mainCamera.transform.position * (1 - parallaxEffect);
            Vector2 dist = new Vector2(mainCamera.transform.position.x * parallaxEffect, 0);

            transform.position = startPosition + dist;

            /*
            if (temp.x > (startPosition.x + bounds.x))
            {
                startPosition.x += bounds.x;
            }
            if (temp.x < (startPosition.x - bounds.x))
            {
                startPosition.x -= bounds.x;
            }
            if (temp.y > (startPosition.y + bounds.y))
            {
                startPosition.y += bounds.y;
            }
            if (temp.y < (startPosition.y - bounds.y))
            {
                startPosition.y -= bounds.y;
            }*/
        }
    }
}