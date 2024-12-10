using HackedDesign.UI;
using System;
using UnityEngine;

namespace HackedDesign.UI
{
    public class ActionBarPresenter : AbstractPresenter
    {
        [Header("Data")]
        [SerializeField] private GameData gameData;
        [Header("UI")]
        [SerializeField] private UnityEngine.UI.Slider healthSlider;
        [SerializeField] private UnityEngine.UI.Text ammoText;

        private void Awake()
        {
            base.Awake();
            gameData.changeActions += Repaint;
            Repaint();
        }

        public override void Repaint()
        {
            healthSlider.value = gameData.Health;
            ammoText.text = gameData.Ammo.ToString();
        }

        public void Ping()
        {
            Game.Instance.Player.Ping();
        }
    }
}
