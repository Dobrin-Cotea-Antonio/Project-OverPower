using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshPathfinder : MonoBehaviour, IPathFinder {
    public Action OnDeadzoneMoveEnd;

    NavMeshAgent agent;
    PlayerController playerController;
    PlayerData data;

    Vector3 targetPosition;
    float agentSpeed;
    float distanceDeadzone;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        playerController = GetComponent<PlayerController>();
        data = GetComponent<PlayerData>();
    }

    public void Update() {
        if (data.isStunned)
            ClearCommand();

        if (distanceDeadzone == 0)
            return;

        if ((targetPosition - transform.position).magnitude >= distanceDeadzone)
            return;

        Stop();
        OnDeadzoneMoveEnd?.Invoke();
        playerController.ClearDeadzone();
    }

    public void MoveTowardsTarget(Vector3 pTargetPosition, float pAgentSpeed, float pDistanceDeadzone = 0) {
        OnDeadzoneMoveEnd = null;

        agent.speed = pAgentSpeed;
        agent.SetDestination(pTargetPosition);

        targetPosition = pTargetPosition;
        agentSpeed = pAgentSpeed;
        distanceDeadzone = pDistanceDeadzone;
    }

    public void Stop() {
        agent.SetDestination(transform.position);
    }

    public void ClearCommand(bool pStopMovingTowardsTarget = true) {
        if (pStopMovingTowardsTarget)
            Stop();
        playerController.ClearDeadzone();
        OnDeadzoneMoveEnd = null;
    }
}
