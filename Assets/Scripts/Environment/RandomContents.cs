
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class RandomContents : MonoBehaviour
    {
        [SerializeField] private float changeToPickSomething = 1.0f;
        //[SerializeField] private List<GameObject> randomPrefabs = new();
        [SerializeField] private List<WeightedPrefab> weightedPrefabs = new();
        

        void Awake() => PickPrefab();

        private void PickPrefab()
        {
            //if (randomPrefabs.Count == 0)
            //{
            //    return;
            //}

            if (Random.value < changeToPickSomething)
            {
                var prefab = GetRandomPrefab();
                var gameObj = Instantiate(prefab, transform.position, Quaternion.identity, this.transform);
                gameObj.ClearCloneSuffix();
            }
        }

        public GameObject GetRandomPrefab()
        {
            if (weightedPrefabs == null || weightedPrefabs.Count == 0)
            {
                Debug.LogError("Invalid weighted prefabs list");
                return null;
            }

            float totalWeight = 0f;
            foreach (var item in weightedPrefabs)
            {
                totalWeight += item.weight;
            }

            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            float runningTotal = 0f;
            foreach (var item in weightedPrefabs)
            {
                runningTotal += item.weight;
                if (randomValue <= runningTotal)
                {
                    return item.prefab;
                }
            }

            return weightedPrefabs[weightedPrefabs.Count - 1].prefab;

        }
    }

    [System.Serializable]
    public class WeightedPrefab
    {
        public float weight = 1;
        public GameObject prefab;
    }
}
