using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData : MonoBehaviour, IDamagable, IStatusEffectReceiver {

    public Action<float, float> OnHpChange;
    public Action<float, float> OnManaChange;

    public Action<float> OnHpRegenChange;
    public Action<float> OnManaRegenChange;

    public Action<StatusEffect> OnStatusCreate;
    public Action<StatusEffect> OnStatusRemove;

    public Action<float> OnGoldChange;

    [Header("Data")]
    [SerializeField] private UIManager _UIManager;
    public UIManager UIManager { get { return _UIManager; } }

    [Header("Stats")]
    [SerializeField] private float maxHp;
    [SerializeField] private float maxMana;
    [SerializeField] private float hpRegen;
    [SerializeField] private float manaRegen;
    [SerializeField] private float baseSpeed;

    [Header("Team")]
    [SerializeField] private Team _team;
    [SerializeField] private bool _isDummy = false;

    [Header("")]
    [SerializeField] private float _gold;


    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    public Team team { get { return _team; } }
    public bool isDummy { get { return _isDummy; } }

    public float hp { get; private set; }
    public float mana { get; private set; }
    public float speed { get; private set; }
    public bool isStunned { get; set; }

    public float gold { get; private set; }

    private void Awake() {
        hp = maxHp / 2;
        mana = maxMana / 2;
        speed = baseSpeed;
    }
    private void Start() {
        OnHpChange?.Invoke(hp, maxHp);
        OnManaChange?.Invoke(mana, maxMana);
        OnHpRegenChange?.Invoke(hpRegen);
        OnManaRegenChange?.Invoke(manaRegen);
        OnGoldChange?.Invoke(gold);
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
    public void ModifyHpRegen(float pValue) {
        hpRegen = Mathf.Max(hpRegen + pValue, 0);
        OnHpRegenChange?.Invoke(hpRegen);
    }

    public void ModifyManaRegen(float pValue) {
        manaRegen = Mathf.Max(manaRegen + pValue, 0);
        OnManaRegenChange?.Invoke(manaRegen);
    }

    public void ModifyHp(float pValue) {
        hp += pValue;
        hp = Mathf.Min(hp, maxHp);
        hp = Math.Max(hp, 0);
        OnHpChange?.Invoke(hp, maxHp);
    }

    public void ModifySpeedByPercentage(float pValue) {
        speed = Mathf.Max(0, speed + (pValue / 100f) * baseSpeed);
    }
    #endregion

    #region HP
    public void TakeDamage(float pDamage) {
        hp = Mathf.Max(hp - pDamage, 0);
        OnHpChange?.Invoke(hp, maxHp);
    }
    #endregion

    #region Status Effects
    public void ApplyStatusEffect(GameObject pStatusEffectPrefab) {

        StatusEffect status = Instantiate(pStatusEffectPrefab, transform).GetComponent<StatusEffect>();

        if (status == null)
            return;

        status.SetTarget(gameObject);
        status.OnStatusDestroy += RemoveEffectFromList;

        bool wasStatusFound = false;
        StatusEffect identicalStatus = null;

        foreach (StatusEffect effect in statusEffects) {
            if (effect.GetType() == status.GetType()) {
                wasStatusFound = true;
                identicalStatus = effect;
                break;
            }
        }

        if (!wasStatusFound) {
            statusEffects.Add(status);
            status.StackEffect();
            OnStatusCreate?.Invoke(status);

        } else {
            identicalStatus.StackEffect();
            Destroy(status.gameObject);
        }
    }

    private void RemoveEffectFromList(StatusEffect pStatus) {
        pStatus.OnStatusDestroy -= RemoveEffectFromList;
        OnStatusRemove?.Invoke(pStatus);
        statusEffects.Remove(pStatus);
    }

    public bool CheckListContainsEffectType<T>() {
        if (!typeof(T).IsSubclassOf(typeof(StatusEffect))) {
            Debug.Log("Wrong data type");
            return false;
        }

        foreach (StatusEffect status in statusEffects)
            if (status is T)
                return true;

        return false;
    }

    public int CheckStackCountOfEffectType<T>() {
        if (!typeof(T).IsSubclassOf(typeof(StatusEffect))) {
            Debug.Log("Wrong data type");
            return -1;
        }

        foreach (StatusEffect status in statusEffects)
            if (status is T)
                return status.ReturnStackCount();

        return -1;
    }
    #endregion

    #region Gold
    public void AddGold(float pAmount) {
        gold = Mathf.Max(gold + pAmount, 0);
        OnGoldChange?.Invoke(gold);
    }
    #endregion
}
