using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : AbilityBase {

    [Header("Projectile Data")]
    [SerializeField] Collider ownerCollider;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] float damage;
    [SerializeField] float extraDamageToBurningTargets;
    [SerializeField] Vector2 explosionSize;
    [SerializeField] float projectileSpeed;

    #region Ability
    protected override void AbilityEffect() {
        base.AbilityEffect();

        Vector3 targetLocation = RaycastForTarget(true);

        if (targetLocation == Vector3.zero)
            return;

        targetLocation.y = spawnPosition.position.y;

        GameObject p = Instantiate(projectilePrefab, spawnPosition.position, Quaternion.identity);
        Vector3 forward = (targetLocation - spawnPosition.position).normalized;

        Projectile projectile = p.GetComponent<Projectile>();
        projectile.SetDamage(damage);
        projectile.SetExplosionSize(explosionSize);
        projectile.SetRange(abilityRange[level]);
        projectile.SetSpeed(projectileSpeed, forward);
        projectile.SetOwnerCollider(ownerCollider);
        projectile.SetExtraDamageFromBurning(extraDamageToBurningTargets);
        projectile.OnImpact += OnImpact;

    }
    #endregion
}
