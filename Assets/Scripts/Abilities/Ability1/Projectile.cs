using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : DamageDealerBase {

    [Header("Stats")]
    [SerializeField] protected ProjectileData projectileData;

    protected Vector3 initialPosition;

    #region Unity Events 
    protected override void Awake() {
        base.Awake();
        initialPosition = transform.position;
    }
    #endregion

    #region Updates
    protected virtual void Update() {
        CheckRange();
    }
    #endregion

    #region Helper Methods
    private void CheckRange() {
        if ((initialPosition - transform.position).magnitude > projectileData.range)
            Destroy(gameObject);
    }

    public override void SetTeam(Team pTeam) {
        base.SetTeam(pTeam);

        rb.AddForce(projectileData.speed * transform.forward, ForceMode.VelocityChange);
        col.enabled = true;
    }

    protected virtual void ApplyStatusEffects(PlayerData pPlayerData) {
        foreach (GameObject statusEffects in projectileData.statusEffectsOnHit)
            pPlayerData.ApplyStatusEffect(statusEffects);
    }

    protected virtual void ApplyAdditionalActions(Collision pCollision) {
        foreach (GameObject action in projectileData.additionalActionsOnHit) {
            GameObject g = Instantiate(action, pCollision.transform.position, Quaternion.identity);
            //finish
        }
    }
    #endregion

    #region Collision
    private void OnCollisionEnter(Collision pCollision) {

        PlayerData playerData = pCollision.gameObject.GetComponent<PlayerData>();

        if (playerData == null)
            return;

        playerData.TakeDamage(projectileData.damage);
        OnDamageDealt?.Invoke(playerData, projectileData.damage);

        ApplyStatusEffects(playerData);
        ApplyAdditionalActions(pCollision);

        Destroy(gameObject);
    }
    #endregion
}
