using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public enum State
    {
        Neutral,
        Captured
    }
    public State state;
    public float progressSpeed = 0.5f;

    
    private List<MapAreaCollider> _mapAreaColliders;
    private float _progress;
    public int zoneTier;
    private PlayerMapAreas _firstPlayerTeam;
    private PlayerMapAreas _currentTeamColor;
    private float _captureMultiplier = 2f;
    
    private void Awake()
    {
        _mapAreaColliders = new List<MapAreaCollider>();
        foreach (Transform child in transform)
        {
            MapAreaCollider mapAreaCollider = child.GetComponent<MapAreaCollider>();
            if (mapAreaCollider != null)
            {
                _mapAreaColliders.Add(mapAreaCollider);
            }
        }

        state = State.Neutral;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Neutral:
                ZoneCapturing();

                if (_progress >= 1f)
                {
                    state = State.Captured;
                    Debug.Log("Capture");
                }
                break;
            case State.Captured:
                ZoneCapturing();
                if (zoneTier == 4)
                {
                    foreach (PlayerMapAreas player in NumberOfPlayers.AllPlayers)
                    {
                        if (player.team == _currentTeamColor.team)
                        {
                            player.objBuff = true;
                        }
                    }
                }
                
                break;
        }
    }

    private void ZoneCapturing()
    {
        if (_progress > 1f)
        {
            _progress = 1f;
        }
        
        List<PlayerMapAreas> playerMapAreasInsideList = new List<PlayerMapAreas>();
        
        foreach (MapAreaCollider mapAreaCollider in _mapAreaColliders)
        {
            foreach (PlayerMapAreas playerMapAreas in mapAreaCollider.GetPlayerAreasList())
            {
                if (!playerMapAreasInsideList.Contains(playerMapAreas))
                {
                    playerMapAreasInsideList.Add(playerMapAreas);
                }
            }
        }

        _firstPlayerTeam = playerMapAreasInsideList.LastOrDefault();
        if(_firstPlayerTeam == null) return;
                
        foreach (PlayerMapAreas player in playerMapAreasInsideList)
        {
            if (player.team != _firstPlayerTeam.team) return;
        }


        if (_currentTeamColor != null && _firstPlayerTeam.team != _currentTeamColor.team && _progress >= 0f)
        {
            if (_firstPlayerTeam.objBuff)
            {
                _progress -= playerMapAreasInsideList.Count * progressSpeed * Time.deltaTime * _captureMultiplier;
            }
            else
            {
                _progress -= playerMapAreasInsideList.Count * progressSpeed * Time.deltaTime;
            }
        }
        else
        {
            _currentTeamColor = _firstPlayerTeam;
            if (_firstPlayerTeam.objBuff)
            {
                _progress += playerMapAreasInsideList.Count * progressSpeed * Time.deltaTime * _captureMultiplier;
            }
            else
            {
                _progress += playerMapAreasInsideList.Count * progressSpeed * Time.deltaTime;
            }
                    
            foreach (MapAreaCollider mapAreaCollider in _mapAreaColliders)
            {
                if (playerMapAreasInsideList.Count > 0)
                {
                    mapAreaCollider.GetComponent<Renderer>().material.color =
                        Color.Lerp(mapAreaCollider.GetComponent<Renderer>().material.color, _firstPlayerTeam.playerColor, _progress);
                }
            }
        }
        Debug.Log("Players inside map: " + playerMapAreasInsideList.Count + "; progress: " + _progress);
    }
}
