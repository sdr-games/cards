using System.Collections.Generic;

using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    public class AbilityScriptableObject : ScriptableObject
    {
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public AbilityLogicScriptableObject[] AbilityLogics { get; private set; }
        [field: SerializeReference] public AbilityModifierScriptableObject[] AbilityModifiers { get; private set; }

        public void ApplyLogics(CharacterCombatManager casterCombatManager, CharacterCombatManager targetCombatManager, int totalSelectedAbilitiesCount)
        {
            if(AbilityModifiers.Length > 0 && totalSelectedAbilitiesCount > 1)
            {
                AbilityModifierScriptableObject modifier = AbilityModifiers[totalSelectedAbilitiesCount - 1];
                if (modifier.SelfUsable)
                {
                    modifier.Apply(casterCombatManager);
                    return;
                }
                modifier.Apply(targetCombatManager);
                return;
            }

            foreach (AbilityLogicScriptableObject logic in AbilityLogics)
            {
                if (logic.SelfUsable)
                {
                    logic.Apply(casterCombatManager);
                    continue;
                }
                logic.Apply(targetCombatManager);
            }
        }

        public void ApplyLogics(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetsCombatManager, int totalSelectedAbilitiesCount, List<int> selectedTargetsIndexes)
        {
            if (AbilityModifiers.Length > 0 && totalSelectedAbilitiesCount > 1)
            {
                AbilityModifierScriptableObject modifier = AbilityModifiers[totalSelectedAbilitiesCount - 1];
                if (modifier.SelfUsable)
                {
                    modifier.Apply(casterCombatManager);
                    return;
                }
                foreach (int index in selectedTargetsIndexes)
                {
                    modifier.Apply(targetsCombatManager[index]);
                }
                return;
            }

            foreach (AbilityLogicScriptableObject logic in AbilityLogics)
            {
                if(logic.SelfUsable)
                {
                    logic.Apply(casterCombatManager);
                    continue;
                }
                foreach(int index in selectedTargetsIndexes)
                {
                    logic.Apply(targetsCombatManager[index]);
                }
            }
        }

        public void ApplyModifiers(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetsCombatManager, int totalSelectedAbilitiesCount, List<CardScriptableObject> cardScriptableObjects)
        {

        }

        private void OnEnable()
        {
            if (Name == null)
            {
                Debug.LogError($"Name не был назначен у {name}");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (Description == null)
            {
                Debug.LogError($"Description не был назначен у {name}");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (Icon == null)
            {
                Debug.LogError($"Icon не был назначен у {name}");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            //if (AbilityLogics.Length == 0)
            //{
            //    Debug.LogError("Name не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //}
        }
    }
}
