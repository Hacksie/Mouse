#nullable enable

namespace HackedDesign
{
    public class WalkToggleCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.WalkToggle();
    }
}