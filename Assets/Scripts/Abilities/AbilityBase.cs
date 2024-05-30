using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class AbilityBase : MonoBehaviour {
    //cooldown,maxcooldown,charge cooldown left,max charge cooldown,charges,maxCharges,current ability
    public Action<float, float, float, float, int, int, AbilityBase> OnCooldownChange;//updating ui and maybe more in the future (displays the cooldown of the ability)
    public Action<float, float, AbilityBase> OnUseTimeChange;//updating ui and maybe more in the future (displays how long the ability is in use for)
    public Action<int, int, AbilityBase> OnChargeChange;//updating ui and maybe more in the future (displays how many charges the ability has)
    public Action<AbilityBase, int, int> OnAbilityLevelUp;//used for communicating with the ui once the ability is upgraded
    public Action<GameObject> OnAbilityImpact;

    [Header("Ability Data")]
    [SerializeField] protected Sprite _iconImage;
    [SerializeField] protected float[] abilityCooldown;
    [SerializeField] protected float[] abilityDuration;
    [SerializeField] protected float[] abilityRange;
    [SerializeField] protected string[] description;

    [Header("Charges")]
    [SerializeField] protected int[] maxCharges;
    [SerializeField] protected float[] chargeCooldown;

    [Header("Additional Info")]
    [SerializeField] protected bool[] blocksAbilities;//if false other abilities can be used while this one is active (Jax W)
    [SerializeField] protected bool[] moveToTarget;//if enabled the target will move towards the target if its outside of its range
    [SerializeField] protected int maxLevel;

    [Header("Ability Raycasting Data")]
    [SerializeField] protected Camera playerCamera;
    [SerializeField] protected LayerMask targetLayerMask;

    public int level { get; protected set; }
    public int charges { get; protected set; }
    public float cooldownLeft { get; protected set; }
    public float useTimeLeft { get; protected set; }
    public float chargeCooldownLeft { get; protected set; }
    public Sprite iconImage { get { return _iconImage; } }

    protected NavMeshPathfinder pathfinder;
    protected PlayerController playerController;

    protected virtual void Awake() {
        level = 0;
        charges = maxCharges[level];
        pathfinder = GetComponent<NavMeshPathfinder>();
        playerController = GetComponent<PlayerController>();
    }

    protected virtual void Update() {
        DecreaseChargeCooldownLeft();
        DecreaseCooldown();
        DecreaseUseTimeLeft();
    }

    bool BlockAbilities() {
        //if (blocksAbilities[level] == false)
        //    return false;
        //return isActive;
        return false;
    }

    #region Cooldown
    protected void DecreaseCooldown() {
        if (cooldownLeft == 0)
            return;

        cooldownLeft = Mathf.Max(cooldownLeft - Time.deltaTime, 0);

        if (cooldownLeft == 0) {
            if (charges <= maxCharges[level]) {
                charges++;

                if (charges != maxCharges[level]) {
                    cooldownLeft = abilityCooldown[level];
                }

                OnChargeChange?.Invoke(charges, maxCharges[level], this);
            }
        }

        OnCooldownChange?.Invoke(cooldownLeft, abilityCooldown[level], chargeCooldownLeft, chargeCooldown[level], charges, maxCharges[level], this);
    }

    protected void DecreaseUseTimeLeft() {
        if (useTimeLeft == 0)
            return;

        useTimeLeft = Mathf.Max(useTimeLeft - Time.deltaTime, 0);
        OnUseTimeChange?.Invoke(useTimeLeft, abilityDuration[level], this);

        if (useTimeLeft == 0) {
            cooldownLeft = abilityCooldown[level] - abilityDuration[level];
            if (maxCharges[level] != 1)
                chargeCooldownLeft = chargeCooldown[level] - abilityDuration[level];
        }
    }

    protected void DecreaseChargeCooldownLeft() {
        if (chargeCooldownLeft == 0 || maxCharges[level] == 1 || charges == maxCharges[level])
            return;

        chargeCooldownLeft = Mathf.Max(chargeCooldownLeft - Time.deltaTime, 0);
    }
    #endregion

    #region Ability
    public void UseAbility() {

        if (maxCharges[level] == 1) {
            if (useTimeLeft != 0 || cooldownLeft != 0)
                return;
        } else {
            if (useTimeLeft != 0 || chargeCooldownLeft != 0 || charges == 0)
                return;
        }

        pathfinder.ClearCommand();

        if (moveToTarget[level]) {
            playerController.AbilityMoveCommand(abilityRange[level]);
            pathfinder.OnDeadzoneMoveEnd += SetAbilityCooldown;
            pathfinder.OnDeadzoneMoveEnd += AbilityEffect;
        } else {
            SetAbilityCooldown();
            AbilityEffect();
        }
    }

    protected void SetAbilityCooldown() {
        if (maxCharges[level] != 1) {
            charges = Mathf.Max(charges - 1, 0);
            OnChargeChange?.Invoke(charges, maxCharges[level], this);
            //return;
        }

        //if (abilityDuration[level] != 0) {
        //    useTimeLeft = abilityDuration[level];
        //    return;
        //}

        if (abilityDuration[level] == 0) {
            cooldownLeft = abilityCooldown[level];
            if (maxCharges[level] != 1)
                chargeCooldownLeft = chargeCooldown[level];
        } else {
            //isActive = true;
            useTimeLeft = abilityDuration[level];
        }
    }

    protected virtual void AbilityEffect() {
        pathfinder.OnDeadzoneMoveEnd -= AbilityEffect;
        pathfinder.OnDeadzoneMoveEnd -= SetAbilityCooldown;
    }

    protected void OnImpact(GameObject pGameObject) {
        OnAbilityImpact?.Invoke(pGameObject);
    }
    #endregion

    #region Raycast For Target
    protected Vector3 RaycastForTarget() {
        Vector3 targetLocation = Vector3.zero;
        RaycastHit hit;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask)) {
            targetLocation = hit.point;
        }

        transform.forward = (targetLocation - transform.position).normalized;
        return targetLocation;
    }

    protected Vector3 GetTargetClickLocation() {
        Vector3 vec = playerController.ReturnTargetClickLocation();
        transform.forward = (vec - transform.position).normalized;
        return vec;
    }
    #endregion

    #region Level
    public void LevelUp() {
        int lastLevel = level;

        level = Mathf.Min(level + 1, maxLevel - 1);

        Debug.Log("test");

        if (level == lastLevel)
            return;

        if (charges != maxCharges[level]) {
            cooldownLeft = abilityCooldown[level];
        }

        OnAbilityLevelUp?.Invoke(this, level, maxLevel);
        OnChargeChange?.Invoke(charges, maxCharges[level], this);
    }
    #endregion
}
