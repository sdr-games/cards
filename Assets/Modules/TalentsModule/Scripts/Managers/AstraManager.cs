using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class AstraManager : TalentManager
    {
        private Astra _astra;
        private AstraPresenter _astraPresenter;

        public void Initialize(UserInputController userInputController, AstraScriptableObject astraScriptableObject, Vector2 position)
        {
            _astra = new Astra(astraScriptableObject);
            base.Initialize(userInputController, _astra);

            _astraPresenter = new AstraPresenter(_astra, TalentView, position);
        }
    }
}
