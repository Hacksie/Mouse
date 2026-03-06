#nullable enable

namespace HackedDesign
{
    public class GhostToggleCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.ToggleGhost();
    }
}