using UnityEngine;

namespace HackedDesign
{
    public class LoadingState : IState
    {
        UI.AbstractPresenter loadingPresenter;
        PlayerController player;
        int x = 0;

        public LoadingState(PlayerController player, UI.AbstractPresenter loadingPresenter)
        {
            this.loadingPresenter = loadingPresenter;
            this.player = player;
        }

        public bool Playing => false;

        public void Begin()
        {
            x = 0;
            this.player.NewLevel();
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
            if (x == 0) // hack to skip a frame
            {
                x = 1;
                return;
            }
            this.loadingPresenter.Repaint();
            Debug.Log("Loading Level");
            Game.Instance.Level.SpawnLevel(Game.Instance.Settings, Game.Instance.Data.currentLevel);
            Debug.Log("Loading Enemies");
            Game.Instance.Enemies.Spawn(Game.Instance.Data.currentLevel, Game.Instance.Settings);
            Debug.Log("Level loaded");
            Game.Instance.AddConsoleMessage("Arena Loaded");
            if (Game.Instance.Data.skipIntro)
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