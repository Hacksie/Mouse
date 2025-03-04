using HackedDesign.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Game : MonoBehaviour
    {
        public const string gameVersion = "1.0";
        [Header("Game")]
        [SerializeField] private PlayerController player = null;
        [SerializeField] private Level level = null;
        [SerializeField] private EnemyPool enemyPool = null;
        [Header("UI")]
        [SerializeField] private MainMenuPresenter mainMenuPresenter = null;
        [SerializeField] private DeathPresenter deathPresenter = null;
        [SerializeField] private PausePresenter pausePresenter = null;
        [SerializeField] private ActionBarPresenter actionBarPresenter = null;
        [SerializeField] private OSPresenter osPresenter = null;
        [SerializeField] private TracePresenter tracePresenter = null;
        
        [Header("Data")]
        [SerializeField] private CountdownTimer levelTimer = null;
        [SerializeField] private float levelTime = 64;
        [SerializeField] private int randomSeed = 2;

        [Header("Settings")]
        [SerializeField] private GameSettings gameSettings = null;

        //[SerializeField] private CharacterData charData = null;

        public static Game Instance { get; private set; }
        private Game() => Instance = this;
        

        private IState currentState;

        public IState CurrentState
        {
            get
            {
                return this.currentState;
            }
            private set
            {
                this.currentState?.End();
                this.currentState = value;
                this.currentState?.Begin();
            }
        }

        public PlayerController Player { get => player; private set => player = value; }
        public CountdownTimer LevelTimer { get => levelTimer; private set => levelTimer = value; }
        public GameSettings GameSettings { get => gameSettings; set => gameSettings = value; }
        public int RandomSeed { get => randomSeed; set => randomSeed = value; }

        void Awake() => CheckBindings();
        void Start() => Initialization();  

        private void Update() => CurrentState.Update();
        private void LateUpdate() => CurrentState.LateUpdate();
        private void FixedUpdate() => CurrentState.FixedUpdate();

        public void ResetLevelTimer()
        {
            levelTimer = new CountdownTimer(levelTime);
        }

        public void SetRoom1() => CurrentState = new Room1State(player, level, actionBarPresenter);
        public void SetIntermission() => CurrentState = new IntermissionState(player, level, actionBarPresenter);
        public void SetPlaying() => CurrentState = new PlayingState(player, level, enemyPool, actionBarPresenter, tracePresenter);

        public void SetLoading() => CurrentState = new LoadingState(player, level, enemyPool);
        public void SetMainMenu() => CurrentState = new MainMenuState(mainMenuPresenter);
        public void SetDeath() => CurrentState = new DeathState(deathPresenter);
        public void SetPaused() => CurrentState = new PausedState(pausePresenter);


        public void SetOS() => CurrentState = new OSState(osPresenter);
        public void SetQuit() => Application.Quit();        

        public void NewGame()
        {
            player.Character.OperatingSystem.Reset();
            //charData.Reset();
            //DataManager.Instance.NewGame(levels[0], GetRandomCorp(), GetRandomCorpName());
            player.Reset();
            if (gameSettings.skipIntro)
            {
                SetLoading();
            }
            else
            {
                SetRoom1();
            }
            
            //SetMainMenu();
        }     

        private void CheckBindings()
        {
        }

        private void Initialization()
        {
            HideUI();
            //NewGame();
            SetMainMenu();
        } 
        
        private void HideUI()
        {
            mainMenuPresenter.Hide();
            deathPresenter.Hide();
            pausePresenter.Hide();
            actionBarPresenter.Hide();
            osPresenter.Hide();
            tracePresenter.Hide();
        }
    }
}