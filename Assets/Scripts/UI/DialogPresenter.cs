using HackedDesign.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign.UI
{
    public class DialogPresenter : AbstractPresenter
    {
        [SerializeField] private Typewriter typewriter;
        [SerializeField] private UnityEngine.UI.Text nameText;
        [SerializeField] private UnityEngine.UI.Text dialogText;
        [SerializeField] private UnityEngine.UI.Image dialogAvatar;
        [SerializeField] private Sprite defaultAvatar;
        [HideInInspector] public UnityEvent finishedEvent;

        private int currentPage = 0;

        private new void Awake()
        {
            base.Awake();
            this.AutoBind(ref typewriter);
            typewriter.Text = dialogText;
        }

        public override void Repaint()
        {
            if (DialogManager.Instance.CurrentDialog != null && DialogManager.Instance.CurrentDialog[currentPage] != null)
            {
                var page = DialogManager.Instance.CurrentDialog[currentPage];
                dialogText.text = page.Text;
                dialogAvatar.sprite = DialogManager.Instance.GetSpeakerSprite(page);
                nameText.text = page.Speaker;
                typewriter.Play(page.Text);
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

            Debug.Log("Page " + currentPage + "/" + DialogManager.Instance.CurrentDialog.Count, this);

            if (currentPage >= DialogManager.Instance.CurrentDialog.Count)
            {
                Debug.Log("Hide", this);
                currentPage = 0;
                Hide();
                finishedEvent.Invoke();
                
            }
            else
            {
                Repaint();
            }
        }
    }
}
