using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPathFinder))]
public class PlayerController : MonoBehaviour {

    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] LayerMask uiMask;
    [SerializeField] Camera playerCamera;

    IPathFinder pathfinder;
    PlayerData data;

    Vector3 targetWalkLocation;
    Vector3 targetClickLocation;
    float distanceDeadzone;

    private void Start() {
        data = GetComponent<PlayerData>();
        pathfinder = GetComponent<IPathFinder>();
        targetWalkLocation = transform.localPosition;
    }

    #region Events
    void OnPlayerMoveCommand() {
        MoveTowardsPoint(true);
    }
    #endregion

    #region Pathfinding
    public void AbilityMoveCommand(float pDistanceDeadzone = 0) {
        distanceDeadzone = pDistanceDeadzone;
        MoveTowardsPoint();
    }

    public void ClearDeadzone() {
        targetWalkLocation = transform.position;
        distanceDeadzone = 0;
    }

    void MoveTowardsPoint(bool pResetDeadzone = false) {
        if (data.isStunned)
            return;

        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (isOverUI)
            return;

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask)) {
            targetWalkLocation = hit.point;
            targetClickLocation = hit.point;
        }

        if (pResetDeadzone)
            distanceDeadzone = 0;

        pathfinder.MoveTowardsTarget(targetWalkLocation, data.speed, distanceDeadzone);
    }
    #endregion

    #region Helper Methods
    public Vector3 ReturnTargetClickLocation() {
        return targetClickLocation;
    }
    #endregion
}
