#nullable enable

namespace HackedDesign
{
    public class AimCommand: ICharacterCommand
    {
        private readonly bool state;

        public AimCommand(bool state) => this.state = state;
        public void Execute(CharController controller) => controller.SetAim(state);
    }
}