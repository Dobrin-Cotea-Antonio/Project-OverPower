using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder {
    public abstract bool MoveTowardsTarget(Vector3 pTargetPosition, float pAgentSpeed);
    public abstract void Stop();
}
