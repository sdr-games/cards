using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class TurnsManager : MonoBehaviour
{
    [SerializeField] private WeaponSelectController _weaponSelectController;
    [SerializeField] private AIMeleeAttackController _aiMeleeAttackController;

    [SerializeField] private MagicSelectController _magicSelectController;

    [SerializeField] private DefenceController _playerDefenceController;
    [SerializeField] private AIDefenceController _aiDefenceController;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _enemyAnimator;

    [SerializeField] private TextMeshProUGUI _enemyDamageText;
    [SerializeField] private TextMeshProUGUI _playerDamageText;
    [SerializeField] private TextMeshProUGUI _currentTurnText;
    [SerializeField] private TextMeshProUGUI _announcementText;

    [SerializeField] private List<ObjectSelectController> _defencePartsApplied;

    private int _currentTurn;

    private void Start()
    {
        _defencePartsApplied = new List<ObjectSelectController>();

        _weaponSelectController.AttackApplied += OnPlayerAttackApplied;
        _weaponSelectController.Initialize();

        _magicSelectController.Initialize();

        _playerDefenceController.Initialize();
        _playerDefenceController.PlayerDefenceApplied += OnPlayerDefenceApplied;

        _aiDefenceController.Initialize();

        _aiMeleeAttackController.Initialize(_playerDefenceController.ObjectSelectableParts);

        int chance = UnityEngine.Random.Range(0, 100);
        if (chance > 50)
        {
            _currentTurn = 0;
            StartPlayerTurn();
        }
        else
        {
            _currentTurn = 1;
            StartAITurn();
        }
    }

    private void SwitchTurn()
    {
        if(_currentTurn == 0)
        {
            _currentTurn = 1;
            StartAITurn();
        }
        else
        {
            _currentTurn = 0;
            StartPlayerTurn();
        }
    }

    private void StartPlayerTurn()
    {
        _weaponSelectController.enabled = true;
        _magicSelectController.enabled = true;
        _currentTurnText.text = "Атакуйте!";
        _announcementText.text = "Выберите тип атаки";
    }

    private void StartAITurn()
    {
        _announcementText.text = "";
        _currentTurnText.text = "Защищайтесь!";
        _aiMeleeAttackController.Attack();
        _playerDefenceController.SetSelectedParts(_aiMeleeAttackController.SelectedParts);
        _announcementText.text = "Выберите до трех направлений для защиты";
        _playerDefenceController.StartDefence();
    }


    private void OnPlayerAttackApplied(object sender, List<MeleeAbility> meleeAbilities)
    {
        List<ObjectSelectController> selectedParts = _aiDefenceController.ObjectSelectedParts;
        if(meleeAbilities.Count == 0)
        {
            _announcementText.text = "Выберите до трех атакующих умений";
            return;
        }

        if(selectedParts.Count == 0)
        {
            _announcementText.text = $"Выберите {meleeAbilities.Count} направлений для нападения";
            _weaponSelectController.enabled = false;
            _aiDefenceController.EnableParts();
            return;
        }

        if(meleeAbilities.Count > selectedParts.Count)
        {
            _announcementText.text = $"Выберите еще {meleeAbilities.Count - selectedParts.Count} направлений для нападаения";
            return;
        }

        if (meleeAbilities.Count < selectedParts.Count)
        {
            _announcementText.text = $"Отмените выбор {selectedParts.Count - meleeAbilities.Count} направлений для нападения";
            return;
        }

        _announcementText.text = "";
        _defencePartsApplied = new List<ObjectSelectController>(_aiDefenceController.Defence(meleeAbilities.Count));
        StartCoroutine(EndAttackTurn(new List<MeleeAbility>(meleeAbilities), _aiDefenceController, _playerAnimator, _enemyAnimator, _enemyDamageText));
        _weaponSelectController.Deselect();
    }

    private void OnPlayerDefenceApplied(object sender, ObjectSelectController[] selectedParts)
    {
        _announcementText.text = "";
        _defencePartsApplied = new List<ObjectSelectController>(selectedParts);
        StartCoroutine(EndAttackTurn(new List<MeleeAbility>(_aiMeleeAttackController.SelectedAbilities), _playerDefenceController, _enemyAnimator, _playerAnimator, _playerDamageText));
    }

    private IEnumerator EndAttackTurn(List<MeleeAbility> meleeAbilities, DefenceController defenceController, Animator attackingAnimator, Animator attackedAnimator, TextMeshProUGUI damageText)
    {
        List<ObjectSelectController> selectedPartsForAttack = new List<ObjectSelectController>(defenceController.ObjectSelectedParts);
        for (int i = 0; i < meleeAbilities.Count; i++)
        {
            MeleeAbility attackingAbility = meleeAbilities[i];
            if (attackingAbility.Name == "Skip")
            {
                continue;
            }
            attackingAnimator.Play(attackingAbility.AnimationClip.name);
            yield return new WaitForSeconds(attackingAbility.AnimationClip.length * 0.2f);

            ObjectSelectController selectedPartForDefence = _defencePartsApplied[i];
            if(selectedPartForDefence != null && selectedPartsForAttack[i].Equals(selectedPartForDefence))
            {
                Debug.Log($"{selectedPartForDefence.name} vs {selectedPartsForAttack[i].name}");
                damageText.text = "Заблокировано";
                yield return new WaitForSeconds(attackingAbility.AnimationClip.length);
                damageText.text = "";
                continue;
            }

            if(selectedPartForDefence != null && selectedPartsForAttack[i].CheckHitSuccess())
            {
                damageText.text = "Промах";
                yield return new WaitForSeconds(attackingAbility.AnimationClip.length);
                damageText.text = "";
                continue;
            }

            int damage = (int)Math.Round(attackingAbility.GetDamage() * selectedPartsForAttack[i].DamageMultiplier);
            damageText.text = $"-{damage}";
            defenceController.TakeDamage(damage);
            yield return new WaitForSeconds(attackingAbility.AnimationClip.length);
            damageText.text = "";

            if(defenceController.IsDead)
            {
                damageText.text = "";
                _currentTurnText.text = _currentTurn == 0 ? "Победа!" : "Поражение!";
                defenceController.ClearSelectedParts();
                yield break;
            }

            //attackedAnimator.Play(selectedPart.AnimationClip.name);
            //yield return new WaitForSeconds(selectedPart.AnimationClip.length);
        }
        damageText.text = "";
        defenceController.ClearSelectedParts();
        SwitchTurn();
    }
}
