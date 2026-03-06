using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class HitGround : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Player))
            {
                Die();
            }
        }

        private static void Die()
        {
            Game.Instance.SetStateDeath();
            //if (Level.Instance.LevelComplete)
            //{
                Game.Instance.SetStateLevelEnd();
            //}
        }
    }
}
