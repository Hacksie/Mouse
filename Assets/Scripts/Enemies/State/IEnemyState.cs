namespace HackedDesign
{
    public interface IEnemyState
    {
        public bool IsAlive { get; }
        public void Begin();
        public void End();
        public void UpdateBehaviour(AiContext ctx);
    }
}
