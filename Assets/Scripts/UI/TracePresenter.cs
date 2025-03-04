using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign.UI
{
    public class TracePresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Text traceLabel;
        [SerializeField] private UnityEngine.UI.Slider traceSlider;

        public override void Repaint()
        {
            traceLabel.text = Game.Instance.LevelTimer.Time.ToString("N0");
            traceSlider.maxValue = Game.Instance.LevelTimer.InitialTime;
            traceSlider.value = Game.Instance.LevelTimer.Time;
        }
    }
}
