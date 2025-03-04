using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace HackedDesign
{
    public interface IEnemyPool
    {
        void Reset();
    }

    public class EnemyPool : MonoBehaviour, IEnemyPool
    {
        [SerializeField] private List<Enemy> prefabList;

        private List<Enemy> pool = new List<Enemy>(1000);

        public void Reset()
        {
            for(int i = 0;i < this.transform.childCount;i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        public Enemy Spawn(EnemyType type, Vector3 position, Quaternion rotation)
        {
            var activePool = Active();


             var enemy = activePool.FirstOrDefault(e => e.type == type);


            if(enemy == null)
            {
                var prefab = prefabList.FirstOrDefault(e => e.type == type);
                if (prefab == null)
                {
                    Debug.LogError("Could not prefab of type " + type);
                }



                enemy = GameObject.Instantiate(prefab, position, rotation, this.transform);

                pool.Add(enemy);


            }

            enemy.gameObject.SetActive(true);


            return enemy;
        }

        public Enemy Spawn(Vector3 position, Quaternion rotation)
        {
            var activePool = Active();


            var enemy = activePool.ElementAtOrDefault(Random.Range(0, activePool.Count));

            if (enemy == null)
            {
                var prefab = prefabList[Random.Range(0, prefabList.Count)];
                if (prefab == null)
                {
                    Debug.LogError("Could not prefab");
                }

                enemy = GameObject.Instantiate(prefab, position, rotation, this.transform);

                pool.Add(enemy);

            }

            enemy.gameObject.SetActive(true);


            return enemy;
        }

        public List<Enemy> Active()
        {
            return pool.Where(e => e.gameObject.activeInHierarchy).ToList();
        }

        public void UpdateAllBehaviour()
        {
            foreach (var enemy in pool.Where(e => e.gameObject.activeInHierarchy))
            {
                enemy.UpdateBehaviour();
            }
        }
    }
}
