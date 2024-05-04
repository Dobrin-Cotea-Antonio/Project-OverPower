using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StatusEffects", menuName = "StatusEffect")]
public class StatusEffectData : ScriptableObject {

    public float value;
    public float duration;
    public Sprite image;

    public bool canStack;
    public int maxStackCount;
}
