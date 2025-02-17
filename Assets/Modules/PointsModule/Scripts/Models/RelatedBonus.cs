using System;

using UnityEngine;

namespace SDRGames.Whist.PointsModule.Models
{
    [Serializable]
    public class RelatedBonus
    {
        public enum RelatedParameters { Strength, Dexterity, Stamina, Constitution, Intelligence, Tenacity, Spirit, Charisma, Immunity };
        [field: SerializeField] public RelatedParameters RelatedParameter { get; private set; }
        [field: SerializeField] public float ValuePerRelatedParameterPoint { get; private set; }
        [field: SerializeField] public int RelatedParameterPoints { get; private set; }

        public float GetBonus()
        {
            return ValuePerRelatedParameterPoint * RelatedParameterPoints;
        }
    }
}
