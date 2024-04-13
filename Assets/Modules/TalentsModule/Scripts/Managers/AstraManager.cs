using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class AstraManager : TalentManager
    {
        private Astra _astra;
        private AstraPresenter _astraPresenter;

        public void Initialize(UserInputController userInputController, AstraScriptableObject astraScriptableObject)
        {
            base.Initialize(userInputController, astraScriptableObject);
            _astra = new Astra(astraScriptableObject);

            _astraPresenter = new AstraPresenter(_astra, TalentView, astraScriptableObject.Position);
        }
    }
}
