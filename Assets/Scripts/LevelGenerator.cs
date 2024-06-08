
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] List<GameObject> prefabs;
        [SerializeField] List<GameObject> leftWallPrefab;
        [SerializeField] List<GameObject> rightWallPrefab;

        [SerializeField] int width = 10;
        [SerializeField] int height = 10;
        [SerializeField] int maxRoomWidth = 3;
        [SerializeField] int maxRoomHeight = 3;
        [SerializeField] int spanWidth = 9;
        [SerializeField] int spanHeight = 9;

        private int[,] map;

        void Start()
        {
            Reset();
            Generate(0);
        
            
        }

        public void Reset()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public void Generate(int level)
        {
            Instantiate(prefabs[0], new Vector3(0 * spanWidth, level * spanHeight, 0), Quaternion.identity, this.transform);
            
        }
    }
}