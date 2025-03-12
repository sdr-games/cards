using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.EnemyBehaviorModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyBehaviorScriptableObject", menuName = "SDRGames/Combat/AI/Behavior")]
    public class BehaviorScriptableObject : ScriptableObject
    {
        [SerializeField][Range(0, 100)] private int _playerDefencePercentFrom = 100;
        [SerializeField][Range(0, 100)] private int _playerDefencePercentTo;
        [SerializeField] private SerializableDictionary<int, List<AbilityScriptableObject>> _abilitiesSets;

        public float MinimalCost { get; private set; }
        public List<AbilityScriptableObject> MinimalCostAttacks { get; private set; }

        public void Initialize()
        {
            MinimalCost = int.MaxValue;
            foreach (KeyValuePair<int, List<AbilityScriptableObject>> attack in _abilitiesSets)
            {
                int totalCost = attack.Value.Sum(x => x.Cost);
                if (totalCost < MinimalCost)
                {
                    MinimalCost = totalCost;
                    MinimalCostAttacks = attack.Value;
                }
            }
        }

        public bool CheckIfAppliableByDefence(float defenceCurrentValuePercent)
        {
            return _playerDefencePercentFrom >= defenceCurrentValuePercent && _playerDefencePercentTo <= defenceCurrentValuePercent;
        }

        public List<AbilityScriptableObject> ChooseRandomAttacks(float currentResourceValue)
        {
            List<AbilityScriptableObject> result = null;
            int offset = 0;
            Dictionary<List<AbilityScriptableObject>, int> abilitiesSets = _abilitiesSets.Where(set => set.Value.Sum(ability => ability.Cost) <= currentResourceValue).ToDictionary(set => set.Value, set => set.Key);
            int chance = Random.Range(0, abilitiesSets.Values.Sum());
            foreach (KeyValuePair<List<AbilityScriptableObject>, int> set in abilitiesSets)
            {
                if(chance > (set.Value + offset))
                {
                    offset += set.Value;
                    continue;
                }
                result = set.Key;
                continue;
            }
            if(result == null && MinimalCost < currentResourceValue)
            {
                result = MinimalCostAttacks;
            }
            return result;
        }

        public List<AbilityScriptableObject> GetAllAbilities()
        {
            List<AbilityScriptableObject> abilities = new List<AbilityScriptableObject>();
            foreach (List<AbilityScriptableObject> attacks in _abilitiesSets.Values)
            {
                foreach(AbilityScriptableObject attack in attacks)
                {
                    if (abilities.Contains(attack))
                    {
                        continue;
                    }
                    abilities.Add(attack);
                }
            }
            return abilities;
        }

        private void OnValidate()
        {
            if(_playerDefencePercentFrom < 0)
            {
                _playerDefencePercentFrom = 0;
            }
            if(_playerDefencePercentTo > 100)
            {
                _playerDefencePercentTo = 100;
            }

            if (_playerDefencePercentFrom < _playerDefencePercentTo)
            {
                _playerDefencePercentTo = _playerDefencePercentFrom;
            }
        }
    }
}
