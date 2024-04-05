using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityManager : MonoBehaviour {
    public Action<float, float, int> OnAbilityCooldownDecrease;
    public Action<float, float, int> OnAbilityUseTimeChange;

    [SerializeField] List<AbilityBase> abilities;

    #region Events
    void OnEnable() {
        foreach (AbilityBase ability in abilities) {
            ability.OnCooldownChange += AbilityCooldownDecrease;
            ability.OnUseTimeChange += AbilityUseTimeDecrease;
        }
    }

    void OnDisable() {
        foreach (AbilityBase ability in abilities) {
            ability.OnCooldownChange -= AbilityCooldownDecrease;
            ability.OnUseTimeChange -= AbilityUseTimeDecrease;
        }
    }
    #endregion

    #region Ability Events
    void AbilityCooldownDecrease(float pCooldownLeft, float pAbilityCooldown, AbilityBase pAbility) {
        OnAbilityCooldownDecrease?.Invoke(pCooldownLeft, pAbilityCooldown, abilities.IndexOf(pAbility));
    }

    void AbilityUseTimeDecrease(float pUseTimeLeft, float pAbilityDuration, AbilityBase pAbility) {
        OnAbilityUseTimeChange?.Invoke(pUseTimeLeft, pAbilityDuration, abilities.IndexOf(pAbility));
    }
    #endregion

    #region InputEvents
    public void OnAbility1Press() {
        abilities[0].UseAbility();
    }

    public void OnAbility2Press() {
        abilities[1].UseAbility();
    }

    public void OnAbility3Press() {
        abilities[2].UseAbility();
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
}
