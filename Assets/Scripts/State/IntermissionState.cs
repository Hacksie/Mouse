using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class IntermissionState : IState
    {
        private readonly PlayerController player;
        private readonly Level level;
        private readonly IPresenter actionPresenter;

        public bool PlayerActionAllowed => true;
        public bool Battle => false;


        public IntermissionState(PlayerController player, Level level, IPresenter actionPresenter)
        {
            this.player = player;
            this.level = level;
            this.actionPresenter = actionPresenter;
        }

        public void Begin()
        {
            
            this.level.Reset();
            this.level.ShowNamedRoom("Hotdog Stand", false, true, this.player);
            this.player.Sit();
            DialogManager.Instance.ShowDialog("IntroIntermission1", new UnityEngine.Events.UnityAction(Intro1Over));
        }

        public void Intro1Over()
        {
            Debug.Log("Intro1 over");

            DialogManager.Instance.ShowDialog("IntroIntermission2", new UnityEngine.Events.UnityAction(Intro2Over));
        }

        public void Intro2Over()
        {

        }


        public void End()
        {
            this.actionPresenter.Hide();
        }

        public void Update()
        {
            this.player.UpdateBehavior();
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