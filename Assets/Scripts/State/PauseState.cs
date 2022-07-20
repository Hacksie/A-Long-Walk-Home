using UnityEngine;

namespace HackedDesign
{
    public class PauseState : IState
    {
        private UI.AbstractPresenter pausePresenter;
        public PauseState(UI.AbstractPresenter pausePresenter)
        {
            this.pausePresenter = pausePresenter;
        }

        public bool Playing => false;

        public void Begin()
        {
            Time.timeScale = 0;
            this.pausePresenter.Show();
            this.pausePresenter.Repaint();
            AudioManager.Instance.PlayMenuMusic();
        }

        public void End()
        {
            Time.timeScale = 1;
            AudioManager.Instance.StopMenuMusic();
            this.pausePresenter.Hide();
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
            
        }

        public void Update()
        {
            
        }
    }

}