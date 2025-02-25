using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyBehaviorScriptableObject", menuName = "SDRGames/Combat/AI/Behavior")]
    public class BehaviorScriptableObject : ScriptableObject
    {
        [SerializeField][Range(0, 100)] private int _playerArmorPercentFrom = 100;
        [SerializeField][Range(0, 100)] private int _playerArmorPercentTo;
        [SerializeField] private SerializableDictionary<int, List<AbilityScriptableObject>> _meleeAttacks;

        public float MinimalAttacksCost { get; private set; }
        public List<AbilityScriptableObject> MinimalCostAttacks { get; private set; }

        public bool CheckIfAppliableByArmor(float paramValue)
        {
            return _playerArmorPercentTo >= paramValue || _playerArmorPercentFrom <= paramValue;
        }

        public bool CheckIfAppliableByResource(float currentResourceAmount)
        {
            foreach (KeyValuePair<int, List<AbilityScriptableObject>> attack in _meleeAttacks)
            {
                int totalCost = attack.Value.Sum(x => x.Cost);
                if (totalCost < currentResourceAmount)
                {
                    return true;
                }
            }
            return false;
        }

        public List<AbilityScriptableObject> ChooseRandomAttacks()
        {
            List<AbilityScriptableObject> result = new List<AbilityScriptableObject>();
            int chance = Random.Range(0, 100);
            int offset = 0;
            foreach(KeyValuePair<int, List<AbilityScriptableObject>> attack in _meleeAttacks)
            {
                if(chance > (attack.Key + offset))
                {
                    offset += attack.Key;
                    continue;
                }
                result = attack.Value;
            }
            return result;
        }

        public List<AbilityScriptableObject> GetAllAttacks()
        {
            List<AbilityScriptableObject> abilities = new List<AbilityScriptableObject>();
            foreach (List<AbilityScriptableObject> attacks in _meleeAttacks.Values)
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

        private void OnEnable()
        {
            MinimalAttacksCost = 0;
            foreach (KeyValuePair<int, List<AbilityScriptableObject>> attack in _meleeAttacks)
            {
                int totalCost = attack.Value.Sum(x => x.Cost);
                if (totalCost < MinimalAttacksCost)
                {
                    MinimalAttacksCost = totalCost;
                    MinimalCostAttacks = attack.Value;
                }
            }
        }


        private void OnValidate()
        {
            if(_playerArmorPercentFrom < 0)
            {
                _playerArmorPercentFrom = 0;
            }
            if(_playerArmorPercentTo > 100)
            {
                _playerArmorPercentTo = 100;
            }

            if (_playerArmorPercentFrom < _playerArmorPercentTo)
            {
                _playerArmorPercentTo = _playerArmorPercentFrom;
            }
        }
    }
}
