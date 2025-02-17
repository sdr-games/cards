using SDRGames.Whist.SettingsModule.Views;
using SDRGames.Whist.UserInputModule.ScriptableObjects;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.SettingsModule
{
    public class ControlsSettingsManager : MonoBehaviour
    {
        [SerializeField] private InterfaceBindings _interfaceBindings;
        [SerializeField] private CombatBindings _combatBindings;

        public void ChangeAlchemyKey(HotkeyChangeSettingsEventArgs e)
        {
            _interfaceBindings.SetKey(nameof(InterfaceBindings.AlchemyKey), e.Value);
        }

        public void ChangeBestiaryKey(HotkeyChangeSettingsEventArgs e)
        {
            _interfaceBindings.SetKey(nameof(InterfaceBindings.BestiaryKey), e.Value);
        }

        public void ChangeCardCollectionsKey(HotkeyChangeSettingsEventArgs e)
        {
            _interfaceBindings.SetKey(nameof(InterfaceBindings.CardCollectionsKey), e.Value);
        }

        public void ChangeJournalKey(HotkeyChangeSettingsEventArgs e)
        {
            _interfaceBindings.SetKey(nameof(InterfaceBindings.JournalKey), e.Value);
        }

        public void ChangeQuestLogKey(HotkeyChangeSettingsEventArgs e)
        {
            _interfaceBindings.SetKey(nameof(InterfaceBindings.QuestLogKey), e.Value);
        }

        public void ChangeTalentsKey(HotkeyChangeSettingsEventArgs e)
        {
            _interfaceBindings.SetKey(nameof(InterfaceBindings.TalentsKey), e.Value);
        }

        public void ChangeQuickStrikeKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.QuickStrikeKey), e.Value);
        }

        public void ChangeFirstCardKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.FirstCardKey), e.Value);
        }

        public void ChangeMediumStrikeKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.MediumStrikeKey), e.Value);
        }

        public void ChangeSecondCardKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.SecondCardKey), e.Value);
        }

        public void ChangeHeavyStrikeKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.HeavyStrikeKey), e.Value);
        }

        public void ChangeThirdCardKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.ThirdCardKey), e.Value);
        }

        public void ChangeFourthCardKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.FourthCardKey), e.Value);
        }

        public void ChangeDeleteLastSlotKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.DeleteLastSlotKey), e.Value);
        }

        public void ChangeEmptySlotKey(HotkeyChangeSettingsEventArgs e)
        {
            _combatBindings.SetKey(nameof(CombatBindings.EmptySlotKey), e.Value);
        }

        private void OnEnable()
        {
            if (_interfaceBindings == null)
            {
                Debug.LogError("Interface Bindings не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_combatBindings == null)
            {
                Debug.LogError("Combat Bindings не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
