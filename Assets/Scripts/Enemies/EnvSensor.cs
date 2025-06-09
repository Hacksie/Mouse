using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class EnvSensor : MonoBehaviour
    {
        [SerializeField] private Collider2D sensor;

        public Collider2D Sensor { get => sensor; private set => sensor = value; }

        public bool IsTouching
        {
            get; private set;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IsTouching = true;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            IsTouching = true;

            
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            IsTouching = false;
        }
    }
}
