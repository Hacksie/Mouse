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
        private readonly IAI ai;

        private float reactionStartTime = 0f;


        private bool sawPlayer = false;

        public EnemySearchingState(IAI ai)
        {
            this.ai = ai;
            ai.Character.ExecuteCommand(new WalkCommand(false));
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

            if (!ctx.settings.Stationary)
            {
                float move = 0;
                if (!ctx.wallInFront && !ctx.dropInFront)
                {
                    move = Mathf.Sign((ctx.position - ctx.lastKnownPlayerPosition).x) < 0 ? 1 : -1;
                }

                ai.Character.ExecuteCommand(new MoveCommand(move, 0));
            }
        }

        public void Begin() => ai.Icon.Searching();

        public void End() => ai.Icon.Hide();
    }
}
