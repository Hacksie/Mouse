using HackedDesign.UI;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class Room1State : IState
    {
        private PlayerController player;
        private Level level;

        public bool PlayerActionAllowed => true;
        public bool Battle => false;


        public Room1State(PlayerController player, Level level)
        {
            this.player = player;
            this.level = level;
        }

        public void Begin()
        {
            this.level.Reset();
            this.level.ShowNamedRoom("Mouse Starting Room", false, true, this.player);
            this.player.Character.SetIdleState();
        }

        public void End()
        {
            
        }

        public void Update()
        {
            this.player.UpdateIdleBehaviour();
        }

        public void FixedUpdate()
        {
            this.player.FixedUpdateBehaviour();
        }

        public void LateUpdate()
        {
            this.player.LateUpdateBehaviour();
            
        }

        public void Menu()
        {
            //GameManager.Instance.SetStartMenu();
        }

        public void Select()
        {

        }
    }
}