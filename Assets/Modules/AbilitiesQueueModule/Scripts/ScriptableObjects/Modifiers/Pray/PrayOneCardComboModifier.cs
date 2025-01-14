using System;

using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    public class PrayOneCardComboModifier : AbilityModifierScriptableObject
    {
        [SerializeField] private int _damageReducePercent = 10;
        [SerializeField] private int _roundsCount = 2;
        [SerializeField] private Sprite _effectIcon;

        public override void Apply(CharacterCombatManager enemyCharacterCombatManager)
        {
            Action<int> action = (int percent) => { enemyCharacterCombatManager.GetParams().IncreasePhysicalDamage(-(enemyCharacterCombatManager.GetParams().PhysicalDamage * (percent / 100))); };
            enemyCharacterCombatManager.SetDebuff(_damageReducePercent, _roundsCount, _effectIcon, action, true);
        }

        private void OnEnable()
        {
            SelfUsable = false;
        }
    }
}
