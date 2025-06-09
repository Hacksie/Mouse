
using System.Collections;
using UnityEngine;

namespace HackedDesign
{
    public class Bird : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float lookaroundTime = 300f;
        [SerializeField] private float flyingDelay = 120f;
        [SerializeField] private float minFlyingSpeedX = 10f;
        [SerializeField] private float maxFlyingSpeedX = 10f;
        [SerializeField] private float minFlyingSpeedY = 0.25f;
        [SerializeField] private float maxFlyingSpeedY = 4f;
        [SerializeField] private float flyingTime = 3f;

        private bool flying = false;
        private float flyingStart = 0;
        private float flyingX = 0;
        private float flyingY = 0;

        void Awake() => this.AutoBind(ref animator);

        void OnEnable()
        {
            StartAtRandomIdleFrame();
            StartCoroutine(PlayLookaround());
            flyingStart = Time.time;
        }

        void Update()
        {
            if (this.flying)
            {
                transform.position = new Vector2(transform.position.x + (this.flyingX * Time.deltaTime), transform.position.y + (this.flyingY * Time.deltaTime));
                return;
            }

            if (Time.time - flyingStart <= flyingDelay)
            {
                return;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                StartFlyAway();
            }
        }

        private void StartAtRandomIdleFrame() => animator.Play(AnimatorParams.Idle, -1, Random.value);

        private IEnumerator PlayLookaround()
        {
            yield return new WaitForSeconds(Random.Range(0, lookaroundTime));
            Debug.Log("Bird Lookaround", this);
            animator.SetTrigger(AnimatorParams.LookAround);
            StartCoroutine(PlayLookaround());
        }

        private void StartFlyAway()
        {
            Debug.Log("Bird Flyaway", this);
            animator.SetTrigger(AnimatorParams.FlyAway);
            this.flying = true;
            this.flyingX = Random.Range(minFlyingSpeedX, maxFlyingSpeedX);
            this.flyingY = Random.Range(minFlyingSpeedY, maxFlyingSpeedY);
            StartCoroutine(FlyingTimeout());
        }

        private IEnumerator FlyingTimeout()
        {
            yield return new WaitForSeconds(flyingTime);
            this.gameObject.SetActive(false);
            Destroy(this);
        }
    }
}
