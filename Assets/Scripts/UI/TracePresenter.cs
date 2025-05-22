using UnityEngine;

namespace HackedDesign.UI
{
    public class TracePresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Text traceLabel;
        [SerializeField] private UnityEngine.UI.Slider traceSlider;

        public override void Repaint()
        {
            traceLabel.text = Game.Instance.LevelTimer.Timer.Time.ToString("N0");
            traceSlider.maxValue = Game.Instance.LevelTimer.Timer.InitialTime;
            traceSlider.value = Game.Instance.LevelTimer.Timer.Time;
        }
    }
}
