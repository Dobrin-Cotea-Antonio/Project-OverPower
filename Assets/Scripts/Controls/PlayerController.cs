using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPathFinder))]
public class PlayerController : MonoBehaviour {

    [SerializeField] LayerMask uiMask;
    [SerializeField] Camera playerCamera;

    IPathFinder pathfinder;
    Vector3 targetLocation;

    private void Start() {
        pathfinder = GetComponent<IPathFinder>();
        targetLocation = transform.localPosition;
    }

    private void Update() {
        pathfinder.MoveTowardsTarget(targetLocation, 10);
    }

    void OnPlayerMoveCommand() {
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (isOverUI)
            return;

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            targetLocation = hit.point;
        }
    }
}
