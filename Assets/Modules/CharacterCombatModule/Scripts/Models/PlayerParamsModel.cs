using SDRGames.Whist.CharacterCombatModule.ScriptableObjects;
using SDRGames.Whist.PointsModule.Models;

namespace SDRGames.Whist.CharacterCombatModule.Models
{
    public class PlayerParamsModel : CharacterParamsModel
    {
        public Points PatientHealthPoints { get; private set; }
        public int Experience { get; private set; }
        public int TalentPoints { get; private set; }
        public float PatientDamageBlockPercent { get; private set; }
        public float SacrificePercent { get; private set; }
        public float ConvertingPercent { get; private set; }

        public PlayerParamsModel(PlayerParamsScriptableObject playerParamsScriptableObject) : base(playerParamsScriptableObject)
        {
            PatientHealthPoints = playerParamsScriptableObject.PatientHealthPoints;
            Experience = playerParamsScriptableObject.Experience;
            TalentPoints = playerParamsScriptableObject.TalentPoints;
            PatientDamageBlockPercent = 0;
            SacrificePercent = 0;
            ConvertingPercent = 0;
        }

        public override void TakePhysicalDamage(int damage, bool isCritical)
        {
            if (PatientHealthPoints.CurrentValue <= 0)
            {
                damage = (int)(damage * 1.5f);
            }
            base.TakePhysicalDamage(damage, isCritical);
        }

        public override void TakeMagicalDamage(int damage, bool isCritical)
        {
            if (PatientHealthPoints.CurrentValue <= 0)
            {
                damage = (int)(damage * 1.5f);
            }
            base.TakeMagicalDamage(damage, isCritical);
        }

        public override void TakeTrueDamage(int damage, bool isCritical = false)
        {
            if (PatientHealthPoints.CurrentValue <= 0)
            {
                damage = (int)(damage * 1.5f);
            }
            base.TakeTrueDamage(damage);
        }

        public void TakePatientDamage(int damage)
        {
            if (SacrificePercent > 0)
            {
                int sacrificeDamage = (int)(SacrificePercent / 100 * damage);
                TakeTrueDamage(sacrificeDamage, false);
                damage -= sacrificeDamage;
            }

            if(ConvertingPercent > 0)
            {
                int convertingAmount = (int)(ConvertingPercent / 100 * damage);
                if(ArmorPoints.CurrentValueInPercents < BarrierPoints.CurrentValueInPercents)
                {
                    RestoreArmor(convertingAmount);
                }
                else
                {
                    RestoreBarrier(convertingAmount);
                }
                damage -= convertingAmount;
            }
            PatientHealthPoints.DecreaseCurrentValue(damage);
        }

        public void RestorePatientHealth(float restoration)
        {
            PatientHealthPoints.IncreaseCurrentValue(restoration);
        }

        public void SetPatientDamageBlockPercent(float percent)
        {
            PatientDamageBlockPercent = percent;
        }

        public void SetSacrificePercent(float percent)
        {
            SacrificePercent = percent;
        }

        public void SetConvertingPercent(float percent)
        {
            ConvertingPercent = percent;
        }
    }
}
