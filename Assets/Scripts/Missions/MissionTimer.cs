using UnityEngine;

namespace HackedDesign
{
    public interface IMissionTimer
    {
        CountdownTimer Timer { get; }

        void Reset();
    }

    public class MissionTimer : MonoBehaviour, IMissionTimer
    {
        [SerializeField] private GameSettings gameSettings;

        private CountdownTimer timer;

        public CountdownTimer Timer => timer;

        public void Reset() => timer = new CountdownTimer(gameSettings.DefaultLevelTime);
    }
}
