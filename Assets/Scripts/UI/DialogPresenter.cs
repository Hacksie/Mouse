﻿using HackedDesign.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign.UI
{
    public class DialogPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Text nameText;
        [SerializeField] private UnityEngine.UI.Text dialogText;
        [SerializeField] private UnityEngine.UI.Image dialogAvatar;
        [SerializeField] private Sprite defaultAvatar;
        [HideInInspector] public UnityEvent finishedEvent;

        private int currentPage = 0;

        public override void Repaint()
        {
            var page = DialogManager.Instance.CurrentDialog?.pages[currentPage];
            if (page != null)
            {
                dialogText.text = page.text;
                dialogAvatar.sprite = DialogManager.Instance.GetSpeakerSprite(page);
                nameText.text = page.speaker;
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

            Debug.Log("Page " + currentPage + "/" + DialogManager.Instance.CurrentDialog.pages.Count, this);

            if (currentPage >= DialogManager.Instance.CurrentDialog.pages.Count)
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
