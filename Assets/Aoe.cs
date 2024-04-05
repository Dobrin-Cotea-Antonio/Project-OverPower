using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aoe : MonoBehaviour {

    [SerializeField] AoeCollision aoe;

    float damage;
    float delay;

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
    public void SetDelay(float pDelay) {
        delay = pDelay;
        timeLeftUntilDamage = delay;
    }

    public void SetDamage(float pDamage) {
        damage = pDamage;
    }

    public void SetSize(float pSize) {
        aoe.SetSize(pSize);
    }

    public void SetOwner(PlayerData pOwner) {
        owner = pOwner;
    }
    #endregion

    #region Target Damage
    void AtteptToDamageTargets() {
        if (timeLeftUntilDamage != 0) {
            timeLeftUntilDamage = Mathf.Max(0, timeLeftUntilDamage - Time.deltaTime);
        } else {
            foreach (PlayerData target in targets)
                if (target != owner)
                    target.TakeDamage(damage);
            Destroy(gameObject);
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
    #endregion 

}
