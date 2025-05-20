using HackedDesign.UI;

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign.UI
{
    public class TargetPresenter : AbstractPresenter
    {
        [Header("UI")]
        [SerializeField] private UnityEngine.UI.Text nameLabel;
        [SerializeField] private UnityEngine.UI.Slider targetHealthbar;
        [SerializeField] private Targeter targeter;


        private new void Awake()
        {
            base.Awake();
            targeter.targetChangedAction += Repaint;
        }

        public void Repaint(Interactable interactable)
        {
            if (interactable == null)
            {
                Repaint();
                return;
            }

            Show();
            nameLabel.text = string.IsNullOrEmpty(interactable.Label) ? interactable.name : interactable.Label;
            if (interactable.TryGetComponent<CharController>(out var character))
            {
                targetHealthbar.value = character?.OperatingSystem?.Health ?? 0;   
            }
            else
            {
                targetHealthbar.value = 0;
            }
        }

        public void Repaint(string name)
        {
            nameLabel.text = name;
            targetHealthbar.value = 0;
        }

        public override void Repaint()
        {
            Hide();
            nameLabel.text = "";
        }
    }
}
