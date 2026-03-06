using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class Act0State : IState
    {

        public bool PlayerActionAllowed => false;
        public bool Battle => false;

        private readonly IGame game;
        private readonly bool skipIntro;
        private readonly ActPresenter presenter;

        public Act0State(IGame game, ActPresenter presenter, bool skipIntro)
        {
            this.game = game;
            this.skipIntro = skipIntro;
            this.presenter = presenter;
            this.presenter.finishedEvent.AddListener(Continue);
        }

        public void Begin()
        {
            presenter.Show();
        }

        private void Continue()
        {
            if (skipIntro)
            {
                game.SetStateLoading();
            }
            else
            {
                game.SetStateRoom1();
            }
        }

        public void End() => presenter.Hide();

        public void Update() { }

        public void FixedUpdate() { }

        public void LateUpdate() { }

        public void Menu() { }

        public void Select() { }
    }
}