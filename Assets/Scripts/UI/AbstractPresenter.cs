using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public interface IPresenter
    {
        void Show();
        void Hide();
        void Toggle();
        void Repaint();
    }

    public abstract class AbstractPresenter : MonoBehaviour, IPresenter
    {
        public void Awake()
        {
            //Hide();
        }
        
        public virtual void Show()
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
        }

        public virtual void Hide()
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }

        public void Toggle() => gameObject.SetActive(!gameObject.activeInHierarchy);

        public abstract void Repaint();
    }
}