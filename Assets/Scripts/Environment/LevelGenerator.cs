
using DunGen;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] Transform sky;
        [SerializeField] Transform smog;
        [SerializeField] Transform rain;
        [SerializeField] RuntimeDungeon runtimeDungeon;
        [SerializeField] List<RuntimeDungeon> backgroundLevels;
        [SerializeField] Transform intermission;

        void Start()
        {
            Reset();
        }

        public void Reset()
        {
            backgroundLevels.ForEach(x => x.Generator.Clear(true));

            runtimeDungeon.Generator.Clear(true);
            intermission.gameObject.SetActive(false);
        }

        public void Intermission()
        {
            Reset();
            intermission.gameObject.SetActive(true);
        }

        public void Generate(int level)
        {
            
            backgroundLevels.ForEach(x => {
                try
                {
                    x.Generate();
                }
                catch 
                {
                    Debug.LogWarning("Failed to generate bg level");
                }
                }
            );
            runtimeDungeon.Generate();
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