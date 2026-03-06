#nullable enable

namespace HackedDesign
{
    public class FacingCommand: ICharacterCommand
    {
        private readonly float movementDirection;
        private readonly float facingDirection;
        public FacingCommand(float movementDirection, float facingDirection)
        {
            this.movementDirection = movementDirection;
            this.facingDirection = facingDirection;
        }

        public void Execute(CharController controller) => controller.UpdateSpriteDirection(movementDirection, facingDirection);
    }
}