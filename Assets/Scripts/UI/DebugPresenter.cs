using System.Text;
using UnityEngine;

namespace HackedDesign.UI
{
    public class DebugPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Text debugText;

        public override void Repaint()
        {
            /*
            var sb = new StringBuilder();
            sb.Append("Current Mission: ");
            sb.Append(Game.Instance.Player.Character.OperatingSystem.CurrentMission);
            sb.Append('\n');

            debugText.text = sb.ToString();*/
        }
    }
}
