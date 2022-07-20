using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class IntroState : IState
    {
        UI.AbstractPresenter dialogPanel;

        public IntroState(UI.AbstractPresenter dialogPanel)
        {
            this.dialogPanel = dialogPanel;
        }

        public bool Playing => false;

        public void Begin()
        {
            // if(Game.Instance.Settings.skipIntro)
            // {
            //     Game.Instance.SetPlaying();
            // }
            //Time.timeScale = 0;
            AudioManager.Instance.PlayLoopMusic();
            this.dialogPanel.Show();
        }

        public void End()
        {
            //Time.timeScale = 1;
            this.dialogPanel.Hide();
        }

        public void FixedUpdate()
        {
        }

        public void Start()
        {
            Game.Instance.SetPlaying();
        }

        public void Select()
        {
            Game.Instance.SetPlaying();
        }        

        public void Update()
        {
            this.dialogPanel.Repaint();
        }

        private void UpdateCameras()
        {
            
        }
    }
}