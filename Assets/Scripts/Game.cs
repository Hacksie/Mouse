#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Game : MonoBehaviour
    {
        public const string gameVersion = "1.0";

        [Header("Game")]
        [SerializeField] private Camera? mainCamera = null;
        [SerializeField] private PlayerController? player = null;        

#pragma warning disable CS8618
        public static Game Instance { get; private set; }
#pragma warning restore CS8618     

        private IState state = new EmptyState();

        public IState State
        {
            get
            {
                return this.state;
            }
            private set
            {
                this.state.End();
                this.state = value;
                this.state.Begin();
            }
        }

        private Game() => Instance = this;

        void Awake() => CheckBindings();
        void Start() => Initialization();  
        void Update() => State?.Update();
        void FixedUpdate() => State?.FixedUpdate();              

        public void SetPlaying() => State = new PlayingState(this.player);
        //public void SetLoading() => State = new LoadingState(this.level);
        //public void SetDead() => State = new DeadState(this.player);

        private void CheckBindings()
        {
            //mainCamera = mainCamera ?? Camera.main;
            //player = player ?? GameObject.FindFirstObjectOfType<PlayerController>();            
        }

        private void Initialization()
        {
            HideAllUI();
            //SetLoading();
            SetPlaying();
        }

        private void HideAllUI()
        {
        }



    }
}