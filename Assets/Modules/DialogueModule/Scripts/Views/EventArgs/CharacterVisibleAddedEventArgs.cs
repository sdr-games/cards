namespace SDRGames.Whist.DialogueModule.Views
{
    public class CharacterVisibleAddedEventArgs
    {
        public int CharactersAdded { get; private set; }

        public CharacterVisibleAddedEventArgs(int charactersAdded = 1)
        {
            CharactersAdded = charactersAdded;
        }
    }
}