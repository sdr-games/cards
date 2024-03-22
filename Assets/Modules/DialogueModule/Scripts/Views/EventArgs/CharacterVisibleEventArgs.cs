namespace SDRGames.Whist.DialogueSystem.Views
{
    public class CharacterVisibleEventArgs
    {
        public int CharactersAdded { get; private set; }

        public CharacterVisibleEventArgs(int charactersAdded = 1)
        {
            CharactersAdded = charactersAdded;
        }
    }
}