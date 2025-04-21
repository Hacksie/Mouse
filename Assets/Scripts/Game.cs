using HackedDesign.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Game : AutoSingleton<Game>
    {
        public const string gameVersion = "1.0";
        [Header("Game")]
        [SerializeField] private PlayerController player = null;
        [SerializeField] private Level level = null;
        [SerializeField] private EnemyPool enemyPool = null;
        [SerializeField] private LevelTimer levelTimer = null;
        [Header("UI")]
        [SerializeField] private MainMenuPresenter mainMenuPresenter = null;
        [SerializeField] private DeathPresenter deathPresenter = null;
        [SerializeField] private PausePresenter pausePresenter = null;
        [SerializeField] private ActionBarPresenter actionBarPresenter = null;
        [SerializeField] private OSPresenter osPresenter = null;
        [SerializeField] private TracePresenter tracePresenter = null;
        [SerializeField] private DialogPresenter dialogPresenter = null;
        [SerializeField] private MissionPresenter missionPresenter = null;
        [SerializeField] private TargetPresenter targetPresenter = null;

        [Header("Data")]
        //[SerializeField] private float levelTime = 64;
        [SerializeField] private GameData gameData = new();
        [SerializeField] private int randomSeed = 2;

        [Header("Settings")]
        [SerializeField] private GameSettings gameSettings = null;

        #region Properties
        public PlayerController Player { get => player; private set => player = value; }
        public LevelTimer LevelTimer { get => levelTimer; private set => levelTimer = value; }
        public GameSettings GameSettings { get => gameSettings; set => gameSettings = value; }
        public int RandomSeed { get => randomSeed; set => randomSeed = value; }
        #endregion

        #region Singleton
        //public static Game Instance { get; private set; }
        //private Game() => Instance = this;
        #endregion

        #region Unity Messages
        void Start() => Initialization();

        private void Update() => CurrentState.Update();
        private void LateUpdate() => CurrentState.LateUpdate();
        private void FixedUpdate() => CurrentState.FixedUpdate();
        #endregion

        #region State

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
                Debug.Log("Start" + value.ToString());
                this.currentState = value;
                this.currentState?.Begin();
            }
        }

        public GameData GameData { get => gameData; set => gameData = value; }


        public void SetRoof1() => CurrentState = new Intro1RoofState(player, level);
        public void SetRoom1() => CurrentState = new Room1State(player, level);
        public void SetDialog() => CurrentState = new DialogState(player, dialogPresenter);
        public void SetMissionSelect() => CurrentState = new MissionSelectState(missionPresenter);
        public void SetIntermission() => CurrentState = new IntermissionState(player, level, actionBarPresenter);
        public void SetPlaying() => CurrentState = new PlayingState(player, level, enemyPool, actionBarPresenter, tracePresenter);
        public void SetLoading() => CurrentState = new LoadingState(player, level, enemyPool);
        public void SetMainMenu() => CurrentState = new MainMenuState(mainMenuPresenter);
        public void SetDeath() => CurrentState = new DeathState(deathPresenter);
        public void SetLevelEndState() => CurrentState = new LevelEndState(player);
        public void SetPaused() => CurrentState = new PausedState(pausePresenter);
        public void SetOS() => CurrentState = new OSState(osPresenter);
        public void SetQuit() => Application.Quit();

        #endregion



        public void NewGame()
        {
            gameData.Reset();
            player.Character.OperatingSystem.Reset();
            
            //DataManager.Instance.NewGame(levels[0], GetRandomCorp(), GetRandomCorpName());
            player.Reset();
            
            if (gameSettings.SkipIntro)
            {
                player.Character.OperatingSystem.CurrentMission = 1; // Random.Range(int.MinValue, int.MaxValue);
                SetLoading();
            }
            else
            {
                SetRoof1();
            }
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
            dialogPresenter.Hide();
            missionPresenter.Hide();
            targetPresenter.Hide();
        }
    }
}