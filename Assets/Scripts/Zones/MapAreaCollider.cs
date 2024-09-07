using System.Collections.Generic;
using UnityEngine;

public class MapAreaCollider : MonoBehaviour {

    public List<PlayerData> playersInsideArea { get; private set; }

    #region Unity Events
    private void Awake() {
        playersInsideArea = new List<PlayerData>();
    }

    private void OnTriggerEnter(Collider pOther) {
        PlayerData data = pOther.GetComponent<PlayerData>();

        if (data == null)
            return;

        if (data.UIManager != null)
            data.UIManager.EnableCaptureBars(true);

        if (data.team != Team.Neutral) 
            playersInsideArea.Add(data);
        
    }

    private void OnTriggerExit(Collider pOther) {
        PlayerData data = pOther.GetComponent<PlayerData>();

        if (data.UIManager != null)
            data.UIManager.EnableCaptureBars(false);

        if (data.team != Team.Neutral)
            playersInsideArea.Remove(data);

        //if (data != null) {
        //    data.UIManager.EnableCaptureBars(false);
        //    playersInsideArea.Remove(data);
        //}
    }
    #endregion
}
