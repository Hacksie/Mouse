namespace HackedDesign
{
    public interface IState
    {
        void Begin();
        void Update();
        
        void LateUpdate();
        void FixedUpdate();
        void End();
        void Menu();
        void Select();
        
        bool PlayerActionAllowed { get; }
        bool Battle { get; }
    }
}