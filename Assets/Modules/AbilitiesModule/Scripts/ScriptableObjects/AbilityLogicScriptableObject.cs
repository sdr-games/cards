using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    public abstract class AbilityLogicScriptableObject : ScriptableObject
    {
        [field: SerializeField] public LocalizedString Description { get; protected set; }
        [field: SerializeField] public Sprite EffectIcon { get; protected set; }
        [field: SerializeField] public int TargetsCount { get; protected set; } = 1;
        [field: SerializeField][field: Range(0, 100)] public int Chance { get; protected set; } = 100;
        [field: SerializeField] public int RoundsCount { get; protected set; } = 0;

        public bool SelfUsable { get; protected set; }
    }
}
