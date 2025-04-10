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
            if (collision.collider.CompareTag("Player"))
            {
                if (collision.collider.TryGetComponent<CharController>(out var character))
                {
                    Debug.Log("Trigger" + character.tag);
                    var direction = (character.transform.position - transform.position).normalized;
                    Debug.Log(direction);
                    character.Knockback(direction);
                    /*
                    if (rb != null)
                    {
                        rb.AddForce(direction * -5, ForceMode2D.Impulse);
                    }*/
                    StartCoroutine(AnimateFlash());
                    //character.Body.Body.MovePosition(character.transform.position + (direction * 3));
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
