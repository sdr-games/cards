using SDRGames.Islands.DiceModule.Views;
using SDRGames.Islands.PointsModule.Views;

using TMPro;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Views
{
    public class PlayerCharacterParamsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameTMP;
        [SerializeField] private TextMeshProUGUI _levelTMP;
        [SerializeField] private TextMeshProUGUI _magicDamageMultiplierTMP;

        [field: SerializeField] public PointsTextView HealthPointsView { get; private set; }
        [field: SerializeField] public PointsTextView StaminaPointsView { get; private set; }
        [field: SerializeField] public PointsTextView BreathPointsView { get; private set; }
        [field: SerializeField] public PointsTextView PhysicalArmorPointsView { get; private set; }
        [field: SerializeField] public PointsTextView MagicShieldPointsView { get; private set; }
        [field: SerializeField] public DiceView PhysicalDamageDiceView { get; private set; }

        public void Initialize(string name, string level, string magicDamageMultiplier)
        {
            _nameTMP.text = name;
            SetLevelText(level);
            SetMagicDamageMultiplierText(magicDamageMultiplier);
        }

        public void SetLevelText(string level)
        {
            _levelTMP.text = level;
        }

        public void SetMagicDamageMultiplierText(string magicDamageMultiplier)
        {
            _magicDamageMultiplierTMP.text = magicDamageMultiplier;
        }

        #region MonoBehaviour methods
        private void OnEnable()
        {
            if (_nameTMP == null)
            {
                Debug.LogError("Name TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_levelTMP == null)
            {
                Debug.LogError("Level TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (HealthPointsView == null)
            {
                Debug.LogError("Health Points View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (StaminaPointsView == null)
            {
                Debug.LogError("Stamina Points View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (BreathPointsView == null)
            {
                Debug.LogError("Breath Points View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (PhysicalArmorPointsView == null)
            {
                Debug.LogError("Physical Armor Points View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (MagicShieldPointsView == null)
            {
                Debug.LogError("Magic Shield Points View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (PhysicalDamageDiceView == null)
            {
                Debug.LogError("Physical Damage Dice View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_magicDamageMultiplierTMP == null)
            {
                Debug.LogError("Magic Damage Multiplier TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
        #endregion
    }
}
