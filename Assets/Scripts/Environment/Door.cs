using UnityEngine;
using System.Collections;
using System.IO;

namespace HackedDesign
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private float timerStart = 0;

        private void Awake() => this.AutoBind(ref animator);

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
            if(animator != null)
            {
                animator.SetTrigger("open");
            }
            this.timerStart = Time.time;
            StartCoroutine(AutoClose());
        }

        private void Close()
        {
            IsOpen = false;
            if (animator != null)
            {
                animator.SetTrigger("close");
            }
        }

        private IEnumerator AutoClose()
        {
            //yield return new WaitForSeconds(6);
            while (Time.time < (this.timerStart + 2))
            {
                yield return null;
            }

            Close();
        }

        private void OnCollisionStay(Collision collision) => this.timerStart = Time.time;
    }
}