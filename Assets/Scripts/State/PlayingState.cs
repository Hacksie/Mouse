using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController player;
        
        public bool PlayerActionAllowed => true;
        public bool Battle => true;


        public PlayingState(PlayerController player)
        {
            this.player = player;
            
            
        }

        public void Begin()
        {

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