using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class MenuPresenter : AbstractPresenter
    {
        public GameObject playPanel;
        public GameObject optionsPanel;
        public GameObject creditsPanel;

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
            Game.Instance.Quit();
        }

        // private void DisableQuitButton()
        // {
        //     quitButton.interactable = Application.platform != RuntimePlatform.WebGLPlayer;
        // }        

    }
    public enum MainMenuState
    {
        Play,
        Options,
        Credits,
        Quit
    }
}