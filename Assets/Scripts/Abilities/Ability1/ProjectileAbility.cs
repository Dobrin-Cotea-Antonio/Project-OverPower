using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : AbilityBase {

    [Header("Projectile Data")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPosition;

    PlayerData playerData;

    #region Unity Events
    protected override void Start() {
        base.Start();

        playerData = GetComponent<PlayerData>();
    }
    #endregion

    #region Ability
    protected override void AbilityEffect() {
        base.AbilityEffect();

        Vector3 targetLocation = RaycastForTarget(true);

        if (targetLocation == Vector3.zero)
            return;

        targetLocation.y = spawnPosition.position.y;

        GameObject p = Instantiate(projectilePrefab, spawnPosition.position, Quaternion.identity);

        Vector3 forward = (targetLocation - spawnPosition.position).normalized;
        p.transform.forward = forward;

        Projectile projectile = p.GetComponent<Projectile>();
        projectile.SetTeam(playerData.team);
    }
    #endregion
}
