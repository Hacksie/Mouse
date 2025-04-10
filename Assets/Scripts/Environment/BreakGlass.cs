using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    [RequireComponent(typeof(Collider2D))]
    public class BreakGlass: MonoBehaviour
    {
        [SerializeField] new private Collider2D collider;
        [SerializeField] private ParticleSystem glassBreakEffect;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool playOnce = true;
        

        private float shatterMagnitude = 6.0f;

        private bool hasPlayed = false;
        private void Awake()
        {
            collider = GetComponent<Collider2D>();
        }

        public void Break(Vector3 other)
        {
            if (hasPlayed)
            {
                return;
            }
            var x = other.x - transform.position.x;

            glassBreakEffect.transform.right = x < 0 ? Vector3.right : Vector3.left;

            glassBreakEffect.Play();
            spriteRenderer.enabled = false;
            hasPlayed = true;
            collider.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            if(collision.gameObject.CompareTag("Player") && !(playOnce && hasPlayed))
            {
                Debug.Log(collision.relativeVelocity.magnitude);
                //var pc = collision.gameObject.GetComponent<PhysicsController>();
                if (collision.relativeVelocity.magnitude > shatterMagnitude)
                {
                    

                    Break(collision.otherCollider.transform.position);

                    //var x = collision.otherCollider.transform.position.x - collision.collider.transform.position.x;

                    //glassBreakEffect.transform.right = x < 0 ? Vector3.right : Vector3.left;

                    //glassBreakEffect.Play();
                    //spriteRenderer.active = false;
                    //hasPlayed = true;
                }



            }
        }
    }
}
