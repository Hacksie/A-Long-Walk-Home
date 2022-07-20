using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class LoadoutState : IState
    {
        private UI.AbstractPresenter invPanel;
        //private UI.AbstractPresenter shopPanel;
        private Camera mainCamera;
        private Camera loadoutCamera;
        //private List<Camera> loadoutCameras;

        public LoadoutState(Camera mainCamera, Camera loadoutCamera, UI.AbstractPresenter invPanel)
        {
            this.invPanel = invPanel;
            //this.shopPanel = shopPanel;
            this.mainCamera = mainCamera;
            //this.loadoutCameras = shopCameras;
            this.loadoutCamera = loadoutCamera;
        }

        public bool Playing => false;

        public void Begin()
        {
            Time.timeScale = 0;
            this.invPanel.Show();
            //this.shopPanel.Show();
            //shopCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            loadoutCamera.gameObject.SetActive(true);
            AudioManager.Instance.PlayMenuMusic();
            //GameManager.Instance.GameData.heat = 0;
            //GameManager.Instance.GameData.coolant = GameManager.Instance.GameSettings.maxCoolant;
            
        }

        public void End()
        {
            AudioManager.Instance.StopMenuMusic();
            Time.timeScale = 1;
            //this.shopPanel.Hide();
            this.invPanel.Hide();
            mainCamera.gameObject.SetActive(true);
            loadoutCamera.gameObject.SetActive(false);
            //loadoutCameras.ForEach((camera) => camera.gameObject.SetActive(false));
            //Game.Instance.Player.Weapons.ClearTempWeapons();
        }

        public void FixedUpdate()
        {
        }

        public void Start()
        {
        }

        public void Select()
        {
            Game.Instance.SetPlaying();
        }        

        public void Update()
        {
            //UpdateCameras();
            //this.shopPanel.Repaint();
            this.invPanel.Repaint();
        }

        private void UpdateCameras()
        {
            // for(int i=0;i<loadoutCameras.Count;i++)
            // {
            //     loadoutCameras[i].gameObject.SetActive((WeaponPosition)i == Game.Instance.Player.Weapons.selectedPrimaryWeapon);
            // }
        }
    }
}