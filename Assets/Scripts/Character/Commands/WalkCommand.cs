#nullable enable

namespace HackedDesign
{
    public class WalkCommand : ICharacterCommand
    {
        private readonly bool state;
        public WalkCommand(bool state) => this.state = state;

        public void Execute(CharController controller) => controller.SetWalk(this.state);
    }
}