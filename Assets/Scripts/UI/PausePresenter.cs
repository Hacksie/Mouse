using HackedDesign.UI;
using System;
using UnityEngine;

namespace HackedDesign.UI
{
    public class PausePresenter : AbstractPresenter
    {
        public override void Repaint()
        {

        }

        public void ContinueClick()
        {
            Game.Instance.SetStatePlaying();
        }

        public void ExitClick()
        {
            Game.Instance.SetStateMainMenu();
        }
    }
}
