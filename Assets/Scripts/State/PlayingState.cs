using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        public bool Playing => true;
        private PlayerController player;

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

        public void FixedUpdate()
        {
            
        }

        public void Select()
        {
            
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            /*
            if(this.player.ShouldPlayerBeDead())
            {
                Game.Instance.SetDead();
                return;
            }*/
            this.player.UpdateBehaviour();
        }

    }
}