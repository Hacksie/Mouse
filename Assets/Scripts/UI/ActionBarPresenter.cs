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
        [SerializeField] private Slider ramSlider;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider energySlider;
        [SerializeField] private RectTransform attackPanel;
        [SerializeField] private RectTransform gunPanel;
        [SerializeField] private Text ammoText;
        [SerializeField] private List<Button> buttonList = new List<Button>(6);
        [SerializeField] private List<Image> imageList = new List<Image>(6);


        void Awake()
        {
            os.changeActions += Repaint;
        }

        public override void Repaint()
        {
            healthSlider.value = os.Health;
            gunPanel.gameObject.SetActive(os.CurrentWeapon.weaponType == WeaponType.Gun);
            if (os.CurrentWeapon.weaponType == WeaponType.Gun)
            {
                ammoText.text = os.Ammo.ToString();
            }
            
            RepaintHacks();
            RepaintMomentum();
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

        public void RepaintMomentum()
        {
            ramSlider.maxValue = os.MaxMomentum;
            ramSlider.value = os.Momentum;
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
