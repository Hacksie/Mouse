using HackedDesign.UI;
using System;
using UnityEngine;

namespace HackedDesign.UI
{
    public class PausePresenter : AbstractPresenter
    {

        private new void Awake()
        {
            base.Awake();
        }

        public override void Repaint()
        {

        }

        public void ContinueClick()
        {
            Game.Instance.SetPlaying();
        }

        public void ExitClick()
        {
            Game.Instance.SetMainMenu();
        }
    }
}
