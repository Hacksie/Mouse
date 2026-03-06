using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class EmptyState : IState
    {
        public bool PlayerActionAllowed => false;
        public bool Battle => false;


        public EmptyState()
        {
        }

        public void Begin()
        {
        }

        public void End()
        {
        }

        public void Update()
        {

  
        }

        public void FixedUpdate()
        {

        }

        public void LateUpdate()
        {
            
        }

        public void Menu()
        {
        }

        public void Select()
        {

        }
    }
}