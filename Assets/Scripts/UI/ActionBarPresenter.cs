using HackedDesign.UI;

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HackedDesign.UI
{
    public class ActionBarPresenter : AbstractPresenter
    {
        [Header("Data")]
        //[SerializeField] private CharacterData gameData;
        [SerializeField] private OperatingSystem os;
        [Header("UI")]
        [SerializeField] private UnityEngine.UI.Slider ramSlider;
        [SerializeField] private UnityEngine.UI.Slider healthSlider;
        [SerializeField] private UnityEngine.UI.Slider energySlider;
        [SerializeField] private RectTransform ammoPanel;
        [SerializeField] private UnityEngine.UI.Text ammoText;
        [SerializeField] private List<Button> buttonList = new List<Button>(6);
        [SerializeField] private List<Image> imageList = new List<Image>(6);


        private new void Awake()
        {
            base.Awake();
            os.changeActions += Repaint;
        }

        public override void Repaint()
        {
            healthSlider.value = os.Health;
            ammoPanel.gameObject.SetActive(os.HasPistol);
            if (os.HasPistol)
            {
                ammoText.text = os.Ammo.ToString();
            }
            
            RepaintHacks();
            RepaintRam();
            RepaintEnergy();
        }

        public void RepaintHacks()
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (os.ActiveHacks.Count > i)
                {
                    Hack hack = os.ActiveHacks[i];
                    var text = buttonList[i].gameObject.GetComponentInChildren<Text>();
                    if (text != null)
                    {
                        text.text = hack.shortName ?? hack.name;
                    }
                    else
                    {
                        Debug.LogWarning("Action button has no text component");
                    }

                    var img = imageList[i];
                    if (img != null)
                    {
                        img.sprite = hack.buttonIcon;
                    }
                    else
                    {
                        Debug.LogWarning("Action button has no image component");
                    }
                }
            }
        }

        public void RepaintRam()
        {
            ramSlider.value = os.GetRamUsage();
        }

        private void RepaintEnergy()
        {
            //energySlider.value = os.Momentum;
        }

        public void Action1Click()
        {
            os.Trigger(0);
        }

        public void Action2Click()
        {
            os.Trigger(1);
        }
        public void Action3Click()
        {
            os.Trigger(2);
        }
        public void Action4Click()
        {
            os.Trigger(3);
        }
        public void Action5Click()
        {
            os.Trigger(4);
        }
        public void Action6Click()
        {
            os.Trigger(5);
        }
    }
}
