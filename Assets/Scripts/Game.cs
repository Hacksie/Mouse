using HackedDesign.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Game : AutoSingleton<Game>
    {
        public const string GameVersion = "1.0";
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
        [SerializeField] private ElevatorPresenter elevatorPresenter = null;
        [SerializeField] private ActPresenter act0Presenter = null;
        [SerializeField] private ActPresenter act1Presenter = null;
        [SerializeField] private ActPresenter act2Presenter = null;
        [SerializeField] private ActPresenter act3Presenter = null;

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
        public void SetElevator() => CurrentState = new ElevatorState(elevatorPresenter);
        public void SetAct0() => CurrentState = new Act0State(act0Presenter);
        public void SetAct1() => CurrentState = new Act1State(act1Presenter);
        public void SetAct2() => CurrentState = new Act2State(act2Presenter);
        public void SetAct3() => CurrentState = new Act3State(act3Presenter);
        public void SetQuit() => Application.Quit();

        #endregion



        public void NewGame()
        {
            gameData.Reset();
            
            //DataManager.Instance.NewGame(levels[0], GetRandomCorp(), GetRandomCorpName());
            player.Reset();

            player.Character.OperatingSystem.CurrentMission = 1;

            SetAct0();


            //if (gameSettings.SkipIntro)
            //{
            //     // Random.Range(int.MinValue, int.MaxValue);
            //    SetLoading();
            //}
            //else
            //{
            //    //SetIntermission();
            //    SetRoof1();
            //}
        }     

        private void Initialization()
        {
            HideUI();
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
            elevatorPresenter.Hide();
            act0Presenter.Hide();
            act1Presenter.Hide();
            act2Presenter.Hide();
            act3Presenter.Hide();
        }
    }
}