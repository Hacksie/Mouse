using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class TripLaser : MonoBehaviour
    {
        private new Collider2D collider2D;
        private Animator animator;
        [SerializeField] private int amount = 200;
        [SerializeField] private DamageType damageType = DamageType.Damage;

        private void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
            //animator.SetBool("on", true);
            //animator.SetFloat("type", 1);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Player))
            {
                var hit = collider2D.ClosestPoint(other.transform.position);
                AttackPlayer(hit);
            }
        }

        private void AttackPlayer(Vector2 hit)
        {
            AnimateAttack();

            if (damageType == DamageType.Damage)
            {
                Game.Instance.Player.Character.TakeDamage(amount, hit + (Vector2.up * 0.5f), Vector2.up);
            }
            else if (damageType == DamageType.Momentum)
            {
                Game.Instance.Player.Character.TakeMomentumHit(amount, hit, Vector2.up);
            }
        }

        private void AnimateAttack()
        {
            if (animator)
            {
                animator.SetTrigger("attack");
                StartCoroutine(Reset());
            }
        }

        private IEnumerator Reset()
        {
            yield return new WaitForEndOfFrame();
            if (animator)
            {
                animator.ResetTrigger("attack");
            }
        }
    }

    public enum DamageType
    {
        None,
        Damage,
        Momentum,
    }
}
