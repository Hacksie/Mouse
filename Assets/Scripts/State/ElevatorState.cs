using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class ElevatorState : IState
    {
        private readonly IGame game;
        private readonly ElevatorPresenter elevatorMenu;
        
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public ElevatorState(IGame game, ElevatorPresenter elevatorMenu)
        {
            this.game = game;
            this.elevatorMenu = elevatorMenu;
            this.elevatorMenu.done.AddListener(Done);
        }

        public void Begin()
        {
            elevatorMenu.Repaint();
            elevatorMenu.Show();
        }

        public void Done() => game.SetStatePlaying();

        public void End() => elevatorMenu.Hide();

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