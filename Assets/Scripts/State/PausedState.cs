using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class PausedState : IState
    {
        private readonly IPresenter pauseMenu;
        
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public PausedState(IPresenter pauseMenu)
        {
            this.pauseMenu = pauseMenu;     
        }

        public void Begin()
        {
            pauseMenu.Show();
        }

        public void End()
        {
            pauseMenu.Hide();
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