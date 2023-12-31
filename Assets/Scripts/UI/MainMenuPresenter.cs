﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

using HackedDesign.Level;

namespace HackedDesign.UI
{
    public class MainMenuPresenter : AbstractPresenter
    {
        public GameObject playPanel;
        public GameObject optionsPanel;
        public GameObject creditsPanel;
        public GameObject randomPanel;
        public UnityEngine.UI.Dropdown resolutionsDropdown;
        public UnityEngine.UI.Toggle fullScreenToggle;
        public UnityEngine.UI.Slider masterSlider;
        public UnityEngine.UI.Slider ambientSlider;
        public UnityEngine.UI.Slider musicSlider;
        public UnityEngine.UI.Slider fxSlider;
        public UnityEngine.Audio.AudioMixer masterMixer;
        public UnityEngine.UI.InputField seedInput;
        public UnityEngine.UI.Dropdown templateDropdown;
        public UnityEngine.UI.Slider lengthSlider;
        public UnityEngine.UI.Slider heightSlider;
        public UnityEngine.UI.Slider widthSlider;
        public UnityEngine.UI.Dropdown difficultyDropdown;
        public UnityEngine.UI.Slider enemiesSlider;
        public UnityEngine.UI.Slider trapsSlider;
        public GameObject defaultButton;
        public UnityEngine.UI.Button[] slotButtons;

        public LevelGenTemplate[] randomLevelTemplates;

        public void Start()
        {
            ShowOptionsPanel(false);
            ShowCreditsPanel(false);
            ShowRandomPanel(false);
            ShowPlayPanel(false);
            PopulateResolutions();
            PopulateAudioSliders();
            PopulateCorpTemplates();
        }

        public override void Repaint()
        {
            /*
            if (GameManager.Instance.state == GameStateEnum.MainMenu)
            {
                Show();
                EventSystem.current.SetSelectedGameObject(defaultButton);
            }
            else
            {
                Hide();
            }*/
        }

        public void ShowPlayPanel(bool show)
        {
            if (playPanel != null)
            {
                playPanel.SetActive(show);
            }
        }

        public void ShowOptionsPanel(bool show)
        {
            if (optionsPanel != null)
            {
                optionsPanel.SetActive(show);
            }

            EventSystem.current.SetSelectedGameObject(slotButtons[GameManager.Instance.Data.GameSlot].gameObject);
        }

        public void ShowCreditsPanel(bool show)
        {
            if (creditsPanel != null)
            {
                creditsPanel.SetActive(show);
            }
        }

        public void ShowRandomPanel(bool show)
        {
            if (randomPanel != null)
            {
                randomPanel.SetActive(show);
            }
        }

        public void SelectSlot0Event()
        {
            GameManager.Instance.Data.GameSlot = 0;
        }

        public void SelectSlot1Event()
        {
            GameManager.Instance.Data.GameSlot = 1;
        }

        public void SelectSlot2Event()
        {
            GameManager.Instance.Data.GameSlot = 2;
        }


        public void StartNewGameEvent()
        {
            Logger.Log(this, "Start New Game Event");
            ShowCreditsPanel(false);
            ShowOptionsPanel(false);
            ShowRandomPanel(false);
            ShowPlayPanel(false);
            GameManager.Instance.LoadNewGame();
        }

        public void PlayGameEvent()
        {
            Logger.Log(this, "Play Game Event");
            ShowCreditsPanel(false);
            ShowOptionsPanel(false);
            ShowRandomPanel(false);
            ShowPlayPanel(true);
            //GameManager.Instance.LoadNewGame();
            //StartCoroutine (LoadNewGameScenes ( "IntroRoom", "IntroRoom"));
        }

        public void RandomGameEvent()
        {
            Logger.Log(this, "Random Game Event");
            var seed = Random.Range(0, int.MaxValue);
            Random.InitState(seed);
            seedInput.text = seed.ToString();
            ShowCreditsPanel(false);
            ShowOptionsPanel(false);
            ShowRandomPanel(true);
            ShowPlayPanel(false);
        }

        public void DeleteSlotEvent()
        {

        }

        public void StartRandomGameEvent()
        {
            Logger.Log(this, "Start Random Game Event");
            UnityEngine.Random.InitState(System.Convert.ToInt32(seedInput.text));
            ShowCreditsPanel(false);
            ShowOptionsPanel(false);
            ShowRandomPanel(false);
            ShowPlayPanel(false);
            Logger.Log(this, templateDropdown.options[templateDropdown.value].text);
            GameManager.Instance.LoadRandomGame(templateDropdown.options[templateDropdown.value].text, (int)lengthSlider.value, (int)heightSlider.value, (int)widthSlider.value, difficultyDropdown.value, (int)enemiesSlider.value, (int)trapsSlider.value);
        }

        public void OptionsEvent()
        {
            Logger.Log(this, "Options Event");
            ShowCreditsPanel(false);
            ShowOptionsPanel(true);
            ShowRandomPanel(false);
            ShowPlayPanel(false);
        }

        public void CreditsEvent()
        {
            Logger.Log(this, "Credits Event");
            ShowCreditsPanel(true);
            ShowOptionsPanel(false);
            ShowRandomPanel(false);
            ShowPlayPanel(false);
        }

        public void QuitEvent()
        {
            Logger.Log(this, "Quit Event");
            Application.Quit();
        }

        private void PopulateCorpTemplates()
        {
            templateDropdown.ClearOptions();
            templateDropdown.AddOptions(randomLevelTemplates.ToList().ConvertAll(t => new UnityEngine.UI.Dropdown.OptionData(t.name.ToString())));
        }

        private void PopulateResolutions()
        {
            resolutionsDropdown.ClearOptions();
            resolutionsDropdown.AddOptions(Screen.resolutions.ToList().ConvertAll(r => new UnityEngine.UI.Dropdown.OptionData(r.ToString())));
            resolutionsDropdown.value = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);
            fullScreenToggle.isOn = Screen.fullScreen;
        }

        private void SetResolution()
        {
            Resolution res = Screen.resolutions.ToList()[resolutionsDropdown.value];
            Screen.SetResolution(res.width, res.height, fullScreenToggle.isOn, res.refreshRate);
        }

        private void PopulateAudioSliders()
        {
            float masterVolume;
            float ambientVolume;
            float fxVolume;
            float musicVolume;
            masterMixer.GetFloat("MasterVolume", out masterVolume);
            masterMixer.GetFloat("AmbientVolume", out ambientVolume);
            masterMixer.GetFloat("FXVolume", out fxVolume);
            masterMixer.GetFloat("MusicVolume", out musicVolume);
            masterSlider.value = masterVolume;
            ambientSlider.value = ambientVolume;
            fxSlider.value = fxVolume;
            musicSlider.value = musicVolume;
        }
    }
}