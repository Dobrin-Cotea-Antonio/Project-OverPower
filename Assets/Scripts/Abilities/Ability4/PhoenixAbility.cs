using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhoenixAbility : AbilityBase {
    [Header("Data")]
    [SerializeField] GameObject aoeOvertimePrefab;
    [SerializeField] GameObject burnEffectPrefab;
    [SerializeField] PlayerData owner;
    [SerializeField] float damage;
    [SerializeField] float flameLifetime;
    [SerializeField] float timeUntilBurnStack;
    [SerializeField] int segmentCount;
    [SerializeField] Vector2 aoeSegmentSize;

    List<AoeOverTimePhoenix> segments=new List<AoeOverTimePhoenix>();
    List<PlayerData> targets = new List<PlayerData>();
    Dictionary<PlayerData, float> targetTimeUntilBurn = new Dictionary<PlayerData, float>();

    bool canUpdateSegments;

    protected override void Update() {
        base.Update();
        UpdateSegments();
    }

    #region Ability
    protected override void AbilityEffect() {
        base.AbilityEffect();
        StartCoroutine(AbilityCoroutine());
    }

    IEnumerator AbilityCoroutine() {

        Vector3 targetLocation = GetTargetClickLocation();
        Vector3 moveDirection = (targetLocation - transform.position).normalized*aoeSegmentSize.y;
        moveDirection.y = 0;

        float delayPerSegment = abilityData.abilityDuration / segmentCount;
        Vector3 segmentForward = (targetLocation - transform.position).normalized;

        canUpdateSegments = true;

        for (int i = 0; i < segmentCount; i++) {
            GameObject g = Instantiate(aoeOvertimePrefab, targetLocation + i * moveDirection, Quaternion.identity);

            g.transform.forward = segmentForward;
            AoeOverTimePhoenix aoeOverTime = g.GetComponent<AoeOverTimePhoenix>();
            segments.Add(aoeOverTime);
            aoeOverTime.SetSize(aoeSegmentSize);
            Destroy(g, flameLifetime + (segmentCount - i) * delayPerSegment);
            yield return new WaitForSeconds(delayPerSegment);
        }

        yield return new WaitForSeconds(flameLifetime);

        canUpdateSegments = false;
        segments.Clear();
        targets.Clear();
        targetTimeUntilBurn.Clear();
    }

    void UpdateSegments() {
        if (!canUpdateSegments)
            return;

        List<PlayerData> targetsUpdate=new List<PlayerData>();

        foreach (PlayerData data in targets) {
            targetTimeUntilBurn[data] += Time.deltaTime;
            data.TakeDamage(damage * Time.deltaTime);
            //attepts to burn the targets
            if (targetTimeUntilBurn[data] >= timeUntilBurnStack) {
                data.ApplyStatusEffect(burnEffectPrefab);
                targetTimeUntilBurn[data] = 0;
                Debug.Log("applied burn");
            }
        }

        //add all targets from each segment to a list
        foreach (AoeOverTimePhoenix segment in segments) {
            foreach (PlayerData data in segment.ReturnTargetList()) {
                if (targetsUpdate.Contains(data))
                    continue;
                targetsUpdate.Add(data);
            }
        }

        //removes targets from the list that are not in the new list
        foreach (PlayerData data in targets) {
            if (targetsUpdate.Contains(data))
                continue;
            targets.Remove(data);
        }

        //adds targets that are not in the old list
        foreach (PlayerData data in targetsUpdate) {
            if (targets.Contains(data))
                continue;
            targets.Add(data);
            targetTimeUntilBurn[data] = 0;
        }
    }
    #endregion
}
