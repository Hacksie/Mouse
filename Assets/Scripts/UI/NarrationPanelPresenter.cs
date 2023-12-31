﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HackedDesign.Story;
using HackedDesign.Dialogue;

namespace HackedDesign.UI
{
    public class NarrationPanelPresenter : AbstractPresenter
    {

        public Narration currentNarration = null;
        public Text text;
        public Button actionButton;
        public Text handleText;
        public Text shortNameText;
        public Text categoryText;
        public Text corpText;
        public Image avatarSprite;

        private NarrationManager narrationManager;

        public void Initialize(NarrationManager narrationManager)
        {
            this.narrationManager = narrationManager;

            if (text == null) Debug.LogError("Text is null");
            if (actionButton == null) Debug.LogError("Button is null");
        }

        public override void Repaint()
        {          
            if (currentNarration != narrationManager.GetCurrentNarration())
            {
                RepaintNarration();
            }
        }

        private void RepaintNarration()
        {
            currentNarration = narrationManager.GetCurrentNarration();

            if (currentNarration == null)
            {
                Hide();
                return;
            }

            var speaker = InfoRepository.Instance.GetCharacter(currentNarration.speaker);
            var corp = InfoRepository.Instance.GetCorp(speaker.corp);
            handleText.text = speaker.handle;
            shortNameText.text = speaker.fullName;

            switch (currentNarration.speakerEmotion)
            {
                case "tired":
                    avatarSprite.sprite = speaker.avatarTired;
                    break;
                case "thinking":
                    avatarSprite.sprite = speaker.avatarThinking;
                    break;
                case "happy":
                    avatarSprite.sprite = speaker.avatarHappy;
                    break;
                case "angry":
                    avatarSprite.sprite = speaker.avatarAngry;
                    break;
                case "smirking":
                    avatarSprite.sprite = speaker.avatarSmirking;
                    break;
                default:
                    avatarSprite.sprite = speaker.avatar;
                    break;
            }

            corpText.text = corp != null ? "<color=\"" + corp.color + "\">" + corp.name + "</color>" : "Free agent";
            text.text = currentNarration.text;
            EventSystem.current.SetSelectedGameObject(actionButton.gameObject);
        }
    }
}