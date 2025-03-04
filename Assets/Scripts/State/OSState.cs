using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class OSState : IState
    {
        private IPresenter inventoryPresenter;
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public OSState(IPresenter inventoryPresenter)
        {
            this.inventoryPresenter = inventoryPresenter;
            
        }

        public void Begin()
        {
            this.inventoryPresenter.Show();
        }

        public void End()
        {
            this.inventoryPresenter.Hide();
        }

        public void Update()
        {
            this.inventoryPresenter.Repaint();   
  
        }

        public void FixedUpdate()
        {

        }

        public void LateUpdate()
        {
            
        }

        public void Menu()
        {
            Game.Instance.SetMainMenu();
        }

        public void Select()
        {
            Game.Instance.SetPlaying();
        }
    }
}