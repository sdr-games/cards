namespace SDRGames.Whist.TurnSwitchModule.Managers
{
    public class TurnSwitchedEventArgs
    {
        public bool IsPlayerTurn { get; private set; }
        public bool IsCombatTurn { get; private set; }
        public int EnemyIndex { get; private set; }

        public TurnSwitchedEventArgs(bool isPlayerTurn, bool isCombatTurn, int enemyIndex)
        {
            IsPlayerTurn = isPlayerTurn;
            IsCombatTurn = isCombatTurn;
            EnemyIndex = enemyIndex;
        }
    }
}