using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class Act3State : IState
    {

        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        private ActPresenter presenter;

        public Act3State(ActPresenter presenter)
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
        }

        public void End() => this.presenter.Hide();

        public void Update() { }

        public void FixedUpdate() { }

        public void LateUpdate() { }

        public void Menu() { }

        public void Select() { }
    }
}