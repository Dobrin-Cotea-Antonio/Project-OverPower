using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AoeOverTime : MonoBehaviour {

    public Action<GameObject> OnImpact;

    [SerializeField] AoeCollision aoe;

    float damage;
    float duration;

    float timeLeftUntilDamage = -10000f;
    List<PlayerData> targets = new List<PlayerData>();
    PlayerData owner;

    void Update() {
        AtteptToDamageTargets();
    }

    #region Events
    void OnEnable() {
        aoe.OnAoeEnter += AddTargetToList;
        aoe.OnAoeExit += RemoveTargetFromList;
    }

    void OnDisable() {
        aoe.OnAoeEnter -= AddTargetToList;
        aoe.OnAoeExit -= RemoveTargetFromList;
    }
    #endregion

    #region Setter Methods
    public void SetDuration(float pDelay) {
        duration = pDelay;
        timeLeftUntilDamage = duration;
    }

    public void SetDamage(float pDamage) {
        damage = pDamage;
    }

    public void SetSize(Vector2 pSize) {
        aoe.SetSize(pSize);
    }

    public void SetOwner(PlayerData pOwner) {
        owner = pOwner;
    }
    #endregion

    #region Target Damage
    void AtteptToDamageTargets() {
        if (timeLeftUntilDamage == 0) {
            Destroy(gameObject);
            return;
        }

        timeLeftUntilDamage = Mathf.Max(0, timeLeftUntilDamage - Time.deltaTime);
        foreach (PlayerData target in targets)
            if (target != owner) {
                target.TakeDamage(damage * Time.deltaTime);
                OnImpact?.Invoke(target.gameObject);
            }
    }
    #endregion

    #region Helper Methods
    void AddTargetToList(PlayerData pData) {
        targets.Add(pData);
    }

    void RemoveTargetFromList(PlayerData pData) {
        targets.Remove(pData);
    }

    public bool ListContainsPlayer(PlayerData pData) {
        return targets.Contains(pData);
    }
    #endregion 

}
