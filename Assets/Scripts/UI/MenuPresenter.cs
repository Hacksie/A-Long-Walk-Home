using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class MenuPresenter : AbstractPresenter
    {
        [SerializeField] private GameObject playPanel;
        [SerializeField] private GameObject optionsPanel;
        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private UnityEngine.UI.Slider masterSlider;
        [SerializeField] private UnityEngine.UI.Slider sfxSlider;
        [SerializeField] private UnityEngine.UI.Slider musicSlider;
        [SerializeField] private UnityEngine.UI.Toggle controlsToggle;
        [SerializeField] private UnityEngine.UI.Button quitButton;
        [SerializeField] private UnityEngine.UI.Text difficultyDescText;

        void Start()
        {
            PopulateValues();
            DisableQuitButton();
        }

        public override void Repaint()
        {
            switch (Game.Instance.Data.menuState)
            {
                case MainMenuState.Play:
                    creditsPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    break;
                case MainMenuState.Options:
                    creditsPanel.SetActive(false);
                    optionsPanel.SetActive(true);
                    break;
                case MainMenuState.Credits:
                    creditsPanel.SetActive(true);
                    optionsPanel.SetActive(false);
                    break;
                case MainMenuState.Quit:
                    creditsPanel.SetActive(false);
                    optionsPanel.SetActive(false);
                    break;

            }
        }

        public void PlayClick()
        {
            Game.Instance.Data.menuState = MainMenuState.Play;
            Game.Instance.SetLoading();
        }

        public void OptionsClick()
        {
            Game.Instance.Data.menuState = MainMenuState.Options;
        }

        public void CreditsClick()
        {
            Game.Instance.Data.menuState = MainMenuState.Credits;
        }

        public void QuitClick()
        {
            Game.Instance.Data.menuState = MainMenuState.Quit;
            Game.Instance.Quit();
        }

        public void OnMasterChangedEvent(float sliderValue)
        {
            PlayerPreferences.Instance.masterVolume = sliderValue;
            PlayerPreferences.Instance.Save();
            AudioManager.Instance.SetMasterVolume(sliderValue);
        }

        public void OnSFXChangedEvent(float sliderValue)
        {
            PlayerPreferences.Instance.sfxVolume = sliderValue;
            PlayerPreferences.Instance.Save();
            AudioManager.Instance.SetSFXVolume(sliderValue);
            AudioManager.Instance.PlayTest();
        }

        public void OnMusicChangedEvent(float sliderValue)
        {
            PlayerPreferences.Instance.musicVolume = sliderValue;
            PlayerPreferences.Instance.Save();
            AudioManager.Instance.SetMusicVolume(sliderValue);
        }

        public void OnControlsToggle()
        {
            PlayerPreferences.Instance.mechControls = controlsToggle.isOn;
            PlayerPreferences.Instance.Save();

        }

        public void OnDifficultyChangedEvent(float sliderValue)
        {
            PlayerPreferences.Instance.difficulty = Mathf.RoundToInt(sliderValue);
            difficultyDescText.text = DifficultyDesc(PlayerPreferences.Instance.difficulty);
            PlayerPreferences.Instance.Save();
        }

        private void PopulateValues()
        {
            masterSlider.value = PlayerPreferences.Instance.masterVolume;
            sfxSlider.value = PlayerPreferences.Instance.sfxVolume;
            musicSlider.value = PlayerPreferences.Instance.musicVolume;
            controlsToggle.isOn = PlayerPreferences.Instance.mechControls;
        }

        private string DifficultyDesc(int level)
        {
            switch (level)
            {
                case 3:
                    return "Nightmare";
                case 2:
                    return "Hard";
                case 1:
                    return "Normal";
                default:
                case 0:
                    return "Easy";
            }
        }

        private void DisableQuitButton()
        {
            if (quitButton is null)
            {
                Debug.LogError("Quit button not set");
                return;
            }
            quitButton.interactable = Application.platform != RuntimePlatform.WebGLPlayer;
        }

    }
    public enum MainMenuState
    {
        Play,
        Options,
        Credits,
        Quit
    }
}