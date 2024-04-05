using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class AbilityBase : MonoBehaviour {

    public Action<float, float, AbilityBase> OnCooldownChange;//updating ui and maybe more in the future
    public Action<float, float, AbilityBase> OnUseTimeChange;//updating ui and maybe more in the future
    public Action OnAbilityStart;//adding status effects and other stuff like that
    public Action OnAbilityEnd;//adding status effects and other stuff like that
    public Action<int, int> OnAbilityUpgrade;//used for communicating with the ui once the ability is upgraded

    [Header("Ability Data")]
    [SerializeField] protected Sprite _iconImage;
    [SerializeField] protected float abilityCooldown;
    [SerializeField] protected float abilityDuration;
    [SerializeField] protected string description;
    [SerializeField] protected int maxLevel = 3;
    [SerializeField] protected bool blocksAbilities;//if false other abilities can be used while this one is active (Jax W)

    public int level { get; protected set; }
    public bool isActive { get; protected set; }
    public float cooldownLeft { get; protected set; }
    public float useTimeLeft { get; protected set; }
    public Sprite iconImage { get { return _iconImage; } }

    protected virtual void Awake() {
        level = 1;
    }

    protected virtual void Update() {
        DecreaseCooldown();
        DecreaseUseTimeLeft();
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
        OnCooldownChange?.Invoke(cooldownLeft, abilityCooldown, this);
    }

    protected void DecreaseUseTimeLeft() {
        if (useTimeLeft == 0)
            return;

        useTimeLeft = Mathf.Max(useTimeLeft - Time.deltaTime, 0);
        OnUseTimeChange?.Invoke(useTimeLeft, abilityDuration, this);

        if (useTimeLeft == 0)
            cooldownLeft = abilityCooldown;
    }
    #endregion

    #region Ability
    public void UseAbility() {
        if (useTimeLeft != 0 || cooldownLeft != 0)
            return;

        if (abilityDuration == 0) {
            cooldownLeft = abilityCooldown;
        } else {
            isActive = true;
            useTimeLeft = abilityDuration;
        }

        AbilityEffect();
    }

    protected abstract void AbilityEffect();
    #endregion
}
