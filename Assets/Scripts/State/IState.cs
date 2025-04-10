
namespace HackedDesign
{
    public interface IState
    {
        /// <summary>
        /// Called by Game when a state change occurs, allowing the new state to perform any initialisation
        /// </summary>
        void Begin();
        /// <summary>
        /// Called by Game within the Unity update loop
        /// </summary>
        void Update();

        /// <summary>
        /// Called by Game within the Unity LateUpdate loop
        /// </summary>
        void LateUpdate();

        /// <summary>
        /// Called by Game within the Unity FixedUpdate loop
        /// </summary>
        void FixedUpdate();

        /// <summary>
        /// Called by Game just before a state change occurs, allowing the state to clean itself up.
        /// </summary>
        void End();

        /// <summary>
        /// Allows a state to handle if the 'Menu' controller button is pressed by the player
        /// </summary>
        void Menu();

        /// <summary>
        /// Allows a state to handle if the 'Select' controller button is pressed by the player
        /// </summary>
        void Select();
        
        /// <summary>
        /// Is the player allowed to do anything in this state
        /// </summary>
        bool PlayerActionAllowed { get; }

        /// <summary>
        /// Is the player considered in battle in this state
        /// </summary>
        bool Battle { get; }
    }
}