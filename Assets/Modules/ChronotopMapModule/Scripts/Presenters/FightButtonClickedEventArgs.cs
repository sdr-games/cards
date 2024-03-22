using SDRGames.Whist.CharacterModule.ScriptableObjects;

namespace SDRGames.Whist.ChronotopMapModule.Presenters
{
    public class FightButtonClickedEventArgs
    {
        public CommonCharacterParamsModel EnemyParams { get; private set; }

        public FightButtonClickedEventArgs(CommonCharacterParamsModel enemyParams)
        {
            EnemyParams = enemyParams;
        }
    }
}