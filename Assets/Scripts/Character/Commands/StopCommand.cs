#nullable enable

namespace HackedDesign
{
    public class StopCommand : ICharacterCommand
    {
        public void Execute(CharController controller) => controller.Stop();
    }
}