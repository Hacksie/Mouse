using UnityEngine;

namespace HackedDesign
{
    public class DialogState : IState
    {
        private PlayerController player;
        
        public bool PlayerActionAllowed => true;
        public bool Battle => true;


        public DialogState(PlayerController player)
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
    }
}