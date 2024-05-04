using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : StatusEffect {
    private PlayerData playerData;

    #region Functionality
    protected override void Apply() {
        if (target == null) {
            Remove();
            return;
        }

        stackCount = 1;

        if (timeElapsed == 0)
            playerData.ModifySpeedByPercentage(-data.value);
        else
            timeElapsed = 0;
    }

    protected override void OnSetTarget(GameObject pTarget) {
        playerData = pTarget.GetComponent<PlayerData>();
    }

    protected override void ResetEffect() {
        playerData.ModifySpeedByPercentage(data.value);
    }

    private void Update() {
        UpdateStackCount();
    }
    #endregion Functionality
}
