using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public enum Team {
    Red,
    Green,
    Blue
}

public class ZoneReusableData : MonoBehaviour {

    public ZoneReusableData instance { get; private set; }

    [SerializeField] SerializedDictionary<Team, Material> teamColor;

    private void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
