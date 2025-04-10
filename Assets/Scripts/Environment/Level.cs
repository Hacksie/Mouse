
using DunGen;
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
        //[SerializeField] Transform intermission;
        //[SerializeField] Transform bedroom1;
        //[SerializeField] Transform bedroom2;
        //[SerializeField] Transform roof1;
        //[SerializeField] Transform roof2;
        [SerializeField] List<Transform> namedRooms;
        [SerializeField] List<GameObject> namedRoomPrefabs;

        private GameObject namedRoom;

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
            if (this.namedRoom)
            {
                this.namedRoom.SetActive(false);
                Destroy(this.namedRoom);
            }
            
            backgroundLevels.ForEach(x => x.Generator.Clear(true));

            runtimeDungeon.Generator.Clear(true);
            //namedRooms.ForEach(x => x.gameObject.SetActive(false));
            //intermission.gameObject.SetActive(false);
            //bedroom1.gameObject.SetActive(false);  
            //bedroom2.gameObject.SetActive(false);
            //roof1.gameObject.SetActive(false);
        }

        public void RainOff()
        {
            this.rain.gameObject.SetActive(false);
        }

        public void RainOn()
        {
            this.rain.gameObject.SetActive(true);
        }

        //public void Intermission(PlayerController player)
        //{
        //    Reset();
        //    //bedroom1.gameObject.SetActive(true);
        //    intermission.gameObject.SetActive(true);
        //    player.Character.MovePosition(intermission.Find("Spawn").transform.position);
        //}

        public void ShowNamedRoom(string name, bool cityBg, bool rain, PlayerController player)
        {
            Reset();


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
            this.namedRoom = Object.Instantiate(room, this.transform);
            this.namedRoom.SetActive(true);
            var spawn = this.namedRoom.transform.Find("Spawn");
            if (spawn)
            {
                player.Character.MovePosition(this.namedRoom.transform.Find("Spawn").transform.position);
            }

            this.rain.gameObject.SetActive(rain);
        }

        
        //public void Roof1(PlayerController player)
        //{
        //    Reset();
        //    roof1.gameObject.SetActive(true);

        //    backgroundLevels.ForEach(bg => {
        //        try
        //        {
        //            bg.Generator.Seed = 50;
        //            bg.Generate();
        //        }
        //        catch
        //        {
        //            Debug.LogWarning("Failed to generate bg level");
        //        }
        //    }
        //    );

        //    player.Character.MovePosition(roof1.Find("Spawn").transform.position);
        //    //intermission.gameObject.SetActive(true);
        //}

        //public void Room1(PlayerController player)
        //{
        //    Reset();
        //    bedroom1.gameObject.SetActive(true);

        //    player.Character.MovePosition(bedroom1.Find("Spawn").transform.position);
            
        //    //intermission.gameObject.SetActive(true);
        //}

        //public void Room2(PlayerController player)
        //{
        //    Reset();
        //    bedroom2.gameObject.SetActive(true);

        //    player.Character.MovePosition(bedroom2.Find("Spawn").transform.position);
        //    //intermission.gameObject.SetActive(true);
        //}

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