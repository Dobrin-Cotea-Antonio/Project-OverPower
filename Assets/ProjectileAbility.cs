using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : AbilityBase {

    [Header("Projectile Data")]
    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] Collider ownerCollider;
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] float damage;
    [SerializeField] float projectileRange;
    [SerializeField] float explosionSize;
    [SerializeField] float projectileSpeed;

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

        //will have to rotate the player towards where the players clicks and casts the ability for now it will just shoot 
        //in that direction

        targetLocation.y = spawnPosition.position.y;

        //WHY THE FUCK SHOOT THE WRONG DIRECTION ONLY SOMETIMES

        GameObject p = Instantiate(projectilePrefab, spawnPosition.position, Quaternion.identity);
        p.transform.forward = (targetLocation - spawnPosition.position).normalized;

        Projectile projectile = p.GetComponent<Projectile>();
        projectile.SetDamage(damage);
        projectile.SetExplosionSize(explosionSize);
        projectile.SetOwnerCollider(ownerCollider);
        projectile.SetRange(projectileRange);
        projectile.SetSpeed(projectileSpeed);

    }
    #endregion
}
