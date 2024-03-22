namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    public class MagicDamageMultiplierChangedEventArgs
    {
        public int MagicDamageMultiplier { get; set; }

        public MagicDamageMultiplierChangedEventArgs(int magicDamageMultiplier)
        {
            MagicDamageMultiplier = magicDamageMultiplier;
        }
    }
}