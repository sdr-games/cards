using System;

using UnityEngine;

namespace SDRGames.Whist.AnimationsModule.Models
{
    [Serializable]
    public class CharacterAnimationsModel
    {
        [field: SerializeField] public AnimationClip DeathAnimationClip { get; private set; }
        [field: SerializeField] public AnimationClip ImpactAnimationClip { get; private set; }
    }
}
