using SDRGames.Whist.MeleeCombatModule.Presenters;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class MeleeAttackManager : MonoBehaviour
    {
        [SerializeField] private MeleeAttackView _meleeAttackView;

        public void Initialize(UserInputController userInputController, MeleeAttackScriptableObject meleeAttackScriptableObject)
        {
            new MeleeAttackPresenter(userInputController, meleeAttackScriptableObject, _meleeAttackView);
        }

        private void OnEnable()
        {
            if (_meleeAttackView == null)
            {
                Debug.LogError("Melee Attack View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
