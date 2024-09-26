using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOEData", menuName = "ScriptableObjects/AOEData")]
public class AOEData : ScriptableObject {
    [Header("Stats")]
    public float damage;
    public float delay;
    public Vector2 aoeSize;

    [Header("Effects")]
    public List<GameObject> statusEffectsOnHit;//used for status effects like burn, slow, stun etc.
    public List<GameObject> additionalActionsOnHit;//used for spawning addtional effects like an aoe zone that deals damage etc.
}
