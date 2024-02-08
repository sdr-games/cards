namespace SDRGames.Whist.CharacterModule.Models
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