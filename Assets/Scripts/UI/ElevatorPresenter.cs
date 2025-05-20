using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class ElevatorPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Button elevatorButtonPrefab;
        [SerializeField] private RectTransform elevatorButtonGroup;

        public override void Repaint()
        {
            for (int i = 0; i < elevatorButtonGroup.childCount; i++)
            {
                elevatorButtonGroup.GetChild(i).gameObject.SetActive(false);
                Destroy(elevatorButtonGroup.GetChild(i).gameObject);
            }

            for (int i = 1; i <= ElevatorManager.Instance.elevators.Count; i++)
            {
                var idx = i;
                var button = Instantiate(elevatorButtonPrefab, elevatorButtonGroup);

                var label = button.GetComponentInChildren<UnityEngine.UI.Text>();

                label.text = idx.ToString();
                button.onClick.AddListener(() => ButtonClickEvent(idx));
            }
        }

        public void ButtonClickEvent(int buttonId)
        {
            ElevatorManager.Instance.GoToFloor(buttonId);
            Game.Instance.SetPlaying();
        }
    }
}
