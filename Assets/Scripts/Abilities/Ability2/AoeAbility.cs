using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAbility : AbilityBase {
    [Header("AOE Data")]
    [SerializeField] protected GameObject aoePrefab;
    [Tooltip("If ticked, the ability will spawn on top of the player ignoring range, otherwise will spawn towards the curson")]
    [SerializeField] protected bool spawnsOnPlayer;

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

        GameObject g;

        if (spawnsOnPlayer) {
            g = Instantiate(aoePrefab, transform.position, Quaternion.identity);
        } else {
            Vector3 targetLocation = GetTargetClickLocation();
            g = Instantiate(aoePrefab, targetLocation, Quaternion.identity);
        }

        AOE aoe = g.GetComponent<AOE>();
        aoe.SetTeam(playerData.team);
    }
    #endregion
}
