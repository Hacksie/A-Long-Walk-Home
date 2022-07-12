using UnityEngine;

namespace HackedDesign
{
    public class LoadingState : IState
    {
        UI.AbstractPresenter loadingPresenter;
        int x = 0;

        public LoadingState(UI.AbstractPresenter loadingPresenter)
        {
            this.loadingPresenter = loadingPresenter;
        }

        public bool Playing => false;

        public void Begin()
        {
            x = 0;
            
            this.loadingPresenter.Show();
        }

        public void End()
        {
            this.loadingPresenter.Hide();
        }

        public void FixedUpdate()
        {

        }

        public void Start()
        {

        }

        public void Select()
        {

        }

        public void Update()
        {
            if(x ==0 ) // hack to skip a frame
            {
                x = 1;
                return;
            }
            this.loadingPresenter.Repaint();
            Debug.Log("Loading Level");
            Game.Instance.Level.SpawnLevel(Game.Instance.Settings);
            Debug.Log("Loading Enemies");
            Game.Instance.Enemies.SpawnEnemies(100 + (Game.Instance.Data.currentLevel * 10), Game.Instance.Settings);
            Debug.Log("Level loaded");
            if(Game.Instance.Settings.skipIntro)
            {
                Game.Instance.SetPlaying();
            }
            else 
            {
                Game.Instance.SetIntro();
            }
            
        }
    }

}