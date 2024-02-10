using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class EnemyCharacterManager : MonoBehaviour
    {
        [SerializeField] private CommonCharacterParamsModel _commonCharacterParamsModel;
        [SerializeField] private CombatCommonCharacterParamsView _combatCommonCharacterParamsView;

        public void Initialize()
        {
            new CombatCommonCharacterParamsPresenter(_commonCharacterParamsModel, _combatCommonCharacterParamsView);
        }

        private void OnEnable()
        {
            if (_combatCommonCharacterParamsView == null)
            {
                Debug.LogError("Combat Common Character Params View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
