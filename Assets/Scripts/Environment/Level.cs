
using DunGen;
using System.Collections.Generic;
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
        [SerializeField] Transform intermission;
        [SerializeField] Transform bedroom1;
        [SerializeField] Transform bedroom2;

        void Start()
        {
            Reset();
            runtimeDungeon.Generator.ShouldRandomizeSeed = false;
            backgroundLevels.ForEach(bg =>
            {
                bg.Generator.ShouldRandomizeSeed = false;
            });
        }

        public void Reset()
        {
            backgroundLevels.ForEach(x => x.Generator.Clear(true));

            runtimeDungeon.Generator.Clear(true);
            intermission.gameObject.SetActive(false);
            bedroom1.gameObject.SetActive(false);  
            bedroom2.gameObject.SetActive(false);
        }

        public void Intermission()
        {
            Reset();
            //bedroom1.gameObject.SetActive(true);
            intermission.gameObject.SetActive(true);
        }

        public void Room1()
        {
            Reset();
            bedroom1.gameObject.SetActive(true);
            //intermission.gameObject.SetActive(true);
        }

        public void Room2()
        {
            Reset();
            bedroom2.gameObject.SetActive(true);
            //intermission.gameObject.SetActive(true);
        }

        public void Generate(int level)
        {
            Random.InitState(level);

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
                }
            );

            runtimeDungeon.Generator.Seed = level;
            runtimeDungeon.Generator.Generate();
            
            //runtimeDungeon.Generate();
        }

        public void SpawnEnemies(int count)
        {
            var spawns = GameObject.FindGameObjectsWithTag("EnemySpawn");

            for(int i = 0;i < count;i++)
            {
                if(spawns.Length == 0)
                {
                    break;
                }

                
            }


        }

    }
}