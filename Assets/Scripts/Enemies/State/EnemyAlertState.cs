using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class EnemyAlertState: IEnemyState
    {
        //private readonly EnemyController enemyController;
        private readonly IAI ai;
        private float startTriggerTime = 0;

        private bool sawPlayer = false;
        private float lastSawPlayer = 0;

        public EnemyAlertState(IAI ai)
        {
            //this.enemyController = enemyController;
            this.ai = ai;
            this.ai.Character.ExecuteCommand(new WalkCommand(false));
        }

        public void UpdateBehaviour(AIContext ctx)
        {
            if(Game.Instance.Player.Character.IsDead)
            {
                return;
                // FIXME: Go to a post game state
            }

            if (!ctx.settings.Stationary && ctx.canSeePlayer)
            {
                this.ai.Character.ExecuteCommand(new FacingCommand(0, Game.Instance.Player.transform.position.x <= this.ai.Character.transform.position.x ? -1 : 1));
            }
            this.ai.Character.ExecuteCommand(new AimCommand(true));

            var canSeePlayer = ctx.canSeePlayer;

            if(!canSeePlayer && lastSawPlayer + ctx.settings.GiveUpTime <= Time.time)
            {
                this.ai.CurrentState = new EnemySearchingState(this.ai);
                return;
            }

            if (!canSeePlayer && sawPlayer)
            {
                sawPlayer = false;
                lastSawPlayer = Time.time;
            }
            else if(canSeePlayer)
            {
                sawPlayer = true;
                if (startTriggerTime + ctx.settings.RecognitionTime <= Time.time)
                {
                    this.ai.Character.Attack(ctx.lastKnownPlayerPosition, true);
                    startTriggerTime = Time.time + this.ai.Character.Settings.AttackRate;
                }
            }

            this.ai.Character.ExecuteCommand(new MoveCommand(0, 0));
        }


        public void Begin()
        {
            startTriggerTime = Time.time;
            lastSawPlayer = Time.time;
            sawPlayer = true;
            this.ai.Icon.Alert();
        }

        public void End() => this.ai.Icon.Hide();
    }
}
