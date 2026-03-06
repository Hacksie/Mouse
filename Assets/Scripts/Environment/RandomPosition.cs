using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HackedDesign
{
    public class RandomPosition: MonoBehaviour
    {
        [SerializeField] private Vector2 rangeX = new Vector2(-5, 5);
        [SerializeField] private Vector2 rangeY = new Vector2(-5, 5);
        void Start()
        {
            float x = UnityEngine.Random.Range(rangeX.x, rangeX.y);
            float y = UnityEngine.Random.Range(rangeY.x, rangeY.y);
            transform.position = transform.position + new Vector3(x, y, transform.position.z);
        }
    }
}
