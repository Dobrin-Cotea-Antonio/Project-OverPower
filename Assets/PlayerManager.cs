using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private SerializedDictionary<Team, List<PlayerData>> playerDictionary;

    [SerializeField] private List<MapArea> mapAreas;

    #region Unity Events
    private void OnEnable() {
        foreach (MapArea area in mapAreas)
            area.OnZoneUpdate += AddGoldToTeam;
    }

    private void OnDisable() {
        foreach (MapArea area in mapAreas)
            area.OnZoneUpdate -= AddGoldToTeam;
    }
    #endregion

    #region Gold Generation
    private void AddGoldToTeam(MapAreaData pData, Team pTeam, float pCaptureProgress) {
        if (pCaptureProgress != MapArea.maxCaptureValue)
            return;

        foreach (KeyValuePair<Team, List<PlayerData>> pair in playerDictionary)
            foreach (PlayerData data in pair.Value)
                data.ModifyGold(pData.goldPerSecond * Time.deltaTime);
    }
    #endregion

}
