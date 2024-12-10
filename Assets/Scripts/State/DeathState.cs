using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class DeathState : IState
    {
        private AbstractPresenter deathMenu;
        
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public DeathState(AbstractPresenter deathMenu)
        {
            this.deathMenu = deathMenu;     
            
        }

        public void Begin()
        {
            this.deathMenu.Show();
        }

        public void End()
        {
            this.deathMenu.Hide();
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