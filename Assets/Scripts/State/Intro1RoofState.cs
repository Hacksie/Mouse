using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class Intro1RoofState : IState
    {
        private PlayerController player;
        private Level level;

        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public Intro1RoofState(PlayerController player, Level level)
        {
            this.player = player;
            this.level = level;
        }

        public void Begin()
        {
            this.level.ShowNamedRoom("Rooftop", true, false, this.player);
            this.player.Sit();
            DialogManager.Instance.ShowDialog("IntroRoof1", new UnityEngine.Events.UnityAction(DialogOver));
        }

        private void DialogOver()
        {
            Debug.Log("Dialog over");
            Game.Instance.SetRoom1();
        }

        public void End()
        {
            
        }

        public void Update()
        {
            this.player.UpdateSitBehaviour();
        }

        public void FixedUpdate()
        {
            this.player.FixedUpdateBehaviour();
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            
        }

        public void Menu()
        {
            //GameManager.Instance.SetStartMenu();
        }

        public void Select()
        {

        }
    }
}