using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttackController : MonoBehaviour
{
    [SerializeField] private MeleeAbility[] _meleeAbilities;
    [SerializeField] private int _maxAbilitiesCount;

    private ObjectSelectController[] _playerSelectableParts;
    public List<MeleeAbility> SelectedAbilities { get; private set; }
    public List<ObjectSelectController> SelectedParts { get; private set; }

    public void Initialize(ObjectSelectController[] playerSelectableParts)
    {
        _playerSelectableParts = playerSelectableParts;
    }

    public void Attack()
    {
        SelectAttackingAbilities();
        SelectPlayerParts();
    }

    private void SelectAttackingAbilities()
    {

        SelectedAbilities = new List<MeleeAbility>();
        for (int i = 0; i < _maxAbilitiesCount; i++)
        {
            int index = Random.Range(0, _meleeAbilities.Length - 1);
            SelectedAbilities.Add(_meleeAbilities[index]);
        }
    }

    private void SelectPlayerParts()
    {
        SelectedParts = new List<ObjectSelectController>();
        for (int i = 0; i < SelectedAbilities.Count; i++)
        {
            int index = Random.Range(0, 100) > 60 ? Random.Range(0, 100) > 90 ? 2 : 1 : 0;
            SelectedParts.Add(_playerSelectableParts[index]);
        }
    }
}
