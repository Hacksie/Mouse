using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class SecCam: MonoBehaviour
    {
        [SerializeField] private List<ISensor> sensors;
        private void Awake()
        {
            foreach (var sensor in GetComponentsInChildren<ISensor>())
            {
                sensor.OnTargetChanged += Alert;
            }
            ;

            /*
            this.AutoBind(ref sensor);
            sensor.OnTargetChanged += Alert;*/
        }
        public void Alert()
        {
            Debug.Log("triggered");
        }
    }
}
