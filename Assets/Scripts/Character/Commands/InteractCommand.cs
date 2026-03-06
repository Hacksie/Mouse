#nullable enable

namespace HackedDesign
{
    public class InteractCommand: ICharacterCommand
    {
        public void Execute(CharController controller) => controller.TriggerInteract();
    }
}