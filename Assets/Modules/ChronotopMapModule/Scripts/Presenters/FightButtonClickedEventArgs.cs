using SDRGames.Whist.CharacterModule.ScriptableObjects;

namespace SDRGames.Whist.ChronotopMapModule.Presenters
{
    public class FightButtonClickedEventArgs
    {
        public CharacterParamsScriptableObject EnemyParams { get; private set; }

        public FightButtonClickedEventArgs(CharacterParamsScriptableObject enemyParams)
        {
            EnemyParams = enemyParams;
        }
    }
}