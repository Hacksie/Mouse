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
            Debug.Log("New Game", this);
            Game.Instance.NewGame();
            //Game.Instance.SetIntermission();
            //Game.Instance.SetRoom1();
        }



        public void OptionsClick()
        {
            Debug.Log("Main Menu Options", this);
        }

        public void CreditsClick()
        {
            Debug.Log("Credits Click", this);
        }

        public void ExitClick()
        {
            Debug.Log("Exit Click", this);
            Application.Quit();
        }

    }
}
