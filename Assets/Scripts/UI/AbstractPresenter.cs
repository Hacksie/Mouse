#nullable enable
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace HackedDesign.UI
{
    public abstract class AbstractPresenter : MonoBehaviour, IPresenter
    {
        public virtual void Show()
        {
            if (!gameObject.activeInHierarchy)
            {
                EventSystem.current.GetComponent<InputSystemUIInputModule>()?.actionsAsset.Disable();
                EventSystem.current.GetComponent<InputSystemUIInputModule>()?.actionsAsset.Enable();
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