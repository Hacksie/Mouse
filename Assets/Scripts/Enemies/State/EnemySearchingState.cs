using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class EnemySearchingState : IEnemyState
    {
        //private readonly EnemyController enemyController;
        private readonly AI ai;

        private float reactionStartTime = 0f;


        private bool sawPlayer = false;

        public EnemySearchingState(AI ai)
        {
            //this.enemyController = enemyController;
            this.ai = ai;
        }

        public void UpdateBehaviour(AIContext ctx)
        {
            if (Game.Instance.Player.Character.IsDead)
            {
                return;
                // FIXME: Go to a post game state
            }

            if (!sawPlayer && (ctx.canSeePlayer || ctx.canHearPlayer))
            {
                sawPlayer = true;
                reactionStartTime = Time.time;
            }
            else if (sawPlayer && !(ctx.canSeePlayer || ctx.canHearPlayer))
            {
                sawPlayer = false;
                reactionStartTime = Time.time;
            }


            if(Time.time >= reactionStartTime + ctx.settings.ReactionTime)
            {
                if(sawPlayer)
                {
                    ai.CurrentState = new EnemyAlertState(ai);
                }
                else
                {
                    if(!ctx.hasSeenPlayer)
                    {
                        ai.CurrentState = new EnemyIdleState(ai);
                    }
                }
            }

            ai.Character.ExecuteCommand(new FacingCommand(0, ctx.facing));
            ai.Character.ExecuteCommand(new MoveCommand(0, 0));
        }

        public void Begin()
        {
            ai.Icon.Searching();
        }

        public void End()
        {
            ai.Icon.Hide();
        }
    }
}
