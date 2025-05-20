using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace HackedDesign
{
    public class EnemyPool : AutoSingleton<EnemyPool>
    {
        [SerializeField] private List<EnemyController> prefabList;

        private readonly List<EnemyController> pool = new(1000);

        public void Reset()
        {
            for(int i = 0;i < this.transform.childCount;i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        public List<EnemySpawn> GetSpawnLocationsOnLevel() => FindObjectsByType<EnemySpawn>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();

        public EnemyController Spawn(EnemySpawn spawn)
        {
            var activePool = Inactive();


             var enemy = activePool.FirstOrDefault();


            if(enemy == null)
            {
                var prefab = prefabList.FirstOrDefault();
                if (prefab == null)
                {
                    Debug.LogError("Could not prefab of type ");
                }

                enemy = Instantiate(prefab,  spawn.transform.position, Quaternion.identity, this.transform);

                pool.Add(enemy);
            }

            enemy.gameObject.SetActive(true);


            return enemy;
        }

        public EnemyController Spawn(Vector3 position, Quaternion rotation)
        {
            var activePool = Inactive();


            var enemy = activePool.ElementAtOrDefault(Random.Range(0, activePool.Count));

            if (enemy == null)
            {
                var prefab = prefabList[Random.Range(0, prefabList.Count)];
                if (prefab == null)
                {
                    Debug.LogError("Could not prefab");
                }

                enemy = Instantiate(prefab, position, rotation, this.transform);

                pool.Add(enemy);

            }

            enemy.gameObject.SetActive(true);
            enemy.Reset();


            return enemy;
        }

        public List<EnemyController> Inactive()
        {
            return pool.Where(e => !e.gameObject.activeInHierarchy).ToList();
        }

        public void UpdateAllBehaviour()
        {
            foreach (var enemy in pool.Where(e => e.gameObject.activeInHierarchy))
            {
                enemy.UpdateBehaviour();
            }
        }

        public void UpdateAllFixedBehaviour()
        {
            foreach (var enemy in pool.Where(e => e.gameObject.activeInHierarchy))
            {
                enemy.FixedUpdateBehaviour();
            }
        }

        public void UpdateAllLateBehaviour()
        {
            foreach (var enemy in pool.Where(e => e.gameObject.activeInHierarchy))
            {
                enemy.LateUpdateBehaviour();
            }
        }
    }
}
