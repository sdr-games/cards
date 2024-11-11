using System;

using UnityEngine;

namespace SDRGames.Whist.SettingsModule.Models
{
    [Serializable]
    public class Scaling
    {
        public static Scaling Instance { get; private set; }

        [field: SerializeField] public int OnePointStrengthToHealthPoints { get; private set; }
        [field: SerializeField] public int OnePointStrengthToPhysicalDamage { get; private set; }
        [field: SerializeField] public int OnePointStrengthToHitChance { get; private set; }
        [field: SerializeField] public int OnePointAgilityToStaminaRestorationPerRound { get; private set; }
        [field: SerializeField] public int OnePointAgilityToInitiative { get; private set; }
        [field: SerializeField] public int OnePointAgilityToDodge { get; private set; }
        [field: SerializeField] public int OnePointStaminaToPhysicalDamageAbsorb { get; private set; }
        [field: SerializeField] public int OnePointStaminaToMagicalDamageAbsorb { get; private set; }
        [field: SerializeField] public int OnePointStaminaToHealthPoints { get; private set; }
        [field: SerializeField] public int OnePointStaminaToStaminaPoints { get; private set; }
        [field: SerializeField] public int OnePointIntelligenceToMagicalDamage { get; private set; }
        [field: SerializeField] public int OnePointIntelligenceToBreathPoints { get; private set; }
        [field: SerializeField] public int OneLevelToArmorPoints { get; private set; }
        [field: SerializeField] public int OneLevelToBarrierPoints { get; private set; }

        public void UpdateStaticFields()
        {
            if(Instance == null)
            {
                Instance = new Scaling();
            }

            Instance.OnePointStrengthToHealthPoints = OnePointStrengthToHealthPoints;
            Instance.OnePointStrengthToPhysicalDamage = OnePointStrengthToPhysicalDamage;
            Instance.OnePointStrengthToHitChance = OnePointStrengthToHitChance;

            Instance.OnePointAgilityToStaminaRestorationPerRound = OnePointAgilityToStaminaRestorationPerRound;
            Instance.OnePointAgilityToInitiative = OnePointAgilityToInitiative;
            Instance.OnePointAgilityToDodge = OnePointAgilityToDodge;

            Instance.OnePointStaminaToPhysicalDamageAbsorb = OnePointStaminaToPhysicalDamageAbsorb;
            Instance.OnePointStaminaToMagicalDamageAbsorb = OnePointStaminaToMagicalDamageAbsorb;
            Instance.OnePointStaminaToHealthPoints = OnePointStaminaToHealthPoints;
            Instance.OnePointStaminaToStaminaPoints = OnePointStaminaToStaminaPoints;

            Instance.OnePointIntelligenceToMagicalDamage = OnePointIntelligenceToMagicalDamage;
            Instance.OnePointIntelligenceToBreathPoints = OnePointIntelligenceToBreathPoints;

            Instance.OneLevelToArmorPoints = OneLevelToArmorPoints;
            Instance.OneLevelToBarrierPoints = OneLevelToBarrierPoints;
        }
    }
}
