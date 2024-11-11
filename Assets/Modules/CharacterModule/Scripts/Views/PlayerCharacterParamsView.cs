using SDRGames.Whist.DiceModule.Views;
using SDRGames.Whist.PointsModule.Views;

using TMPro;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Views
{
    public class PlayerCharacterParamsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameTMP;
        [SerializeField] private TextMeshProUGUI _levelTMP;
        [SerializeField] private TextMeshProUGUI _experienceTMP;
        [SerializeField] private TextMeshProUGUI _gloryTMP;
        [SerializeField] private TextMeshProUGUI _physicalDamageTMP;
        [SerializeField] private TextMeshProUGUI _magicDamageTMP;

        [field: SerializeField] public PointsTextView HealthPointsView { get; private set; }
        [field: SerializeField] public PointsTextView StaminaPointsView { get; private set; }
        [field: SerializeField] public PointsTextView BreathPointsView { get; private set; }
        [field: SerializeField] public PointsTextView PhysicalArmorPointsView { get; private set; }
        [field: SerializeField] public PointsTextView MagicShieldPointsView { get; private set; }

        public void Initialize(string name, string level, string experience, string glory, string physicalDamage, string magicDamage)
        {
            _nameTMP.text = name;
            SetLevelText(level);
            SetPhysicalDamageText(physicalDamage);
            SetMagicDamageText(magicDamage);
            SetExperienceText(experience);
            SetGloryText(glory);
        }

        public void SetLevelText(string level)
        {
            _levelTMP.text = level;
        }

        public void SetPhysicalDamageText(string physicalDamage)
        {
            _physicalDamageTMP.text = physicalDamage;
        }

        public void SetMagicDamageText(string magicDamage)
        {
            _magicDamageTMP.text = magicDamage;
        }

        public void SetExperienceText(string experience)
        {
            _experienceTMP.text = experience;
        }

        public void SetGloryText(string glory)
        {
            _gloryTMP.text = glory;
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

            if (_physicalDamageTMP == null)
            {
                Debug.LogError("Physical Damage TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_magicDamageTMP == null)
            {
                Debug.LogError("Magic Damage TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_experienceTMP == null)
            {
                Debug.LogError("Experience TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_gloryTMP == null)
            {
                Debug.LogError("Glory TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
        #endregion
    }
}
