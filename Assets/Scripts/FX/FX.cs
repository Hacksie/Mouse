#nullable enable
using System.Collections;
using UnityEngine;

namespace HackedDesign
{
    [RequireComponent(typeof(ParticleSystem))]
    public class FX: MonoBehaviour
    {
        [SerializeField] private new ParticleSystem? particleSystem;
        [SerializeField] private FXType fxType;

        void Awake()
        {
            this.AutoBind(ref particleSystem);
        }
        public void Spawn(Vector3 position, Vector3 direction)
        {
            this.transform.position = position;
            this.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }

        public void Despawn()
        {
            if (particleSystem.EnsureNotNull(nameof(FX)))
            {
                particleSystem.Stop();
                
            }
            this.gameObject.SetActive(false);
        }

        public bool Playing => particleSystem != null && particleSystem.isPlaying;

        public FXType FxType { get => this.fxType; set => this.fxType = value; }
    }

    public enum FXType
    {
        Blood,
        EnvHit,
        Machine
    }
}
