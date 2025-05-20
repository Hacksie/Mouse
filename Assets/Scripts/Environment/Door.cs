using UnityEngine;
using System.Collections;

namespace HackedDesign
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private float timerStart = 0;

        private void Awake()
        {
            this.AutoBind(ref animator);
        }

        public bool IsOpen { get; private set; } = false;

        public void Trigger()
        {
            if (!IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        private void Open()
        {
            IsOpen = true;
            animator.SetTrigger("open");
            timerStart = Time.time;
            StartCoroutine(AutoClose());
        }

        private void Close()
        {
            IsOpen = false;
            animator.SetTrigger("close");
        }

        private IEnumerator AutoClose()
        {
            if(Time.time < (timerStart + 5))
            {
                yield return null;
            }

            Close();

        }

        private void OnCollisionStay(Collision collision)
        {
            timerStart = Time.time;
        }
    }
}