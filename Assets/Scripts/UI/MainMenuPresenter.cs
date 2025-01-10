using HackedDesign.UI;
using System;
using UnityEngine;

namespace HackedDesign.UI
{
    public class MainMenuPresenter : AbstractPresenter
    {

        private new void Awake()
        {
            base.Awake();
        }

        public override void Repaint()
        {

        }

        public void StartClick()
        {
            Game.Instance.SetIntermission();
        }



        public void OptionsClick()
        {
            Debug.Log("Main Menu Options");
        }

        public void CreditsClick()
        {
            Debug.Log("Credits Click");
        }

        public void ExitClick()
        {
            Application.Quit();
        }

    }
}
