using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.TalentsModule
{
    public class TalamusManager : TalentManager
    {
        private TalamusScriptableObject _talamusScriptableObject;
        private TalamusPresenter _talamusPresenter;

        public void Initialize(UserInputController userInputController, TalamusScriptableObject talamusScriptableObject)
        {
            base.Initialize(userInputController);
            _talamusScriptableObject = talamusScriptableObject;

            _talamusPresenter = new TalamusPresenter(_talamusScriptableObject, _talentView);
        }

        protected override void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                foreach (TalentScriptableObject dependencyTalent in _talamusScriptableObject.Dependencies)
                {
                    if (!dependencyTalent.IsActive)
                    {
                        return;
                    }
                }
                _talamusPresenter.ChangeActive();
            }
        }
    }
}
