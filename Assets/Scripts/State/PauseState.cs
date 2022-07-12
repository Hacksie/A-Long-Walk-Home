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
            Debug.Log("Pause");
            this.pausePresenter.Show();
            this.pausePresenter.Repaint();
        }

        public void End()
        {
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