using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class PropSpawner : MonoBehaviour
    {
        [SerializeField] private bool allowProps = true;
        [SerializeField] private float startGap = 5f;
        [SerializeField] private List<GameObject> props;

        void Awake()
        {
                
        }

        public void SpawnProps()
        {
            var x = startGap;
            var y = transform.position.y;

        }
    }
}
