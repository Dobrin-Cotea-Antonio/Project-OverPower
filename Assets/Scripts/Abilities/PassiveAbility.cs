using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : AbilityBase {
    [SerializeField] GameObject burnEffectPrefab;

    protected override void Start() {
        base.Start();
        UseAbility();
    }

    #region Ability
    protected override void AbilityEffect() {
        base.AbilityEffect();
        AbilityBase[] abilities = GetComponents<AbilityBase>();

        foreach (AbilityBase ability in abilities) {
            if (ability == this)
                continue;

            ability.OnAbilityImpact += ApplyBurn;
        }
    }

    void ApplyBurn(GameObject pGameObject) {
        IStatusEffectReceiver target = pGameObject.GetComponent<IStatusEffectReceiver>();

        if (target == null)
            return;

        target.ApplyStatusEffect(burnEffectPrefab);
    }
    #endregion
}
