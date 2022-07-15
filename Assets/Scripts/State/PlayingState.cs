using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        PlayerController player;
        EnemyManager enemies;
        UI.AbstractPresenter hudPresenter;
        UI.AbstractPresenter actionPresenter;

        public PlayingState(PlayerController player, EnemyManager enemies, UI.AbstractPresenter hudPresenter, UI.AbstractPresenter actionPresenter)
        {
            this.player = player;
            this.enemies = enemies;
            this.hudPresenter = hudPresenter;
            this.actionPresenter = actionPresenter;
        }

        public bool Playing => true;

        public void Begin()
        {
            this.hudPresenter.Show();
            this.actionPresenter.Show();
        }

        public void End()
        {
            this.hudPresenter.Hide();
            this.actionPresenter.Hide();
        }

        public void Update()
        {
            this.player.Mech.UpdateStatus(Time.deltaTime);
            UpdateNearDeath();

            this.player.UpdateBehaviour();
            this.enemies.UpdateBehaviour();
            this.hudPresenter.Repaint();
            this.actionPresenter.Repaint();


        }

        public void FixedUpdate()
        {
            this.player.FixedUpdateBehaviour();
        }

        public void Start()
        {
            Game.Instance.SetPause();
        }

        public void Select()
        {
            Game.Instance.SetLoadout();
        }



        private void UpdateNearDeath()
        {
            // if (Game.Instance.Data.armour <= 10)
            // {
            //     //AudioManager.Instance.PlayWarning();
            // }
        }


    }

}