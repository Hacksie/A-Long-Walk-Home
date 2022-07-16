using UnityEngine;

namespace HackedDesign
{
    public class DeadState : IState
    {
        private UI.AbstractPresenter deadPresenter;
        public DeadState(UI.AbstractPresenter deadPresenter)
        {
            this.deadPresenter = deadPresenter;
        }

        public bool Playing => false;

        public void Begin()
        {
            Debug.Log("Dead");
            Game.Instance.Player.Die();
            this.deadPresenter.Show();
            this.deadPresenter.Repaint();
        }

        public void End()
        {
            this.deadPresenter.Hide();
            
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