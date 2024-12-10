using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController player;
        private LevelGenerator level;
        private UI.AbstractPresenter actionBar;
        
        public bool PlayerActionAllowed => true;
        public bool Battle => true;


        public PlayingState(PlayerController player, LevelGenerator level, AbstractPresenter actionBar)
        {
            this.player = player;
            this.level = level;
            this.actionBar = actionBar;
            
        }

        public void Begin()
        {
            this.level.Reset();
            this.level.Generate(1);
            var spawn = GameObject.FindGameObjectWithTag("Respawn");

            if (spawn != null)
            {
                this.player.transform.position = spawn.transform.position;
            }

            this.player.Go();
            this.actionBar.Show();
        }

        public void End()
        {

        }

        public void Update()
        {
            this.player.UpdateBehavior();
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
            //GameManager.Instance.SetStartMenu();
        }

        public void Select()
        {

        }
    }
}