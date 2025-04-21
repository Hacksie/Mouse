
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private List<SpawnList> spawnList = new List<SpawnList>();
        //[SerializeField] private List<GameObject> backgroundProps;
        //[SerializeField] private List<GameObject> obstacles;
        [SerializeField] private Collider2D edge;


        private void Start()
        {
            //SpawnBackgroundProps();
            //SpawnProps();
        }

        public void SpawnBackgroundProps(System.Random rng, int level)
        {
            if (edge == null)
            {
                Debug.LogWarning("Building collider not set", this);
                return;
            }

            Debug.Log("Spawning props for : " + level);

            var list = spawnList.FirstOrDefault(x => x.minLevel <= level && x.maxLevel >= level);
            Debug.Log(list);

            if (list == null || list.bgProps == null || list.bgProps.Count <= 0)
            {
                Debug.LogWarning("No background props defined:" + (list==null), this);
                return;
            }


            
            var offset = rng.NextFloat(Game.Instance.GameSettings.MinBasePropGap, Game.Instance.GameSettings.MaxBasePropGap);
            var topLeft = new Vector3(edge.bounds.min.x, edge.bounds.max.y, transform.position.z);

            while ((edge.bounds.min.x + offset) < (edge.bounds.max.x - 5))
            {

                Vector3 pos = edge.ClosestPoint(topLeft + (Vector3.right * offset));

                var idx = rng.Next(list.bgProps.Count);
                Instantiate(list.bgProps[idx], transform.position + pos, Quaternion.identity, this.transform);

                offset += rng.NextFloat(Game.Instance.GameSettings.MinBasePropGap, Game.Instance.GameSettings.MaxBasePropGap);
            }
        }

        public void SpawnProps(System.Random rng, int level)
        {
            if (edge == null)
            {
                return;
            }

            var list = spawnList.FirstOrDefault(x => x.minLevel <= level && x.maxLevel >= level);

            if (list == null || list.obstacles == null || list.obstacles.Count <= 0)
            {
                Debug.LogWarning("No obstacless defined", this);
                return;
            }

            var offset = rng.NextFloat(Game.Instance.GameSettings.MinBasePropGap, Game.Instance.GameSettings.MaxBasePropGap);
            var topLeft = new Vector3(edge.bounds.min.x, edge.bounds.max.y, transform.position.z);

            while ((edge.bounds.min.x + offset) < (edge.bounds.max.x - 5))
            {

                Vector3 pos = edge.ClosestPoint(topLeft + (Vector3.right * offset));

                var idx = rng.Next(list.obstacles.Count);
                Instantiate(list.obstacles[idx], transform.position + pos, Quaternion.identity, this.transform);

                offset += rng.NextFloat(Game.Instance.GameSettings.MinBasePropGap, Game.Instance.GameSettings.MaxBasePropGap);
            }
        }
    }
}
