using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : StatusEffect {

    private IDamagable damagableTarget;

    #region Functionality
    protected override void Apply() {
        if (target == null) {
            Remove();
            return;
        }
        stackCount = Mathf.Min(stackCount + 1, data.maxStackCount);
        timeElapsed = 0;
    }

    protected override void OnSetTarget(GameObject pTarget) {
        damagableTarget = pTarget.GetComponent<IDamagable>();
    }

    protected override void ResetEffect() {
        timeElapsed = 0;
    }

    private void Update() {
        UpdateStackCount();

        if (damagableTarget == null)
            return;

        damagableTarget.TakeDamage(data.value * Time.deltaTime);
    }
    #endregion Functionality
}
