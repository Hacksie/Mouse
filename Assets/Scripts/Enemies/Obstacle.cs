using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace HackedDesign
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private Rigidbody2D rb;

        private void Awake()
        {
            this.AutoBind(ref anim);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(Tags.Player))
            {
                if (collision.collider.TryGetComponent<CharController>(out var character))
                {
                    var direction = (character.transform.position - transform.position).normalized;
                    character.ExecuteCommand(new KnockbackCommand(direction));
                    StartCoroutine(AnimateFlash());
                }
            }

        }


        private IEnumerator AnimateFlash()
        {
            yield return new WaitForSeconds(0.2f);
            anim.SetTrigger("flash");
            StartCoroutine(Deactivate());
        }

        private IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(0.2f);
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }
}
