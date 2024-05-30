using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityManager : MonoBehaviour {
    public Action<float, float,float,float,int,int, int> OnAbilityCooldownDecrease;
    public Action<float, float, int> OnAbilityUseTimeChange;
    public Action<int, int, int> OnAbilityLevelUp;
    public Action<int, int, int> OnChargeChange;

    [SerializeField] PlayerData data;
    [SerializeField] List<AbilityBase> abilities;

    #region Events
    void OnEnable() {
        foreach (AbilityBase ability in abilities) {
            ability.OnCooldownChange += AbilityCooldownDecrease;
            ability.OnUseTimeChange += AbilityUseTimeDecrease;
            ability.OnAbilityLevelUp += AbilityLevelUp;
            ability.OnChargeChange += ChargeChange;
        }
    }

    void OnDisable() {
        foreach (AbilityBase ability in abilities) {
            ability.OnCooldownChange -= AbilityCooldownDecrease;
            ability.OnUseTimeChange -= AbilityUseTimeDecrease;
            ability.OnAbilityLevelUp -= AbilityLevelUp;
            ability.OnChargeChange -= ChargeChange;
        }
    }
    #endregion

    #region Ability Events
    void AbilityCooldownDecrease(float pCooldownLeft, float pAbilityCooldown,float pAbilityChargeCooldownLeft,float pAbiliyChargeCooldownMax,int pCharges,int pMaxCharges, AbilityBase pAbility) {
        OnAbilityCooldownDecrease?.Invoke(pCooldownLeft, pAbilityCooldown,pAbilityChargeCooldownLeft,pAbiliyChargeCooldownMax,pCharges,pMaxCharges, abilities.IndexOf(pAbility));
    }

    void AbilityUseTimeDecrease(float pUseTimeLeft, float pAbilityDuration, AbilityBase pAbility) {
        OnAbilityUseTimeChange?.Invoke(pUseTimeLeft, pAbilityDuration, abilities.IndexOf(pAbility));
    }

    void AbilityLevelUp(AbilityBase pAbility, int pLevel, int pMaxLevel) {
        OnAbilityLevelUp?.Invoke(pLevel, pMaxLevel, abilities.IndexOf(pAbility));
    }

    void ChargeChange(int pCharges, int pMaxCharges, AbilityBase pAbility) {
        OnChargeChange?.Invoke(pCharges, pMaxCharges, abilities.IndexOf(pAbility));
    }
    #endregion

    #region InputEvents
    public void OnAbility1Press() {
        if (data.isStunned)
            return;
        abilities[0].UseAbility();
    }

    public void OnAbility2Press() {
        if (data.isStunned)
            return;
        abilities[1].UseAbility();
    }

    public void OnAbility3Press() {
        if (data.isStunned)
            return;
        abilities[2].UseAbility();
    }

    public void OnAbility4Press() {
        if (data.isStunned)
            return;
        abilities[3].UseAbility();
    }
    #endregion

    #region Helper Methods
    public AbilityBase ReturnAbilityByIndex(int pIndex) {
        return abilities[pIndex];
    }

    public Sprite ReturnAbilityIconByIndex(int pIndex) {
        return abilities[pIndex].iconImage;
    }
    #endregion

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A))
            abilities[0].LevelUp();
        if (Input.GetKeyDown(KeyCode.S))
            abilities[1].LevelUp();
        if (Input.GetKeyDown(KeyCode.D))
            abilities[2].LevelUp();
    }
}
