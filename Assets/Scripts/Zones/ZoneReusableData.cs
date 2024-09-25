using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public enum Team {
    Red,
    Green,
    Blue,
    Neutral
}

public class ZoneReusableData : MonoBehaviour {

    public static ZoneReusableData instance { get; private set; }

    public SerializedDictionary<Team, Material> teamZoneMaterial = new SerializedDictionary<Team, Material>();
    public SerializedDictionary<Team, Color> teamZoneColor = new SerializedDictionary<Team, Color>();

    #region Unity Events
    private void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion
}
