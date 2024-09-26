using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AOE : DamageDealerBase {

    public Action<GameObject> OnImpact;
    public Action<GameObject> OnDestroy;

    [SerializeField] AOEData aoeData;
    [SerializeField] AoeCollision aoeCollision;

    float timeLeftUntilDamage = -10000f;
    List<PlayerData> targets = new List<PlayerData>();

    #region Unity Events
    protected override void Start() {
        base.Start();
        aoeCollision.SetSize(aoeData.aoeSize);
    }

    protected virtual void Update() {
        AtteptToDamageTargets();
    }

    protected virtual void OnEnable() {
        aoeCollision.OnAoeEnter += AddTargetToList;
        aoeCollision.OnAoeExit += RemoveTargetFromList;
    }

    protected virtual void OnDisable() {
        aoeCollision.OnAoeEnter -= AddTargetToList;
        aoeCollision.OnAoeExit -= RemoveTargetFromList;
    }
    #endregion

    #region Target Damage
    void AtteptToDamageTargets() {

        if (timeLeftUntilDamage != 0) {
            timeLeftUntilDamage = Mathf.Max(0, timeLeftUntilDamage - Time.deltaTime);
            return;
        }

        foreach (PlayerData target in targets) {
            target.TakeDamage(aoeData.damage);
            //OnImpact?.Invoke(target.gameObject);

            ApplyStatusEffects(target);
            ApplyAdditionalActions(target.gameObject);
        }

        OnDestroy?.Invoke(gameObject);
        Destroy(gameObject);

    }
    #endregion

    #region Helper Methods
    void AddTargetToList(PlayerData pData) {
        targets.Add(pData);
    }

    void RemoveTargetFromList(PlayerData pData) {
        targets.Remove(pData);
    }

    protected virtual void ApplyStatusEffects(PlayerData pPlayerData) {
        foreach (GameObject statusEffects in aoeData.statusEffectsOnHit)
            pPlayerData.ApplyStatusEffect(statusEffects);
    }

    protected virtual void ApplyAdditionalActions(GameObject pTarget) {
        foreach (GameObject action in aoeData.additionalActionsOnHit) {
            GameObject g = Instantiate(action, pTarget.transform.position, Quaternion.identity);
            //finish
        }
    }
    #endregion 
}
