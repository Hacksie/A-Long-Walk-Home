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
            Game.Instance.Level.SpawnLevel(Game.Instance.Settings);
            Game.Instance.Enemies.SpawnEnemies(100 + (Game.Instance.Data.currentLevel * 10));
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