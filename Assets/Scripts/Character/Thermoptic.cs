using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class Thermoptic: MonoBehaviour
    {
        [SerializeField] new Renderer renderer;
        [SerializeField] private Material defaultMaterial = null;
        [SerializeField] private Material thermopticMaterial = null;

        [SerializeField] private bool active = false;

        public bool Active { get { return active; } set { active = value; renderer.material = active ? thermopticMaterial : defaultMaterial; } }

        private void Awake()
        {
            this.AutoBind(ref renderer);
        }
    }
}
