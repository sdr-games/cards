using System.Collections;
using System.Collections.Generic;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class Ability
    {
        public LocalizedString Name { get; private set; }
        public LocalizedString Description { get; private set; }
        public Sprite Icon { get; private set; }
        public int Cost { get; private set; }
        public List<AbilityLogic> AbilityLogics { get; protected set; }

        public Ability(AbilityScriptableObject abilityScriptableObject)
        {
            Name = abilityScriptableObject.Name;
            Description = abilityScriptableObject.Description;
            Icon = abilityScriptableObject.Icon;
            Cost = abilityScriptableObject.Cost;
            AbilityLogics = new List<AbilityLogic>();
            foreach (AbilityLogicScriptableObject abilityLogicScriptableObject in abilityScriptableObject.AbilityLogics)
            {
                switch (abilityLogicScriptableObject)
                {
                    case BuffLogicScriptableObject buffLogicScriptableObject:
                        BuffLogic buffLogic = new BuffLogic(buffLogicScriptableObject);
                        AbilityLogics.Add(buffLogic);
                        break;
                    case DamageLogicScriptableObject damageLogicScriptableObject:
                        DamageLogic damageLogic = new DamageLogic(damageLogicScriptableObject);
                        AbilityLogics.Add(damageLogic);
                        break;
                    case RestorationLogicScriptableObject restorationLogicScriptableObject:
                        RestorationLogic restorationLogic = new RestorationLogic(restorationLogicScriptableObject);
                        AbilityLogics.Add(restorationLogic);
                        break;
                    default:
                        break;
                }
            }
        }

        public void ApplyLogics(CharacterCombatManager casterCombatManager, CharacterCombatManager targetCombatManager)
        {
            foreach (AbilityLogic logic in AbilityLogics)
            {
                if (logic.SelfUsable)
                {
                    logic.Apply(casterCombatManager);
                    continue;
                }
                logic.Apply(targetCombatManager);
            }
        }

        public void ApplyLogics(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetsCombatManager)
        {
            foreach (AbilityLogic logic in AbilityLogics)
            {
                if (logic.SelfUsable)
                {
                    logic.Apply(casterCombatManager);
                    continue;
                }
                foreach (CharacterCombatManager targetCombatManager in targetsCombatManager)
                {
                    logic.Apply(targetCombatManager);
                }
            }
        }
    }
}
