using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.PointsModule.Models;

namespace SDRGames.Whist.CharacterModule.Models
{
    public class PlayerParamsModel : CharacterParamsModel
    {
        public PlayerParamsModel(PlayerParamsScriptableObject playerParamsScriptableObject) : base(playerParamsScriptableObject)
        {
            PatientHealthPoints = playerParamsScriptableObject.PatientHealthPoints;
        }

        public Points PatientHealthPoints { get; private set; }
        public int Experience { get; private set; }
        public int TalentPoints { get; private set; }

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
            PatientHealthPoints.DecreaseCurrentValue(damage);
        }
    }
}
