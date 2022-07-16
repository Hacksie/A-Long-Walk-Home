#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Game : MonoBehaviour
    {
        [Header("Referenced Game Objects")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private List<Camera> loadoutCameras;
        [SerializeField] private Camera loadoutCamera;
        [SerializeField] private Camera menuCamera;
        [SerializeField] private PlayerController player = null;
        [SerializeField] private Pool pool = null;
        [SerializeField] private LevelManager levelManager = null;
        [SerializeField] private EnemyManager enemyManager = null;
        [SerializeField] private CameraShake cameraShake = null;

        [Header("Data")]
        [SerializeField] private int currentSlot = 0;
        [SerializeField] public List<GameData> gameSlots = new List<GameData>(3);
        [SerializeField] private Settings settings;

        [Header("UI")]
        [SerializeField] private UI.ActionPresenter actionPanel = null;
        [SerializeField] private UI.HudPresenter hudPanel = null;
        [SerializeField] private UI.LoadingPresenter loadingPanel = null;
        [SerializeField] private UI.DialogPresenter dialogPanel = null;
        [SerializeField] private UI.MenuPresenter menuPanel = null;
        [SerializeField] private UI.InventoryPresenter invPanel = null;
        [SerializeField] private UI.InvHoverPresenter invHoverPanel = null;
        [SerializeField] private UI.DeadPresenter deadPanel = null;
        [SerializeField] private UI.PausePresenter pausePanel = null;
        [SerializeField] private UI.DragPresenter dragPanel = null;


        private IState state = new EmptyState();

        public static Game Instance { get; private set; }

        public IState State
        {
            get
            {
                return state;
            }
            private set
            {
                state.End();
                state = value;
                state.Begin();
            }
        }

        public PlayerController Player { get { return player; } private set { player = value; } }
        public GameData Data { get { return this.gameSlots[this.currentSlot]; } private set { this.gameSlots[this.currentSlot] = value; } }
        public Pool Pool { get => pool; set => pool = value; }
        public Settings Settings { get => settings; set => settings = value; }
        public LevelManager Level { get => levelManager; set => levelManager = value; }
        public EnemyManager Enemies { get => enemyManager; set => enemyManager = value; }
        public CameraShake CameraShake { get { return cameraShake; } private set { cameraShake = value; } }
        public Camera MainCamera { get => mainCamera; set => mainCamera = value; }
        public int CurrentSlot { get => currentSlot; set => currentSlot = value; }

        Game()
        {
            Instance = this;
        }

        void Awake() => CheckBindings();
        void Start() => Initialization();
        void Update() => state.Update();
        void FixedUpdate() => state.FixedUpdate();

        public void SetPlaying() => State = new PlayingState(Player, Enemies, hudPanel, actionPanel);
        public void SetLoading() => State = new LoadingState(loadingPanel);
        public void SetLoadout() => State = new LoadoutState(mainCamera, loadoutCamera, invPanel);
        public void SetIntro() => State = new IntroState(dialogPanel);
        public void SetMenu() => State = new MenuState(mainCamera, menuCamera, menuPanel);
        public void SetDead() => State = new DeadState(deadPanel);
        public void SetPause() => State = new PauseState(pausePanel);

        public void Quit()
        {
            Application.Quit();
        }

        private void CheckBindings()
        {
            mainCamera = mainCamera ?? Camera.main;
            //player = player ?? GameObject.FindGameObjectWithTag("Player");
            //throw new NotImplementedException();
        }

        public void Reset()
        {
            Data.Reset(Settings);
            Player.Reset();
            Enemies.Reset();
            Level.Reset();
        }

        private void Initialization()
        {
            Reset();
            HideAllUI();
            SetMenu();
        }

        private void HideAllUI()
        {
            actionPanel.Hide();
            hudPanel.Hide();
            loadingPanel.Hide();
            dialogPanel.Hide();
            menuPanel.Hide();
            invPanel.Hide();
            invHoverPanel.Hide();
            deadPanel.Hide();
            pausePanel.Hide();
            dragPanel.Hide();
        }
    }
}