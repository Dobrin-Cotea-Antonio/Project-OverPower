using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerData), typeof(PlayerController))]
abstract public class AbilityBase : MonoBehaviour {
    //cooldown,maxcooldown,charge cooldown left,max charge cooldown,charges,maxCharges,current ability
    public Action<float, float, float, float, int, int, AbilityBase> OnCooldownChange;//updating ui and maybe more in the future (displays the cooldown of the ability)
    public Action<float, float, AbilityBase> OnUseTimeChange;//updating ui and maybe more in the future (displays how long the ability is in use for)
    public Action<int, int, AbilityBase> OnChargeChange;//updating ui and maybe more in the future (displays how many charges the ability has)
    public Action<AbilityBase, int, int> OnAbilityLevelUp;//used for communicating with the ui once the ability is upgraded
    public Action<GameObject> OnAbilityImpact;

    [Header("Data")]
    [SerializeField] protected AbilityData _abilityData;

    [Header("Ability Raycasting Data")]
    [SerializeField] protected Camera playerCamera;
    [SerializeField] protected LayerMask targetLayerMask;

    public int charges { get; protected set; }
    public float cooldownLeft { get; protected set; }
    public float useTimeLeft { get; protected set; }
    public float chargeCooldownLeft { get; protected set; }
    public AbilityData abilityData { get { return _abilityData; } }

    protected PlayerController playerController;

    #region Unity Events
    protected virtual void Awake() {
        playerController = GetComponent<PlayerController>();
    }

    protected virtual void Start() { }

    protected virtual void Update() {
        DecreaseChargeCooldownLeft();
        DecreaseCooldown();
        DecreaseUseTimeLeft();
    }
    #endregion

    #region Cooldown
    protected void DecreaseCooldown() {
        if (cooldownLeft == 0)
            return;

        cooldownLeft = Mathf.Max(cooldownLeft - Time.deltaTime, 0);

        if (cooldownLeft == 0) {
            if (charges <= abilityData.maxCharges) {
                charges++;

                if (charges != abilityData.maxCharges) {
                    cooldownLeft = abilityData.abilityCooldown;
                }

                OnChargeChange?.Invoke(charges, abilityData.maxCharges, this);
            }
        }

        OnCooldownChange?.Invoke(cooldownLeft, abilityData.abilityCooldown, chargeCooldownLeft, abilityData.chargeCooldown, charges, abilityData.maxCharges, this);
    }

    protected void DecreaseUseTimeLeft() {
        if (useTimeLeft == 0)
            return;

        useTimeLeft = Mathf.Max(useTimeLeft - Time.deltaTime, 0);
        OnUseTimeChange?.Invoke(useTimeLeft, abilityData.abilityDuration, this);

        if (useTimeLeft == 0) {
            cooldownLeft = abilityData.abilityCooldown - abilityData.abilityDuration;
            if (abilityData.maxCharges != 1)
                chargeCooldownLeft = abilityData.chargeCooldown - abilityData.abilityDuration;
        }
    }

    protected void DecreaseChargeCooldownLeft() {
        if (chargeCooldownLeft == 0 || abilityData.maxCharges == 1 || charges == abilityData.maxCharges)
            return;

        chargeCooldownLeft = Mathf.Max(chargeCooldownLeft - Time.deltaTime, 0);
    }
    #endregion

    #region Ability
    public void UseAbility() {

        if (abilityData.maxCharges == 1) {
            if (useTimeLeft != 0 || cooldownLeft != 0)
                return;
        } else {
            if (useTimeLeft != 0 || chargeCooldownLeft != 0 || charges == 0)
                return;
        }

        SetAbilityCooldown();
        AbilityEffect();
    }

    protected void SetAbilityCooldown() {
        if (abilityData.maxCharges != 1) {
            charges = Mathf.Max(charges - 1, 0);
            OnChargeChange?.Invoke(charges, abilityData.maxCharges, this);

            if (charges != 0)
                cooldownLeft = abilityData.abilityCooldown;

            chargeCooldownLeft = abilityData.chargeCooldown;
            return;
        }

        if (abilityData.abilityDuration == 0) {
            cooldownLeft = abilityData.abilityCooldown;
            if (abilityData.maxCharges != 1)
                chargeCooldownLeft = abilityData.chargeCooldown;
        } else {
            useTimeLeft = abilityData.abilityDuration;
        }
    }

    protected virtual void AbilityEffect() { }

    protected void OnImpact(GameObject pGameObject) {
        OnAbilityImpact?.Invoke(pGameObject);
    }
    #endregion

    #region Raycast For Target
    protected Vector3 RaycastForTarget(bool pSetTransformForward = false) {
        Vector3 targetLocation = Vector3.zero;
        RaycastHit hit;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask)) 
            targetLocation = hit.point;

        transform.forward = (targetLocation - transform.position).normalized;
        return targetLocation;
    }

    protected Vector3 GetTargetClickLocation() {
        Vector3 pos = transform.position;

        Vector3 targetPos = RaycastForTarget();

        Vector3 distanceVector = (targetPos - pos);
        float distance = distanceVector.magnitude;

        if (distance > abilityData.abilityRange)
            targetPos = pos + distanceVector.normalized * abilityData.abilityRange;

        return targetPos;
    }
    #endregion
}
