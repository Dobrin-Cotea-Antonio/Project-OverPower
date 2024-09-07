using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum State {
    Neutral,
    Captured
}

public class MapArea : MonoBehaviour {

    public System.Action<PlayerData> OnZoneEnter;
    public System.Action<PlayerData> OnZoneExit;

    public System.Action<Team> OnZoneCapture;
    public System.Action<Team> OnZoneLost;
    public System.Action<List<PlayerData>, Team, float, float> OnZoneValueChange;// ADD ALL THE INVOKES

    [Tooltip("Just for Debugging; Do not touch!")]
    [SerializeField] private State state;

    [Header("Data")]
    [SerializeField] private MapAreaData mapAreaData;
    [SerializeField] private MapAreaCollider mapAreaCollider;
    [SerializeField] private MeshRenderer meshRenderer;

    private Dictionary<Team, float> teamCaptureProgress = new Dictionary<Team, float>();
    private Dictionary<Team, bool> isPlayerOnZone = new Dictionary<Team, bool>();

    private int teamsInZone = 0;

    private Team ownerTeam = Team.Neutral;

    private const float maxCaptureValue = 100f;

    #region Unity Events
    private void Awake() {
        teamCaptureProgress[Team.Red] = 0;
        teamCaptureProgress[Team.Green] = 0;
        teamCaptureProgress[Team.Blue] = 0;

        isPlayerOnZone[Team.Red] = false;
        isPlayerOnZone[Team.Green] = false;
        isPlayerOnZone[Team.Blue] = false;

        state = State.Neutral;
    }

    private void Update() {
        teamsInZone = ReturnNumberOfTeamsInZone();

        switch (state) {
            case State.Neutral:
                NeutralState();
                break;
            case State.Captured:
                CaptureState();
                break;
        }
    }

    private void OnEnable() {
        OnZoneCapture += ZoneCaptured;
        OnZoneLost += ZoneLost;
    }

    private void OnDisable() {
        OnZoneCapture -= ZoneCaptured;
        OnZoneLost -= ZoneLost;
    }
    #endregion

    #region Neutral State
    private void NeutralState() {
        switch (teamsInZone) {
            case 0:
                DecayProgressOverTime();
                break;
            case 1:
                NeutralStateCapture();
                break;
            default:
                //Do nothing bcz we have to many teams fighting over the objective
                break;
        }
    }

    private void DecayProgressOverTime() {
        foreach (Team key in teamCaptureProgress.Keys.ToList()) {
            teamCaptureProgress[key] = Mathf.Clamp(teamCaptureProgress[key] - mapAreaData.captureDecayAutomatic * Time.deltaTime, 0, maxCaptureValue);
            OnZoneValueChange?.Invoke(mapAreaCollider.playersInsideArea, key, teamCaptureProgress[key], maxCaptureValue);
        }
    }

    private void NeutralStateCapture() {
        bool wasWrongTeamFoundWithProgress = false;
        Team capturingTeam = Team.Neutral;

        //check who has capture progress in the zone
        foreach (KeyValuePair<Team, bool> pair in isPlayerOnZone) {

            if (!pair.Value) {
                if (teamCaptureProgress[pair.Key] != 0)
                    wasWrongTeamFoundWithProgress = true;

                continue;
            }

            capturingTeam = pair.Key;
        }

        //if no other team has progress increase the progress of the target team
        if (!wasWrongTeamFoundWithProgress) {

            teamCaptureProgress[capturingTeam] = Mathf.Clamp(teamCaptureProgress[capturingTeam] + mapAreaData.captureSpeed * Time.deltaTime, 0, maxCaptureValue);
            OnZoneValueChange?.Invoke(mapAreaCollider.playersInsideArea, capturingTeam, teamCaptureProgress[capturingTeam], maxCaptureValue);

            if (teamCaptureProgress[capturingTeam] != maxCaptureValue)
                return;

            state = State.Captured;
            ownerTeam = capturingTeam;

            OnZoneCapture?.Invoke(ownerTeam);

            return;
        }

        //remove progress for the wrong team
        foreach (KeyValuePair<Team, bool> pair in isPlayerOnZone) {
            if (pair.Key == capturingTeam)
                continue;

            teamCaptureProgress[pair.Key] = Mathf.Clamp(teamCaptureProgress[pair.Key] - mapAreaData.captureDecayUnderEnemy * Time.deltaTime, 0, maxCaptureValue);
            OnZoneValueChange?.Invoke(mapAreaCollider.playersInsideArea, pair.Key, teamCaptureProgress[pair.Key], maxCaptureValue);
        }

    }
    #endregion

    #region Capture State
    private void CaptureState() {
        switch (teamsInZone) {
            case 0:
                ZoneRegainOverTime();
                break;
            case 1:
                CapturedStateCapture();
                break;
            default:
                //Do nothing bcz we have to many teams fighting over the objective
                break;
        }
    }

    private void CapturedStateCapture() {

        Team capturingTeam = Team.Neutral;

        //check who has capture progress in the zone
        foreach (KeyValuePair<Team, bool> pair in isPlayerOnZone)
            if (pair.Value) {
                capturingTeam = pair.Key;
                break;
            }

        //decrease the capture progress of all non capturing teams
        foreach (Team key in teamCaptureProgress.Keys.ToList()) { //remove the foreach bcz crash

            if (key == capturingTeam)
                continue;

            teamCaptureProgress[key] = Mathf.Clamp(teamCaptureProgress[key] - mapAreaData.captureDecayUnderEnemy * Time.deltaTime, 0, maxCaptureValue);

            //Debug.Log()

            OnZoneValueChange?.Invoke(mapAreaCollider.playersInsideArea, key, teamCaptureProgress[key], maxCaptureValue);

            if (key != ownerTeam)
                continue;

            if (teamCaptureProgress[ownerTeam] != 0)
                continue;

            Debug.Log("GG");

            state = State.Neutral;
            ownerTeam = Team.Neutral;
            OnZoneLost?.Invoke(ownerTeam);
        }

    }

    private void ZoneRegainOverTime() {
        bool canOwnerRegainProgress = true;

        foreach (Team key in teamCaptureProgress.Keys.ToList()) {
            if (key == ownerTeam)
                continue;

            teamCaptureProgress[key] = Mathf.Clamp(teamCaptureProgress[key] - mapAreaData.captureDecayUnderEnemy * Time.deltaTime, 0, maxCaptureValue);
            OnZoneValueChange?.Invoke(mapAreaCollider.playersInsideArea, ownerTeam, teamCaptureProgress[ownerTeam], maxCaptureValue);

            if (teamCaptureProgress[key] != 0)
                canOwnerRegainProgress = false;
        }


        if (canOwnerRegainProgress) {
            teamCaptureProgress[ownerTeam] = Mathf.Clamp(teamCaptureProgress[ownerTeam] + mapAreaData.captureRegainSpeed * Time.deltaTime, 0, maxCaptureValue);
            OnZoneValueChange?.Invoke(mapAreaCollider.playersInsideArea, ownerTeam, teamCaptureProgress[ownerTeam], maxCaptureValue);
        }
    }
    #endregion

    #region Helper Methods
    private int ReturnNumberOfTeamsInZone() {
        List<Team> teamList = new List<Team>();

        isPlayerOnZone[Team.Red] = false;
        isPlayerOnZone[Team.Green] = false;
        isPlayerOnZone[Team.Blue] = false;

        for (int i = 0; i < mapAreaCollider.playersInsideArea.Count; i++) {
            Team team = mapAreaCollider.playersInsideArea[i].team;

            if (teamList.Contains(team))
                continue;

            teamList.Add(team);
            isPlayerOnZone[team] = true;
        }

        return teamList.Count;
    }

    private void ZoneCaptured(Team pTeam) {
        meshRenderer.material = ZoneReusableData.instance.teamZoneMaterial[pTeam];
    }

    private void ZoneLost(Team pTeam) {
        meshRenderer.material = ZoneReusableData.instance.teamZoneMaterial[Team.Neutral];
    }
    #endregion
}