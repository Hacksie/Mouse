using UnityEngine;
using Unity.Cinemachine;

namespace HackedDesign
{
    public class CameraShake : AutoSingleton<CameraShake>
    {
        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private float frequency = 2.0f;
        private CinemachineBasicMultiChannelPerlin perlinNoise;

        private float shakeTimer = 0;

 
        new void Awake()
        {
            base.Awake();
            perlinNoise = virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        }

        public void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0)
                {
                    shakeTimer = 0;
                    Shake(0, 0);
                }
            }
        }

        public void Shake(float intensity, float time)
        {
            perlinNoise.AmplitudeGain = intensity;
            perlinNoise.FrequencyGain = frequency;
            shakeTimer = time;
        }
    }
}



