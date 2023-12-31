using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HackedDesign.UI
{
    public class TitlecardPresenter : AbstractPresenter
    {
        [Header("Referenced GameObjects")]
        public Text titleText;
        public Button continueButton;
        [Header("Settings")]
        public string[] titleStrings;
        public string[] nextActions;

        private Story.SceneManager actionManager;

        public void Initialize(Story.SceneManager actionManager)
        {
            this.actionManager = actionManager;
        }

        public override void Repaint()
        {
            titleText.text = titleStrings[GameManager.Instance.Data.Story.act];
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        }

        public void ClickEvent()
        {
            actionManager.CurrentScene.Begin();
            //actionManager.Invoke(nextActions[GameManager.Instance.GameState.Story.act]);
        }
    }
}