using UnityEngine;

namespace HackedDesign
{
    public class EnemyIdleState: IEnemyState
    {
        private readonly IAI ai;
        private bool isRoaming = false;
        private float startPhaseChange = 0;

        private const int MaxPhaseOffset = 10;

        public EnemyIdleState(IAI ai)
        {
            this.ai = ai;
            var facing = Random.value < 0.5f ? 1 : -1;

            this.ai.Character.ExecuteCommand(new FacingCommand(0, facing));
            this.ai.Character.ExecuteCommand(new WalkCommand(true));
            this.isRoaming = Random.value < 0.5f;
            this.startPhaseChange = Time.time + Random.Range(0, MaxPhaseOffset); 
        }

        public void UpdateBehaviour(AIContext ctx)
        {
            this.ai.Character.ExecuteCommand(new AimCommand(false));

            if (ctx.canSeePlayer && (ctx.canHearPlayer || ctx.playerInFrontOfUs))
            {
                this.ai.CurrentState = new EnemySearchingState(this.ai);
                return;
            }

            if(ctx.settings.Stationary)
            {
                return;
            }

            // FIXME: Use constants for facing directions
            if (ctx.wallInFront || ctx.dropInFront)
            {
                this.ai.Character.ExecuteCommand(new FacingCommand(0, ctx.facing * -1));
            }

            if(startPhaseChange + ctx.settings.RoamTime < Time.time)
            {
                
                isRoaming = !isRoaming;
                Debug.Log("Switch roaming " + isRoaming + " " + ctx.name);
                startPhaseChange = Time.time;
            }

            float move = 0;
            if(isRoaming && !ctx.wallInFront && !ctx.dropInFront)
            {
                move = ctx.facing;
            }
            this.ai.Character.ExecuteCommand(new MoveCommand(move, 0));
        }

        public void Begin()
        {

        }

        public void End()
        {

        }
    }
}
