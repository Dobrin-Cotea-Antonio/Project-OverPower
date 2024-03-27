using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData : MonoBehaviour {

    public Action<float, float> OnHpChange;
    public Action<float, float> OnManaChange;
    public Action<float> OnHpRegenChange;
    public Action<float> OnManaRegenChange;

    [SerializeField] float maxHp;
    [SerializeField] float maxMana;
    [SerializeField] float hpRegen;
    [SerializeField] float manaRegen;

    float hp;
    float mana;

    private void Awake() {
        hp = maxHp/2;
        mana = maxMana/2;
    }
    private void Start() {
        OnHpChange?.Invoke(hp, maxHp);
        OnManaChange?.Invoke(mana, maxMana);
        OnHpRegenChange?.Invoke(hpRegen);
        OnManaRegenChange?.Invoke(manaRegen);
    }

    private void Update() {
        RegenerateHp();
        RegenerateMana();
    }

    #region Regeneration
    void RegenerateHp() {
        if (hp == maxHp)
            return;
        hp = Mathf.Min(hp + hpRegen * Time.deltaTime, maxHp);
        OnHpChange?.Invoke(hp, maxHp);
    }

    void RegenerateMana() {
        if (mana == maxMana)
            return;
        mana = Mathf.Min(mana + manaRegen * Time.deltaTime, maxMana);
        OnManaChange?.Invoke(mana, maxMana);
    }
    #endregion

    #region Stat Modifications
    void ModifyHpRegen(float pValue) {
        hpRegen = Mathf.Max(hpRegen + pValue, 0);
        OnHpRegenChange?.Invoke(hpRegen);
    }

    void ModifyManaRegen(float pValue) {
        manaRegen = Mathf.Max(manaRegen + pValue, 0);
        OnManaRegenChange?.Invoke(manaRegen);
    }
    #endregion

}
