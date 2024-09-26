using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData")]
public class AbilityData : ScriptableObject {

    [Header("Ability Data")]
    public Sprite iconImage;
    public float abilityCooldown;
    public float abilityDuration;
    public float abilityRange;
    public string description;
    //ADD ABILITY CAST TIME

    [Header("Charges")]
    public int maxCharges;
    public float chargeCooldown;
}
