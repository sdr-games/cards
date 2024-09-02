namespace SDRGames.Whist.TurnSwitchModule.Managers
{
    public class TurnSwitchedEventArgs
    {
        public bool IsPlayerTurn { get; private set; }
        public bool IsCombatTurn { get; private set; }

        public TurnSwitchedEventArgs(bool isPlayerTurn, bool isCombatTurn)
        {
            IsPlayerTurn = isPlayerTurn;
            IsCombatTurn = isCombatTurn;
        }
    }
}