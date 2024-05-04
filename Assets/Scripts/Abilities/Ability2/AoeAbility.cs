using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAbility : AbilityBase {
    [Header("AOE Data")]
    [SerializeField] GameObject aoePrefab;
    [SerializeField] PlayerData owner;
    [SerializeField] float damage;
    [SerializeField] Vector2 aoeSize;

    [Header("Upgrade")]
    [SerializeField] GameObject aoeOvertimePrefab;
    [SerializeField] float overtimeDamage;
    [SerializeField] float overtimeDuration;

    #region Ability
    protected override void AbilityEffect() {
        base.AbilityEffect();
        Vector3 targetLocation = GetTargetClickLocation();

        GameObject g = Instantiate(aoePrefab, targetLocation, Quaternion.identity);
        AoeTimed aoe = g.GetComponent<AoeTimed>();
        aoe.SetDamage(damage);
        aoe.SetDelay(abilityDuration[level]);
        aoe.SetOwner(owner);
        aoe.SetSize(aoeSize);
        aoe.OnImpact += OnImpact;

        if (level >= 1) {
            aoe.OnDestroy += SpawnOvertimeAoe;
        }
    }

    private void SpawnOvertimeAoe(GameObject pGameObject) {
        GameObject g = Instantiate(aoeOvertimePrefab, pGameObject.transform.position, Quaternion.identity);
        AoeOverTime aoeOverTime = g.GetComponent<AoeOverTime>();
        aoeOverTime.SetDamage(overtimeDamage);
        aoeOverTime.SetDuration(overtimeDuration);
        aoeOverTime.SetOwner(owner);
        aoeOverTime.SetSize(aoeSize);
    }

    #endregion
}
