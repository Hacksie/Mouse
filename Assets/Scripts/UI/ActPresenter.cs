using HackedDesign.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign.UI
{
    public class ActPresenter : AbstractPresenter
    {
        [HideInInspector] public UnityEvent finishedEvent;

        private new void Awake()
        {
            base.Awake();
        }

        public override void Repaint()
        {

        }

        public void NextClick()
        {
            finishedEvent.Invoke();
        }
    }
}
