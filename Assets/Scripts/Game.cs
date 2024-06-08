using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Game : MonoBehaviour
    {
        public const string gameVersion = "1.0";
        [Header("Game")]
        [SerializeField] private PlayerController playerController = null;

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

        void Awake() => CheckBindings();
        void Start() => Initialization();  

        private void Update() => CurrentState.Update();
        private void LateUpdate() => CurrentState.LateUpdate();
        private void FixedUpdate() => CurrentState.FixedUpdate();

        public void SetPlaying() => CurrentState = new PlayingState(this.playerController);                    

        public void SetQuit() => Application.Quit();        

        public void NewGame()
        {
            //DataManager.Instance.NewGame(levels[0], GetRandomCorp(), GetRandomCorpName());
            //Player.Reset();
            SetPlaying();
        }     

        private void CheckBindings()
        {
        }

        private void Initialization()
        {
            NewGame();
        }         


    }
}