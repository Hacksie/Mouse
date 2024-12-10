using HackedDesign.UI;
using System;
using UnityEngine;

namespace HackedDesign.UI
{
    public class DeathPresenter : AbstractPresenter
    {

        private void Awake()
        {
            base.Awake();
        }

        public override void Repaint()
        {

        }

        public void RestartClick()
        {
            Game.Instance.SetPlaying();
        }

        public void ExitClick()
        {
            Game.Instance.SetMainMenu();
        }
    }
}
