namespace HackedDesign
{
    public interface IEnemyState
    {
        public void Begin();
        public void End();
        public void UpdateBehaviour(AIContext ctx);
    }
}
