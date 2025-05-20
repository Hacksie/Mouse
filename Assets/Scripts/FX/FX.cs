using System.Collections;
using UnityEngine;

namespace HackedDesign
{
    public class FX: MonoBehaviour
    {
        [SerializeField] private float aliveTime = 0.5f;
        [SerializeField] private Animator animator;

        void Awake()
        {
            this.AutoBind(ref animator);
        }
        public void Spawn(FXType type)
        {
            this.gameObject.SetActive(true);
            animator.SetFloat("Random", Random.value);
            animator.SetTrigger(type.ToString());
            StartCoroutine(Despawn(type));
        }

        private IEnumerator Despawn(FXType type)
        {
            yield return new WaitForEndOfFrame();
            animator.ResetTrigger(type.ToString());
            yield return new WaitForSeconds(aliveTime);
            FXOver();
        }

        void FXOver() => this.gameObject.SetActive(false);
    }

    public enum FXType
    {
        Blood,
        EnvHit
    }
}
