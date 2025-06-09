
using DunGen;
using log4net.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class Level : MonoBehaviour
    {
        [Header("Environment")]
        [SerializeField] Transform sky;
        [SerializeField] Transform smog;
        [SerializeField] Transform rain;
        [SerializeField] List<RuntimeDungeon> backgroundLevels;
        [Header("Dungeon")]
        [SerializeField] RuntimeDungeon runtimeDungeon;
        [Header("Prefabs")]
        [SerializeField] List<Transform> namedRooms;
        [SerializeField] List<GameObject> namedRoomPrefabs;
        [SerializeField] GameObject levelStartPrefab;

        private GameObject namedRoom;
        private GameObject levelStart;

        void Start()
        {
            Reset();
            runtimeDungeon.Generator.ShouldRandomizeSeed = false;
            backgroundLevels.ForEach(bg => bg.Generator.ShouldRandomizeSeed = false);
        }

        public void Reset()
        {
            if (this.namedRoom)
            {
                this.namedRoom.SetActive(false);
                Destroy(this.namedRoom);
            }
            
            backgroundLevels.ForEach(x => x.Generator.Clear(true));

            runtimeDungeon.Generator.Clear(true);
        }

        public void RainOff() => this.rain.gameObject.SetActive(false);

        public void RainOn() => this.rain.gameObject.SetActive(true);
        public void ShowNamedRoom(string name, bool cityBg, bool rain, PlayerController player)
        {
            Reset();
            Random.seed = 1;

            if (cityBg)
            {
                backgroundLevels.ForEach(bg =>
                {
                    try
                    {
                        bg.Generator.Seed = 50;
                        bg.Generate();
                    }
                    catch
                    {
                        Debug.LogWarning("Failed to generate bg level");
                    }
                }
                );
            }

            var room = namedRoomPrefabs.First(x => x.name == name);
            this.namedRoom = Instantiate(room, this.transform);
            this.namedRoom.SetActive(true);
            var spawn = this.namedRoom.transform.Find("Spawn");
            if (spawn)
            {
                player.Teleport(this.namedRoom.transform.Find("Spawn").transform.position);
            }

            this.rain.gameObject.SetActive(rain);
        }

        // FIXME: This should be separated into seed and level
        public void Generate(int level)
        {
            Random.seed = level;
            backgroundLevels.ForEach(bg => {
                try
                {
                    bg.Generator.Seed = level;
                    bg.Generate();
                }
                catch 
                {
                    Debug.LogWarning("Failed to generate bg level");
                }
            });

            runtimeDungeon.Generator.Root = runtimeDungeon.gameObject;
            runtimeDungeon.Generator.Seed = level;
            runtimeDungeon.Generator.Generate();

            this.levelStart = Instantiate(levelStartPrefab , Vector3.zero, Quaternion.identity, this.transform);

            ElevatorManager.Instance.Refresh();
            SpawnEnemies(6);

            //Debug.Log("Seed:" + level);

            //var random = new System.Random(level);

            //var spawners = FindObjectsByType<LevelSpawner>(FindObjectsSortMode.InstanceID);
            //foreach (var spawner in spawners)
            //{
            //    spawner.SpawnBackgroundProps(random, level);
            //    spawner.SpawnProps(random, level);
            //}
        }

        public List<EnemySpawn> GetSpawnLocationsOnLevel() => FindObjectsByType<EnemySpawn>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).OrderBy(_ => Random.value).ToList();
        public List<EnemySpawn> GetSpawnLocationsOnLevel(EnemyType type) => FindObjectsByType<EnemySpawn>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Where(x => x.CanSpawn(type)).OrderBy(_ => Random.value).ToList();

        public void SpawnEnemies(int count)
        {
            EnemyPool.Instance.Reset();
            var spawns = GetSpawnLocationsOnLevel();

            Debug.Log(Mathf.Min(count, spawns.Count), this);

            for (int i = 0; i < Mathf.Min(count, spawns.Count); i++)
            {
                Debug.Log("Spawn " + i.ToString(), this);
                EnemyPool.Instance.Spawn(spawns[i]);
            }
        }
    }
}