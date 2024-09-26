using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject {
    [Header("Stats")]
    public float range;
    public float damage;
    public float speed;

    [Header("Effects")]
    public List<GameObject> statusEffectsOnHit;//used for status effects like burn, slow, stun etc.
    public List<GameObject> additionalActionsOnHit;//used for spawning addtional effects like an aoe zone that deals damage etc.
}
