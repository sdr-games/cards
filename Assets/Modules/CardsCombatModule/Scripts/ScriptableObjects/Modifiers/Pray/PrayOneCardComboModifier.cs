using System;
using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    public class PrayOneCardComboModifier : CardModifierScriptableObject
    {
        [Header("Pray in combo with one card decrease enemy's damage by 10% for 2 turns")][SerializeField] private int _damageReducePercent = 10;
        [SerializeField] private int _roundsCount = 2;
        [SerializeField] private Sprite _effectIcon;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Card> affectedCards)
        {
            foreach (CharacterCombatManager targetCombatManager in targetCombatManagers)
            {
                Action<int> action = (int percent) => { targetCombatManager.GetParams().IncreasePhysicalDamage(-(targetCombatManager.GetParams().PhysicalDamage * (percent / 100))); };
                targetCombatManager.SetDebuff(_damageReducePercent, _roundsCount, _effectIcon, "", action, true);
            }
        }
    }
}
