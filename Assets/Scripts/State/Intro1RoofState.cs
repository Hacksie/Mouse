using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class Intro1RoofState : IState
    {
        private const string DialogId = "intro_roof1";
        private const string LevelName = "Rooftop";
        private readonly PlayerController player;
        private readonly Level level;

        public bool PlayerActionAllowed => false;
        public bool Battle => false;

        public Intro1RoofState(PlayerController player, Level level)
        {
            this.player = player;
            this.level = level;
        }

        public void Begin()
        {
            this.player.Character.ExecuteCommand(new FacingCommand(0, 1f));
            this.player.Character.SetSitState();
            this.level.ShowNamedRoom(LevelName, true, false, this.player);
            DialogManager.Instance.ShowDialog(DialogId, new UnityEngine.Events.UnityAction(DialogOver));
        }

        private void DialogOver() => Game.Instance.SetRoom1();

        public void End() => DialogManager.Instance.HideDialog();

        public void Update() => this.player.UpdateSitBehaviour();

        public void FixedUpdate() => this.player.FixedUpdateBehaviour();

        public void LateUpdate() => this.player.LateUpdateBehaviour();

        public void Menu()
        {
            //GameManager.Instance.SetStartMenu();
        }

        public void Select()
        {

        }
    }
}