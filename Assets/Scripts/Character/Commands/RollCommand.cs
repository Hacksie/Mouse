#nullable enable

namespace HackedDesign
{
    public class RollCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.Roll();
    }
}