using System;
using System.Collections.Generic;

using SDRGames.Whist.CharacterModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public abstract class CharacterCombatManager : MonoBehaviour
    {
        protected Dictionary<int, PeriodicalEffect> PeriodicalHealthChanges;

        public abstract void Initialize();
        public abstract void TakePhysicalDamage(int damage);
        public abstract void TakeMagicalDamage(int damage);
        public abstract void TakeTrueDamage(int damage);
        public abstract void RestoreArmor(int restoration);
        public abstract void RestoreBarrier(int restoration);
        public abstract void RestoreHealth(int restoration);

        public void SetPeriodicalChanges(int valuePerRound, int roundsCount, Action changingAction)
        {
            if (PeriodicalHealthChanges.ContainsKey(valuePerRound))
            {
                PeriodicalHealthChanges[valuePerRound].IncreaseDuration(roundsCount);
                return;
            }
            PeriodicalHealthChanges.Add(valuePerRound, new PeriodicalEffect(roundsCount, changingAction));
        }

        protected virtual void OnEnable()
        {
            PeriodicalHealthChanges = new Dictionary<int, PeriodicalEffect>();
        }
    }
}
