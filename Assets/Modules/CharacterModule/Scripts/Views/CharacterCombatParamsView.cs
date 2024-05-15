using SDRGames.Whist.PointsModule.Views;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Views
{
    public class CharacterCombatParamsView : MonoBehaviour
    {
        [field: SerializeField] public PointsBarView ArmorPointsBarView { get; protected set; }
        [field: SerializeField] public PointsBarView BarrierPointsBarView { get; protected set; }
        [field: SerializeField] public PointsBarView HealthPointsBarView { get; protected set; }

        private void OnEnable()
        {
            if (HealthPointsBarView == null)
            {
                Debug.LogError("Health PointsBar View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (ArmorPointsBarView == null)
            {
                Debug.LogError("Armor PointsBar View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (BarrierPointsBarView == null)
            {
                Debug.LogError("Barrier PointsBar View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
