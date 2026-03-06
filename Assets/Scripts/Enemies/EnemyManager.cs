using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace HackedDesign
{
    public interface IEnemyManager
    {
        void Reset();
        EnemyController Spawn(EnemySpawn spawn);
        void UpdateAllBehaviour();
        void UpdateAllFixedBehaviour();
        void UpdateAllLateBehaviour();
    }

    public class EnemyManager : AutoSingleton<EnemyManager>, IEnemyManager
    {
        [SerializeField] private List<EnemyController> prefabList;

        private readonly List<EnemyController> pool = new(100);

        public void Reset()
        {
            for(int i = 0;i < this.transform.childCount;i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
                Destroy(this.transform.GetChild(i).gameObject);
            }

            pool.Clear();
        }

        public EnemyController Spawn(EnemySpawn spawn)
        {
            Debug.Log("Spawning enemy at " + spawn.transform.position);

            var enemy = FindInactiveEnemyForSpawn(spawn);

            if(enemy == null)
            {
                enemy = InstantiateNewEnemy(spawn);
                pool.Add(enemy);
            }

            enemy.gameObject.SetActive(true);

            return enemy;
        }

        public EnemyController InstantiateNewEnemy(EnemySpawn spawn)
        {
            var prefab = FindPrefabForSpawn(spawn);

            if (prefab == null)
            {
                Debug.LogError("Could not find a prefab to spawn", this);
                return null;
            }           

            var enemy = Instantiate(prefab, spawn.transform.position + (Vector3)prefab.EnemySettings.SpawnOffset, Quaternion.identity, this.transform);
            enemy.gameObject.ClearCloneSuffix();
            return enemy;
        }

        public EnemyController FindInactiveEnemyForSpawn(EnemySpawn spawn) => 
            pool.Where(e => !e.gameObject.activeInHierarchy && spawn.CanSpawnEnemy(e.EnemyType)).OrderBy(_ => Random.value).FirstOrDefault();
        private EnemyController FindPrefabForSpawn(EnemySpawn spawn) => 
            prefabList.Where(e => spawn.CanSpawnEnemy(e.EnemyType)).OrderBy(_ => Random.value).FirstOrDefault();
        
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
