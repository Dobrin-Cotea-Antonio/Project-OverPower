using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class AbilityBase : MonoBehaviour {

    public Action<float, float> onCooldownChange;
    public Action OnAbilityStart;
    public Action OnAbilityEnd;

    [Header("Ability Data")]
    [SerializeField] Sprite iconImage;
    [SerializeField] protected float abilityCooldown;
    [SerializeField] protected float abilityDuration;
    [SerializeField] string description;
    [SerializeField] protected int maxLevel = 3;
    [SerializeField] protected bool blocksAbilities;//if false other abilities can be used while this one is active (Jax W)

    public int level { get; protected set; }
    public bool isActive { get; protected set; }
    public float cooldownLeft { get; protected set; }

    protected virtual void Awake() {
        level = 1;
    }

    protected virtual void Update() {
        DecreaseCooldown();
    }

    bool BlockAbilities() {
        if (blocksAbilities == false)
            return false;
        return isActive;
    }

    #region Cooldown
    protected void DecreaseCooldown() {
        if (cooldownLeft == 0)
            return;
        cooldownLeft = Mathf.Max(cooldownLeft - Time.deltaTime, 0);
        onCooldownChange?.Invoke(cooldownLeft, abilityCooldown);
    }
    #endregion

    #region Ability
    public abstract void UseAbility();
    #endregion
}
