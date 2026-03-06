#nullable enable

namespace HackedDesign
{
    public class JumpCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.SetJump();
    }
}