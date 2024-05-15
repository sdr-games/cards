using SDRGames.Whist.CharacterModule.ScriptableObjects;

namespace SDRGames.Whist.ChronotopMapModule.Presenters
{
    public class FightButtonClickedEventArgs
    {
        public CharacterParamsModel EnemyParams { get; private set; }

        public FightButtonClickedEventArgs(CharacterParamsModel enemyParams)
        {
            EnemyParams = enemyParams;
        }
    }
}