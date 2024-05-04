using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder {
    public abstract void MoveTowardsTarget(Vector3 pTargetPosition, float pAgentSpeed, float pDistanceDeadzone = 0);
    public abstract void Stop();
}
