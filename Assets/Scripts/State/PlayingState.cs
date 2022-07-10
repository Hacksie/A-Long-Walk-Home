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
            UpdateOverHeat();
            UpdateNearDeath();
            UpdateAmbientHeatLoss();
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
            
        }

        public void Select()
        {
            Game.Instance.SetLoadout();
        }

        private static void UpdateAmbientHeatLoss()
        {
            Game.Instance.IncreaseHeat(-1.0f * (Game.Instance.Data.ambientHeatLoss) * Time.deltaTime);
        }        

        private void UpdateOverHeat()
        {
            if (Game.Instance.Data.heat >= Game.Instance.Settings.maxHeat)
            {
                Game.Instance.DamageArmour(Game.Instance.Data.heatDamage * Time.deltaTime);
                //AudioManager.Instance.PlayWarning();
            }
        }

        private void UpdateNearDeath()
        {
            if (Game.Instance.Data.armour <= 10)
            {
                //AudioManager.Instance.PlayWarning();
            }
        }


    }

}