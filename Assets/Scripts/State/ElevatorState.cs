using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class ElevatorState : IState
    {
        private readonly IPresenter elevatorMenu;
        
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public ElevatorState(IPresenter elevatorMenu)
        {
            this.elevatorMenu = elevatorMenu;     
            
        }

        public void Begin()
        {
            this.elevatorMenu.Repaint();
            this.elevatorMenu.Show();
        }

        public void End()
        {
            this.elevatorMenu.Hide();
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