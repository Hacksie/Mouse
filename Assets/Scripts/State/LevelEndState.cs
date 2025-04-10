using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedDesign
{
    public class LevelEndState : IState
    {
        private readonly PlayerController player;

        public bool PlayerActionAllowed => false;

        public bool Battle => false;

        public LevelEndState(PlayerController player)
        {
            this.player = player;
        }

        public void Begin()
        {
            this.player.Stop();
        }

        public void End()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void LateUpdate()
        {
            
        }

        public void Menu()
        {
            
        }

        public void Select()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
