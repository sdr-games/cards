using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class EnemyCharacterCombatManager : MonoBehaviour
    {
        [SerializeField] private CharacterParamsModel _characterParamsModel;
        [SerializeField] private CharacterCombatParamsView _characterCombatParamsView;

        private CharacterCombatParamsPresenter _characterCombatParamsPresenter;

        public void Initialize()
        {
            _characterCombatParamsPresenter = new CharacterCombatParamsPresenter(_characterParamsModel, _characterCombatParamsView);
        }

        public CharacterParamsModel GetParams()
        {
            return _characterParamsModel;
        }

        public void TakeDamage(int damage)
        {
            _characterCombatParamsPresenter.TakeDamage(damage);
        }

        private void OnEnable()
        {
            if (_characterCombatParamsView == null)
            {
                Debug.LogError("Common Character Combat Params View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
