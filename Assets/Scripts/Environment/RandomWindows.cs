using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class RandomWindows: MonoBehaviour
    {
        [SerializeField] List<SpriteRenderer> windowSpritePrefabs;

        [SerializeField] Bounds bounds;

        private void Start()
        {
            for (float x = bounds.min.x; x <= bounds.max.x; x+=5.5f)
            {
                for (float y = bounds.min.y; y <= bounds.max.y; y+=4)
                {
                    Instantiate(windowSpritePrefabs[Random.Range(0, windowSpritePrefabs.Count)], new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0f), Quaternion.identity, this.transform);

                    //if (Random.value < 0.4f)
                    //{
                    //    Instantiate(windowSpritePrefabs[Random.Range(0, windowSpritePrefabs.Count)], new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0f), Quaternion.identity, this.transform);
                    //}
                }
            }
        }
    }
}
