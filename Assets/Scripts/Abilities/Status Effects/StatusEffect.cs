using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class StatusEffect : MonoBehaviour {

    public Action<StatusEffect> OnStatusDestroy;

    [SerializeField] protected StatusEffectData data;

    protected float timeElapsed = 0;
    protected float duration;
    protected IStatusEffectReceiver target;
    [SerializeField] protected int stackCount = 0;

    private void Awake() {
        duration = data.duration;
    }

    #region Functionality
    protected abstract void Apply();

    protected abstract void ResetEffect();

    protected abstract void OnSetTarget(GameObject pTarget);

    protected void UpdateStackCount() {
        if (target == null)
            return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed > duration) {
            stackCount--;
            timeElapsed = 0;
        }

        if (stackCount <= 0)
            Remove();
    }

    public void StackEffect() {
        Apply();
    }

    protected void Remove() {
        ResetEffect();
        OnStatusDestroy?.Invoke(this);
        Destroy(this.gameObject);
    }

    public void SetTarget(GameObject pTarget) {
        target = pTarget.GetComponent<IStatusEffectReceiver>();
        OnSetTarget(pTarget);
    }
    #endregion

    #region Helper Methods
    public Sprite GetImage() {
        return data.image;
    }

    public float ReturnTimeLeft() {
        return duration - timeElapsed;
    }

    public int ReturnStackCount() {
        return stackCount;
    }

    public void SetDuration(float pValue) {
        duration = pValue;
    }
    #endregion
}
