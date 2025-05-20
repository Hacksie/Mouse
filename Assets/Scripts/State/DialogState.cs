using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class DialogState : IState
    {
        private readonly PlayerController player;
        private readonly IPresenter dialogPresenter;
        
        public bool PlayerActionAllowed => true;
        public bool Battle => true;


        public DialogState(PlayerController player, IPresenter dialogPresenter)
        {
            this.player = player;
            this.dialogPresenter = dialogPresenter;           
            
        }

        public void Begin()
        {
            this.player.Character.SetSitState();
            this.dialogPresenter.Show();
        }

        public void End()
        {
            this.dialogPresenter.Hide();
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