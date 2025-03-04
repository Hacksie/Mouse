using HackedDesign.UI;
using System;
using UnityEngine;

namespace HackedDesign.UI
{
    public class DeathPresenter : AbstractPresenter
    {

        private new void Awake()
        {
            base.Awake();
        }

        public override void Repaint()
        {

        }

        public void RestartClick()
        {
            Game.Instance.SetLoading();
        }

        public void ExitClick()
        {
            Game.Instance.SetMainMenu();
        }
    }
}
