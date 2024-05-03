using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class TalamusManager : TalentManager
    {
        private Talamus _talamus;
        private TalamusPresenter _talamusPresenter;

        public void Initialize(UserInputController userInputController, TalamusScriptableObject talamusScriptableObject)
        {
            _talamus = new Talamus(talamusScriptableObject);
            base.Initialize(userInputController, _talamus);

            _talamusPresenter = new TalamusPresenter(_talamus, TalentView, talamusScriptableObject.CalculatePositionInContainer());
        }
    }
}
