namespace HackedDesign
{
    public class EnemyIdleState: IEnemyState
    {
        private readonly AI ai;

        public EnemyIdleState(AI ai)
        {
            this.ai = ai;
        }

        public void UpdateBehaviour(AIContext ctx)
        {
            ai.Character.ExecuteCommand(new AimCommand(false));

            if (ctx.canSeePlayer && (ctx.canHearPlayer || ctx.playerInFrontOfUs))
            {
                
                ai.CurrentState = new EnemySearchingState(this.ai);
                return;
            }

            ai.Character.ExecuteCommand(new FacingCommand(0, ctx.facing));
            ai.Character.ExecuteCommand(new MoveCommand(0, 0));
        }

        public void Begin()
        {

        }

        public void End()
        {

        }
    }
}
