using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private LayerMask uiMask;
    [SerializeField] private Camera playerCamera;

    [Header("Data")]
    [SerializeField] private float playerMovementSpeed;

    private IPathFinder pathfinder;
    private PlayerData data;

    private Vector3 targetWalkLocation;
    private Vector3 targetClickLocation;
    private float distanceDeadzone;

    private Rigidbody rb;

    private Vector3 right;
    private Vector3 forward;
    private Vector3 moveDirection;

    #region Unity Events
    private void Start() {
        data = GetComponent<PlayerData>();
        pathfinder = GetComponent<IPathFinder>();
        targetWalkLocation = transform.localPosition;
        rb = GetComponent<Rigidbody>();
        FindAbsoluteDirections();
    }

    private void Update() {
        rb.velocity = moveDirection;
    }
    #endregion

    #region Events
    void OnPlayerMoveCommand() {
        //MoveTowardsPoint(true);
    }

    void OnMovePlayer(InputValue pInputValue) {
        Vector2 direction = pInputValue.Get<Vector2>() * playerMovementSpeed;
        moveDirection = direction.y * forward + direction.x * right;
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
    void FindAbsoluteDirections() {
        Quaternion cameraRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        right = transform.right;
        forward = transform.forward;
        transform.rotation = cameraRotation;
    }
    #endregion
}
