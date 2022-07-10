using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class MenuState : IState
    {
        UI.AbstractPresenter menuPanel;
        private Camera menuCamera;
        private Camera mainCamera;

        public MenuState(Camera mainCamera, Camera menuCamera, UI.AbstractPresenter menuPanel)
        {
            this.mainCamera = mainCamera;
            this.menuCamera = menuCamera;
            this.menuPanel = menuPanel;
        }

        public bool Playing => false;

        public void Begin()
        {
            this.menuPanel.Show();
            mainCamera.gameObject.SetActive(false);
            menuCamera.gameObject.SetActive(true);
        }

        public void End()
        {
            this.menuPanel.Hide();
            mainCamera.gameObject.SetActive(true);
            menuCamera.gameObject.SetActive(false);
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
            this.menuPanel.Repaint();
        }

        private void UpdateCameras()
        {

        }
    }
}