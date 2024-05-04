using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallAbilityPlaced : MonoBehaviour {
    public Action<GameObject> OnImpact;

    [SerializeField] WallCollision aoe;

    #region Events
    void OnEnable() {
        aoe.OnAoeEnter += SlowTarget;
        //aoe.OnAoeExit += RemoveTargetFromList;
    }

    void OnDisable() {
        aoe.OnAoeEnter -= SlowTarget;
        //aoe.OnAoeExit -= RemoveTargetFromList;
    }
    #endregion

    #region Helper Methods
    void SlowTarget(PlayerData pData) {
        OnImpact?.Invoke(pData.gameObject);
    }

    #endregion 


}
