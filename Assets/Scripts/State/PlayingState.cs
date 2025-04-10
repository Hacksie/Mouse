using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private readonly PlayerController player;
        private readonly Level level;
        private readonly IPresenter actionBar;
        private readonly IPresenter traceBar;
        private readonly EnemyPool enemyPool;

        public bool PlayerActionAllowed => true;
        public bool Battle => true;


        public PlayingState(PlayerController player, Level level, EnemyPool enemyPool, IPresenter actionBar, IPresenter traceBar)
        {
            this.player = player;
            this.level = level;
            this.actionBar = actionBar;
            this.enemyPool = enemyPool;
            this.traceBar = traceBar;
            
        }

        public void Begin()
        {
            Game.Instance.LevelTimer.Timer.Start();
            Game.Instance.LevelTimer.Timer.OnTimerStop += TimeOut;
            //this.level.Reset();
            //this.level.Generate(1);
            //var spawn = GameObject.FindGameObjectWithTag("Respawn");

            //if (spawn != null)
            //{
            //    this.player.transform.position = spawn.transform.position;
            //}

            this.player.Battle();
            this.actionBar.Show();
            this.traceBar.Show();
        }

        public void End()
        {
            this.player.Stop();
            this.traceBar.Hide();
            this.actionBar.Hide();
        }

        private void TimeOut()
        {
            Debug.Log("Timeout");
        }

        public void Update()
        {
            Game.Instance.LevelTimer.Timer.Tick(Time.deltaTime);
            this.player.UpdateBehavior();
            this.enemyPool.UpdateAllBehaviour();
            this.traceBar.Repaint();
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
            Game.Instance.SetPaused();
        }

        public void Select()
        {
            Game.Instance.SetOS();
        }
    }
}