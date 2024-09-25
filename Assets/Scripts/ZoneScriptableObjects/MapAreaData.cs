using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapAreaData", menuName = "ScriptableObjects/MapAreaData")]
public class MapAreaData : ScriptableObject {

    [Header("Zone Info")]
    public int zoneTier;

    [Header("Zone Capture Data")]
    public float captureSpeed;
    public float captureDecayAutomatic;
    public float captureDecayUnderEnemy;
    public float captureRegainSpeed;

    [Header("Gold Generation")]
    public float goldPerSecond;
}
