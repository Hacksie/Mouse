#nullable enable

namespace HackedDesign
{
    public interface ICharacterExecute
    {
        public void ExecuteCommand(ICharacterCommand cmd);
    }
}