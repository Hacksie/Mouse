using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class OSState : IState
    {
        private readonly IGame game;
        private readonly IPresenter inventoryPresenter;
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public OSState(IGame game, IPresenter inventoryPresenter)
        {
            this.game = game;
            this.inventoryPresenter = inventoryPresenter;
        }

        public void Begin() => inventoryPresenter.Show();

        public void End() => inventoryPresenter.Hide();

        public void Update() => inventoryPresenter.Repaint();

        public void FixedUpdate()
        {

        }

        public void LateUpdate()
        {
            
        }

        public void Menu() => game.SetStateMainMenu();

        public void Select() => game.SetStatePlaying();
    }
}