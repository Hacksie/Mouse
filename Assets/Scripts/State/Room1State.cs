using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class Room1State : IState
    {
        private PlayerController player;
        private Level level;
        private readonly IPresenter actionBar;

        public bool PlayerActionAllowed => true;
        public bool Battle => false;


        public Room1State(PlayerController player, Level level, IPresenter actionBar)
        {
            this.player = player;
            this.level = level;
            this.actionBar = actionBar;
        }

        public void Begin()
        {
            this.level.Reset();
            this.level.Room1();
            var spawn = GameObject.FindGameObjectWithTag("Respawn");

            if (spawn != null)
            {
                this.player.transform.position = spawn.transform.position;
            }

            //this.player.
            this.player.Idle();
            //this.player.Sit();
            this.actionBar.Show();
        }

        public void End()
        {
            
        }

        public void Update()
        {
            this.player.UpdateBehavior();
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