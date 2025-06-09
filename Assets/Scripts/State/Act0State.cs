using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class Act0State : IState
    {

        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        private ActPresenter presenter;

        public Act0State(ActPresenter presenter)
        {
            this.presenter = presenter;
            this.presenter.finishedEvent.AddListener(Continue);
        }

        public void Begin()
        {
            this.presenter.Show();
        }

        private void Continue()
        {
            if (Game.Instance.GameSettings.SkipIntro)
            {
                 // Random.Range(int.MinValue, int.MaxValue);
                Game.Instance.SetLoading();
            }
            else
            {
                //SetIntermission();
                Game.Instance.SetRoof1();
            }

        }

        public void End() => this.presenter.Hide();

        public void Update() { }

        public void FixedUpdate() { }

        public void LateUpdate() { }

        public void Menu() { }

        public void Select() { }
    }
}