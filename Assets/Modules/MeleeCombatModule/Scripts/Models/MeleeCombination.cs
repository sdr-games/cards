using System;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;

namespace SDRGames.Whist.MeleeCombatModule.Models
{
    public class MeleeCombination : Ability
    {
        public AbilitySequence[] AttackSequence { get; private set; }
        public int EuristicPoints { get; private set; }

        public MeleeCombination(MeleeCombinationScriptableObject meleeCombinationScriptableObject) : base(meleeCombinationScriptableObject)
        {
            AttackSequence = meleeCombinationScriptableObject.AttackSequence;
            EuristicPoints = meleeCombinationScriptableObject.EuristicPoints;
        }

        public bool Match(List<MeleeAttack> meleeAttacks)
        {
            foreach(AbilitySequence abilitySequence in AttackSequence)
            {
                bool match = false;
                List<Guid> guids = abilitySequence.GetAbilitiesGuids();
                for (int i = 0; i < guids.Count; i++)
                {
                    if (meleeAttacks[i] == null || guids[i] != meleeAttacks[i].Guid)
                    {
                        break;
                    }
                    match = true;
                }
                if(match)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
