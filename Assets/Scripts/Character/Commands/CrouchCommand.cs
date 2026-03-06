#nullable enable

namespace HackedDesign
{
    public class CrouchCommand: ICharacterCommand
    {
        private readonly bool state;
        public CrouchCommand(bool state) => this.state = state;
        public void Execute(CharController controller) => controller.SetCrouch(state);
    }
}