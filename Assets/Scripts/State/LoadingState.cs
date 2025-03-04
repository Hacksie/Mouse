using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class LoadingState : IState
    {
        private readonly PlayerController player;
        private readonly Level level;
        private readonly EnemyPool enemyPool;

        public bool PlayerActionAllowed => true;
        public bool Battle => true;


        public LoadingState(PlayerController player, Level level, EnemyPool enemyPool)
        {
            this.player = player;
            this.level = level;
            this.enemyPool = enemyPool;
        }

        public void Begin()
        {
            this.level.Reset();
            this.level.Generate(2);
            var spawn = GameObject.FindGameObjectWithTag("Respawn");

            if (spawn != null)
            {
                this.player.transform.position = spawn.transform.position;
            }

            //SpawnEnemies();

        }

        private void SpawnEnemies()
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    var posX = Random.Range(100, 500);
            //    var pos = new Vector3(posX, 3, 0);

            //    //this.enemyPool.Spawn(pos, Quaternion.identity);
            //}
        }

        public void End()
        {
        }

        public void Update()
        {
            Game.Instance.ResetLevelTimer();
            //Game.Instance.LevelTimer = new StopwatchTimer();
            Game.Instance.SetPlaying();
        }

        public void FixedUpdate()
        {
        }

        public void LateUpdate()
        {
           
        }

        public void Menu()
        {
            //GameManager.Instance.SetStartMenu();
        }

        public void Select()
        {

        }
    }
}