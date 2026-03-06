using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class MainMenuState : IState
    {
        private readonly IPresenter mainMenu;
        
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public MainMenuState(IPresenter mainMenu)
        {
            this.mainMenu = mainMenu;     
            
        }

        public void Begin() => mainMenu.Show();

        public void End() => mainMenu.Hide();

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