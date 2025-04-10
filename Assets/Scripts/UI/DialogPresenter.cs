using HackedDesign.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign.UI
{
    public class DialogPresenter : AbstractPresenter
    {
        [SerializeField] private TargetPresenter targetPresenter;
        [SerializeField] private UnityEngine.UI.Text dialogText;
        [SerializeField] private UnityEngine.UI.Image dialogAvatar;
        [SerializeField] private Sprite defaultAvatar;
        [HideInInspector] public UnityEvent finishedEvent;

        private int currentPage = 0;

        private new void Awake()
        {
            base.Awake();
        }

        public override void Hide()
        {
            targetPresenter.Hide();
            base.Hide();
        }

        public override void Repaint()
        {
            var page = DialogManager.Instance.CurrentDialog?.pages[currentPage];
            if (page != null)
            {
                dialogText.text = page.text;
                dialogAvatar.sprite = DialogManager.Instance.GetSpeakerSprite(page);
                targetPresenter.Repaint(page.speaker);
                targetPresenter.Show();
            }
            else
            {
                Debug.LogWarning("Unknown dialog", this);
                dialogText.text = "";
                dialogAvatar.sprite = defaultAvatar;
            }
        }

        public void NextClick()
        {
            currentPage++;

            Debug.Log("Page" + currentPage + "/" + DialogManager.Instance.CurrentDialog.pages.Count, this);

            if (currentPage >= DialogManager.Instance.CurrentDialog.pages.Count)
            {
                Debug.Log("Hide", this);
                currentPage = 0;
                Hide();
                finishedEvent.Invoke();
                
            }
            else
            {
                Debug.Log("Next Page", this);
                Repaint();
            }
        }
    }
}
