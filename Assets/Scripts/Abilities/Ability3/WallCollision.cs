using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallCollision : MonoBehaviour {
    public Action<PlayerData> OnAoeEnter;
    public Action<PlayerData> OnAoeExit;

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
}
