using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class AbilityBase : MonoBehaviour {

    public Action<float, float, AbilityBase> OnCooldownChange;//updating ui and maybe more in the future
    public Action<float, float, AbilityBase> OnUseTimeChange;//updating ui and maybe more in the future
    public Action<AbilityBase, int, int> OnAbilityLevelUp;//used for communicating with the ui once the ability is upgraded
    public Action<GameObject> OnAbilityImpact;

    [Header("Ability Data")]
    [SerializeField] protected Sprite _iconImage;
    [SerializeField] protected float[] abilityCooldown;
    [SerializeField] protected float[] abilityDuration;
    [SerializeField] protected float[] abilityRange;
    [SerializeField] protected string[] description;
    [SerializeField] protected int[] maxCharges;
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
    public Sprite iconImage { get { return _iconImage; } }

    protected NavMeshPathfinder pathfinder;
    protected PlayerController playerController;

    protected virtual void Awake() {
        level = -1;
        LevelUp();
        charges = maxCharges[level];
        pathfinder = GetComponent<NavMeshPathfinder>();
        playerController = GetComponent<PlayerController>();
    }

    protected virtual void Update() {
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
        OnCooldownChange?.Invoke(cooldownLeft, abilityCooldown[level], this);
    }

    protected void DecreaseUseTimeLeft() {
        if (useTimeLeft == 0)
            return;

        useTimeLeft = Mathf.Max(useTimeLeft - Time.deltaTime, 0);
        OnUseTimeChange?.Invoke(useTimeLeft, abilityDuration[level], this);

        if (useTimeLeft == 0)
            cooldownLeft = abilityCooldown[level];
    }
    #endregion

    #region Ability
    public void UseAbility() {
        if (useTimeLeft != 0 || cooldownLeft != 0)
            return;

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
        if (abilityDuration[level] == 0) {
            cooldownLeft = abilityCooldown[level];
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
    protected void UpdateCharges(int pAmount) {

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
        level = Mathf.Min(level + 1, maxLevel - 1);
        OnAbilityLevelUp?.Invoke(this, level, maxLevel);
    }
    #endregion
}
