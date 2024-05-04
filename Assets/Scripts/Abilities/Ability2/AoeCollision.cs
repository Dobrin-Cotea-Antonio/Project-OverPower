using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AoeCollision : MonoBehaviour {
    public Action<PlayerData> OnAoeEnter;
    public Action<PlayerData> OnAoeExit;

    Vector3 initialScale;

    private void Awake() {
        initialScale = transform.localScale;
    }

    #region Trigger Events
    private void OnTriggerEnter(Collider other) {
        PlayerData p = other.GetComponent<PlayerData>();
        if (p != null)
            OnAoeEnter?.Invoke(p);
    }

    private void OnTriggerExit(Collider other) {
        PlayerData p = other.GetComponent<PlayerData>();
        if (p != null)
            OnAoeExit?.Invoke(p);
    }
    #endregion

    #region Scale
    public void SetSize(Vector2 pSize) {
        transform.localScale = new Vector3(initialScale.x * pSize.x, initialScale.y, initialScale.z*pSize.y);
    }
    #endregion
}
