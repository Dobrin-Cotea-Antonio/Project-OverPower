using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : StatusEffect {
    private PlayerData playerData;

    #region Functionality
    protected override void Apply() {
        if (target == null) {
            Remove();
            return;
        }

        stackCount = 1;

        if (timeElapsed == 0)
            playerData.isStunned = true;
        else
            timeElapsed = 0;
    }

    protected override void OnSetTarget(GameObject pTarget) {
        playerData = pTarget.GetComponent<PlayerData>();
    }

    protected override void ResetEffect() {
        playerData.isStunned = false;
    }

    private void Update() {
        UpdateStackCount();
    }
    #endregion Functionality
}
