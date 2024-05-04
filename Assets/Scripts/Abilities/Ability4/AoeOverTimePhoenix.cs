using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AoeOverTimePhoenix : MonoBehaviour {

    public Action<GameObject> OnImpact;

    [SerializeField] AoeCollision aoe;

    List<PlayerData> targets = new List<PlayerData>();

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
    public void SetSize(Vector2 pSize) {
        aoe.SetSize(pSize);
    }
    #endregion

    #region Helper Methods
    void AddTargetToList(PlayerData pData) {
        targets.Add(pData);
    }

    void RemoveTargetFromList(PlayerData pData) {
        targets.Remove(pData);
    }

    public List<PlayerData> ReturnTargetList() {
        return targets;
    }
    #endregion  
}
