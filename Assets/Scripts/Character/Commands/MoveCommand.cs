#nullable enable

namespace HackedDesign
{
    public class MoveCommand : ICharacterCommand
    {
        private readonly float movementDirection;
        private readonly float climb;
        public MoveCommand(float movementDirection, float climb)
        {
            this.movementDirection = movementDirection;
            this.climb = climb;
        }
        public void Execute(CharController controller) => controller.SetMovement(movementDirection, climb);
    }
}