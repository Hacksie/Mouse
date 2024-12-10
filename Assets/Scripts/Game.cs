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
        [SerializeField] private LevelGenerator level = null;
        [Header("UI")]
        [SerializeField] private UI.MainMenuPresenter mainMenuPresenter = null;
        [SerializeField] private UI.DeathPresenter deathPresenter = null;
        [SerializeField] private UI.ActionBarPresenter actionBarPresenter = null;
        [Header("Data")]
        [SerializeField] private GameData gameData = null;



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
                if (currentState != null)
                {
                    this.currentState.End();
                }
                this.currentState = value;
                if (this.currentState != null)
                {
                    this.currentState.Begin();
                }
            }
        }

        public PlayerController Player { get => player; private set => player = value; }

        void Awake() => CheckBindings();
        void Start() => Initialization();  

        private void Update() => CurrentState.Update();
        private void LateUpdate() => CurrentState.LateUpdate();
        private void FixedUpdate() => CurrentState.FixedUpdate();

        public void SetPlaying() => CurrentState = new PlayingState(this.player, this.level, this.actionBarPresenter);
        public void SetMainMenu() => CurrentState = new MainMenuState(this.mainMenuPresenter);
        public void SetDeath() => CurrentState = new DeathState(this.deathPresenter);

        public void SetQuit() => Application.Quit();        

        public void NewGame()
        {
            gameData.Reset();
            //DataManager.Instance.NewGame(levels[0], GetRandomCorp(), GetRandomCorpName());
            player.Reset();
            SetMainMenu();
        }     

        private void CheckBindings()
        {
        }

        private void Initialization()
        {
            HideUI();
            NewGame();
        } 
        
        private void HideUI()
        {
            this.mainMenuPresenter.Hide();
            this.deathPresenter.Hide();
            this.actionBarPresenter.Hide();
        }


    }
}