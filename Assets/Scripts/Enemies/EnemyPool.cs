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
        private const string InstantiateCloneSuffix = "(Clone)";
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

        public EnemyController Spawn(EnemySpawn spawn)
        {
            var enemy = FindInactiveEnemyForSpawn(spawn);

            if(enemy == null)
            {
                var prefab = FindPrefabForSpawn(spawn);

                if (prefab == null)
                {
                    Debug.LogError("Could not find a prefab to spawn", this);
                    return null;
                }

                enemy = Instantiate(prefab, spawn.transform.position, Quaternion.identity, this.transform);
                enemy.name = enemy.name.Replace(InstantiateCloneSuffix, "");

                pool.Add(enemy);
            }

            enemy.gameObject.SetActive(true);

            return enemy;
        }

        public EnemyController FindInactiveEnemyForSpawn(EnemySpawn spawn) => pool.Where(e => !e.gameObject.activeInHierarchy && spawn.CanSpawn(e.EnemyType)).OrderBy(_ => Random.value).FirstOrDefault();
        private EnemyController FindPrefabForSpawn(EnemySpawn spawn) => prefabList.Where(e => spawn.CanSpawn(e.EnemyType)).OrderBy(_ => Random.value).FirstOrDefault();
        
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
