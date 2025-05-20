using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class RandomContents : MonoBehaviour
    {
        [SerializeField] private List<GameObject> randomPrefabs = new List<GameObject>();

        void Awake()
        {
            PickPrefab();
        }

        private void PickPrefab()
        {
            if (randomPrefabs.Count == 0) return;

            var idx = Random.Range(0, randomPrefabs.Count);

            Instantiate(randomPrefabs[idx], transform.position, Quaternion.identity, this.transform);
        }
    }
}
