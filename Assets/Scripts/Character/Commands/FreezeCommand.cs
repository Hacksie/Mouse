#nullable enable

namespace HackedDesign
{
    public class FreezeCommand : ICharacterCommand
    {
        public void Execute(CharController controller) => controller.Freeze();
    }
}