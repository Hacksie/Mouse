using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class DeathState : IState
    {
        private readonly IPresenter deathMenu;
        
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public DeathState(IPresenter deathMenu)
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