using UnityEngine;

namespace HackedDesign
{
    public class GameOverState : IState
    {
        private UI.AbstractPresenter gameoverPresenter;
        public GameOverState(UI.AbstractPresenter gameoverPresenter)
        {
            this.gameoverPresenter = gameoverPresenter;
        }

        public bool Playing => false;

        public void Begin()
        {
            AudioManager.Instance.PlayDeadMusic();
            Game.Instance.Player.Die();
            this.gameoverPresenter.Show();
            this.gameoverPresenter.Repaint();
        }

        public void End()
        {
            this.gameoverPresenter.Hide();
            AudioManager.Instance.StopDeadMusic();
            
        }

        public void FixedUpdate()
        {
            
        }

        public void Start()
        {
            Game.Instance.SetMenu();
        }

        public void Select()
        {
            
        }

        public void Update()
        {
            
        }
    }

}