using System;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class BecameInsaneEventArgs: EventArgs
    {
        public int InsanityTurns { get; private set; }

        public BecameInsaneEventArgs(int insanityTurns)
        {
            InsanityTurns = insanityTurns;
        }
    }
}