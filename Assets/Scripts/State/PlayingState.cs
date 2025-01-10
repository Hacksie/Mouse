using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController player;
        private LevelGenerator level;
        private UI.AbstractPresenter actionBar;
        private EnemyPool enemyPool;

        public bool PlayerActionAllowed => true;
        public bool Battle => true;


        public PlayingState(PlayerController player, LevelGenerator level, EnemyPool enemyPool, AbstractPresenter actionBar)
        {
            this.player = player;
            this.level = level;
            this.actionBar = actionBar;
            this.enemyPool = enemyPool; ;
            
        }

        public void Begin()
        {
            //this.level.Reset();
            //this.level.Generate(1);
            //var spawn = GameObject.FindGameObjectWithTag("Respawn");

            //if (spawn != null)
            //{
            //    this.player.transform.position = spawn.transform.position;
            //}

            this.player.Battle();
            this.actionBar.Show();
        }

        public void End()
        {
            this.player.Stop();
        }

        public void Update()
        {
            this.player.UpdateBehavior();
            this.enemyPool.UpdateAllBehaviour();
            //this.actionBarPresenter.Repaint();
  
        }

        public void FixedUpdate()
        {
            this.player.FixedUpdateBehaviour();
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            
        }

        public void Menu()
        {
            Game.Instance.SetMainMenu();
        }

        public void Select()
        {

        }
    }
}