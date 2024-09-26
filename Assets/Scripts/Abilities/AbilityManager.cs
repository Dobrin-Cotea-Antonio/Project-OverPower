using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class AbilityUpgradeData {
    public AbilityBase baseAbility;
    public AbilityBase evolutionAbility;
    public int evolutionCost;
}

[RequireComponent(typeof(PlayerData))]
public class AbilityManager : MonoBehaviour {
    public Action<float, float, float, float, int, int, int> OnAbilityCooldownDecrease;
    public Action<float, float, int> OnAbilityUseTimeChange;
    //public Action<int, int, int> OnAbilityLevelUp;
    public Action<int, int, int> OnChargeChange;
    public Action<int, AbilityBase> OnAbilityEvolve;

    [SerializeField] private List<AbilityUpgradeData> abilityUpgradeData;

    private List<AbilityBase> activeAbilities = new List<AbilityBase>();
    private PlayerData playerData;

    #region Events
    private void Awake() {
        playerData = GetComponent<PlayerData>();

        foreach (AbilityUpgradeData data in abilityUpgradeData)
            activeAbilities.Add(data.baseAbility);
    }

    void OnEnable() {
        foreach (AbilityBase ability in activeAbilities) {
            ability.OnCooldownChange += AbilityCooldownDecrease;
            ability.OnUseTimeChange += AbilityUseTimeDecrease;
            ability.OnAbilityLevelUp += AbilityLevelUp;
            ability.OnChargeChange += ChargeChange;
        }
    }

    void OnDisable() {
        foreach (AbilityBase ability in activeAbilities) {
            ability.OnCooldownChange -= AbilityCooldownDecrease;
            ability.OnUseTimeChange -= AbilityUseTimeDecrease;
            ability.OnAbilityLevelUp -= AbilityLevelUp;
            ability.OnChargeChange -= ChargeChange;
        }
    }
    #endregion

    #region Ability Events
    void AbilityCooldownDecrease(float pCooldownLeft, float pAbilityCooldown, float pAbilityChargeCooldownLeft, float pAbiliyChargeCooldownMax, int pCharges, int pMaxCharges, AbilityBase pAbility) {
        OnAbilityCooldownDecrease?.Invoke(pCooldownLeft, pAbilityCooldown, pAbilityChargeCooldownLeft, pAbiliyChargeCooldownMax, pCharges, pMaxCharges, activeAbilities.IndexOf(pAbility));
    }

    void AbilityUseTimeDecrease(float pUseTimeLeft, float pAbilityDuration, AbilityBase pAbility) {
        OnAbilityUseTimeChange?.Invoke(pUseTimeLeft, pAbilityDuration, activeAbilities.IndexOf(pAbility));
    }

    void AbilityLevelUp(AbilityBase pAbility, int pLevel, int pMaxLevel) {
        //OnAbilityLevelUp?.Invoke(pLevel, pMaxLevel, activeAbilities.IndexOf(pAbility));
    }

    void ChargeChange(int pCharges, int pMaxCharges, AbilityBase pAbility) {
        OnChargeChange?.Invoke(pCharges, pMaxCharges, activeAbilities.IndexOf(pAbility));
    }
    #endregion

    #region InputEvents
    public void OnAbility1Press() {
        if (playerData.isStunned)
            return;
        activeAbilities[0].UseAbility();
    }

    public void OnAbility2Press() {
        if (playerData.isStunned)
            return;
        activeAbilities[1].UseAbility();
    }

    public void OnAbility3Press() {
        if (playerData.isStunned)
            return;
        activeAbilities[2].UseAbility();
    }

    public void OnAbility4Press() {
        if (playerData.isStunned)
            return;
        activeAbilities[3].UseAbility();
    }
    #endregion

    #region Helper Methods
    public AbilityBase ReturnAbilityByIndex(int pIndex) {
        return activeAbilities[pIndex];
    }

    public Sprite ReturnAbilityIconByIndex(int pIndex) {
        return activeAbilities[pIndex].abilityData.iconImage;
    }
    #endregion

    private void Update() {
    }

    #region AbilityEvolution
    public void EvolveAbility(int pAbilityIndex) {

        if (playerData.gold < abilityUpgradeData[pAbilityIndex].evolutionCost)
            return;

        playerData.ModifyGold(-abilityUpgradeData[pAbilityIndex].evolutionCost);

        activeAbilities[pAbilityIndex] = abilityUpgradeData[pAbilityIndex].evolutionAbility;

        OnAbilityEvolve?.Invoke(pAbilityIndex, activeAbilities[pAbilityIndex]);
    }
    #endregion
}
