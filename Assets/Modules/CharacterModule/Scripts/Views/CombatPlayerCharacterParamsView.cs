using SDRGames.Islands.PointsModule.Views;
using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Views
{
    public class CombatPlayerCharacterParamsView : CombatCommonCharacterParamsView
    {
        [field: SerializeField] public PointsBarView StaminaPointsBarView { get; protected set; }
        [field: SerializeField] public PointsBarView BreathPointsBarView { get; protected set; }

        private void OnEnable()
        {
            if (StaminaPointsBarView == null)
            {
                Debug.LogError("Stamina PointsBar View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (BreathPointsBarView == null)
            {
                Debug.LogError("Breath PointsBar View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
