using System.Collections.Generic;
using UnityEngine;

public class NumberOfPlayers : MonoBehaviour
{
    [HideInInspector] public static List<PlayerMapAreas> AllPlayers = new List<PlayerMapAreas>();
    
    void Start()
    {
        foreach (PlayerMapAreas player in GetComponentsInChildren<PlayerMapAreas>())
        {
            AllPlayers.Add(player);
        }
    }
}
