using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallAbility : AbilityBase {

    [Header("Wall Data")]
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject slowEffectPrefab;
    [SerializeField] GameObject stunEffectPrefab;

    #region Ability
    protected override void AbilityEffect() {
        base.AbilityEffect();
        Vector3 targetLocation = GetTargetClickLocation();

        GameObject g = Instantiate(wallPrefab, targetLocation, Quaternion.identity);
        WallAbilityPlaced wall = g.GetComponent<WallAbilityPlaced>();
        g.transform.forward = (targetLocation - transform.position).normalized;

        wall.OnImpact += OnImpact;

        if (level == 0)
            wall.OnImpact += ApplySlow;
        else
            wall.OnImpact += ApplyStun;

        Destroy(g, abilityDuration[level]);
    }

    private void ApplySlow(GameObject pGameObject) {
        PlayerData data = pGameObject.GetComponent<PlayerData>();
        if (data == null)
            return;

        data.ApplyStatusEffect(slowEffectPrefab);
    }

    private void ApplyStun(GameObject pGameObject) {
        PlayerData data = pGameObject.GetComponent<PlayerData>();
        if (data == null)
            return;

        data.ApplyStatusEffect(stunEffectPrefab);
    }
    #endregion
}
