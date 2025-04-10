
using UnityEngine;

namespace HackedDesign
{
    public class LevelTimer : MonoBehaviour
    {
        [SerializeField] private float levelTime = 64;

        private CountdownTimer timer;

        public CountdownTimer Timer { get => timer; }
        //public float InitialTime { get => timer.InitialTime; }

        public void Reset()
        {
            this.timer = new CountdownTimer(levelTime);
        }
    }
}
