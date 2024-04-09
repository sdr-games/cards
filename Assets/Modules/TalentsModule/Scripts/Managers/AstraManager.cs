using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.TalentsModule
{
    public class AstraManager : TalentManager
    {
        private AstraScriptableObject _astraScriptableObject;
        private AstraPresenter _astraPresenter;

        public void Initialize(UserInputController userInputController, AstraScriptableObject astraScriptableObject)
        {
            base.Initialize(userInputController);
            _astraScriptableObject = astraScriptableObject;

            _astraPresenter = new AstraPresenter(_astraScriptableObject, _talentView);
        }

        protected override void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                foreach (TalentScriptableObject dependencyTalent in _astraScriptableObject.Dependencies)
                {
                    if(!dependencyTalent.IsActive)
                    {
                        return;
                    }
                }
                _astraPresenter.ChangeActive();
            }
        }
    }
}
