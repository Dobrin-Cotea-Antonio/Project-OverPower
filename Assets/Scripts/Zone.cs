using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class Zone : MonoBehaviour {

    Dictionary<Team, List<PlayerData>> playersInZone = new Dictionary<Team, List<PlayerData>>();

    Team ownerTeam;

    private void Update() {
        
    }

    void CalculateWinningTeam() { 
    
    }

    private void OnCollisionEnter(Collision collision) {
        PlayerData data = collision.gameObject.GetComponent<PlayerData>();
        if (data == null)
            return;
        if (!playersInZone[data.team].Contains(data))
            playersInZone[data.team].Add(data);
    }



}
