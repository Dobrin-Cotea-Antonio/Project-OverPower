using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAbility : AbilityBase {

    /// <summary>
    /// has infinite range so it has to be fixed
    /// </summary>

    [Header("AOE Data")]
    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject aoePrefab;
    [SerializeField] PlayerData owner;
    [SerializeField] float damage;
    [SerializeField] float aoeSize;

    #region Ability
    protected override void AbilityEffect() {
        Vector3 targetLocation = Vector3.zero;

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask)) {
            targetLocation = hit.point;
        }

        if (targetLocation == Vector3.zero)
            return;

        GameObject g = Instantiate(aoePrefab, targetLocation, Quaternion.identity);
        Aoe aoe = g.GetComponent<Aoe>();
        aoe.SetDamage(damage);
        aoe.SetDelay(abilityDuration);
        aoe.SetOwner(owner);
        aoe.SetSize(aoeSize);

    }
    #endregion
}
