using SDRGames.Whist.CharacterInfoModule.ScriptableObjects;

namespace SDRGames.Whist.ChronotopMapModule.Presenters
{
    public class FightButtonClickedEventArgs
    {
        public CharacterInfoScriptableObject EnemyParams { get; private set; }

        public FightButtonClickedEventArgs(CharacterInfoScriptableObject enemyParams)
        {
            EnemyParams = enemyParams;
        }
    }
}