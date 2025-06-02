using System.Collections.Generic;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.SoundModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class Ability
    {
        public LocalizedString Name { get; private set; }
        public LocalizedString ShortDescription { get; private set; }
        public Sprite Icon { get; private set; }
        public int Cost { get; private set; }
        public AnimationClip AnimationClip { get; private set; }
        public List<AbilityLogic> AbilityLogics { get; protected set; }
        public SoundClipScriptableObject SoundClip { get; private set; }

        public Ability(AbilityScriptableObject abilityScriptableObject)
        {
            Name = abilityScriptableObject.Name;
            ShortDescription = abilityScriptableObject.ShortDescription;
            Icon = abilityScriptableObject.Icon;
            Cost = abilityScriptableObject.Cost;
            AnimationClip = abilityScriptableObject.AnimationClip;
            SoundClip = abilityScriptableObject.SoundClip;
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
                logic.Apply(casterCombatManager, targetCombatManager);
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
                    logic.Apply(casterCombatManager, targetCombatManager);
                }
            }
        }

        public string GetShortDescription()
        {
            return ShortDescription.GetLocalizedText();
        }

        public virtual string GetLocalizedDescription(CharacterParamsModel characterParamsModel)
        {
            List<string> localizedDescription = new List<string>();
            foreach (AbilityLogic abilityLogic in AbilityLogics)
            {
                localizedDescription.Add(abilityLogic.GetLocalizedDescription(characterParamsModel));
            }
            return string.Join(". ", localizedDescription);
        }
    }
}
