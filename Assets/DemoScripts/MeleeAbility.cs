using System;

using UnityEngine;

[Serializable]
public class MeleeAbility
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int MinimalDamage { get; private set; }
    [field: SerializeField] public int MaximumDamage { get; private set; }
    [field: SerializeField] public AnimationClip AnimationClip { get; private set; }

    public int GetDamage()
    {
        return UnityEngine.Random.Range(MinimalDamage, MaximumDamage);
    }
}
