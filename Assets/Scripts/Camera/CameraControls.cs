using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraControls : MonoBehaviour {
    public enum CameraModes { 
        Freemove,
        Follow
    }

    [Header("Speed")]
    [SerializeField] float cameraMovementSpeed = 1;
    [SerializeField] float cameraZoomSpeed = 100;

    [Header("Zoom Values")]
    [SerializeField] float minZoomDistance = 5;
    [SerializeField] float maxZoomDistance = 20;

    [Header("Camera Border Size")]
    [SerializeField] Vector2 borderValue;//in percentages

    [Header("Parent Transform")]
    [SerializeField] Transform parent;

    [Header("Player Transform")]
    [SerializeField] Transform playerTransform;

    Vector3 right;
    Vector3 forward;
    Vector3 moveDirection;

    CameraModes mode = CameraModes.Follow;

    private void Start() {
        parent.SetParent(null);
        FindAbsoluteDirections();
        transform.Translate(new Vector3(0, 0, -maxZoomDistance));
        ClampCameraZoom();
    }

    private void Update() {
        if (!(mode == CameraModes.Freemove)) {
            parent.position = new Vector3(playerTransform.position.x, parent.position.y, playerTransform.position.z);
            return;
        }

        MoveCameraWithBorder();
        parent.Translate(moveDirection * Time.deltaTime, Space.World);
    }

    #region Camera Edge Move Methods
    void MoveCameraWithBorder() {
        Vector2 cursorSide = CheckForBorder();
        parent.Translate(cameraMovementSpeed * Time.deltaTime * (cursorSide.y * forward + cursorSide.x * right), Space.World);
    }

    Vector2 CheckForBorder() {
        Vector2 cursorSide = Vector2.zero;

        Vector3 mousePosition = Input.mousePosition;

        Rect leftRect = new Rect(0, 0, Screen.width * (borderValue.x / 100f), Screen.height);
        Rect rightRect = new Rect(Screen.width - Screen.width * (borderValue.x / 100f), 0, Screen.width * (borderValue.x / 100f), Screen.height);
        Rect upRect = new Rect(0, Screen.height - Screen.height * (borderValue.y / 100f), Screen.width, Screen.height * (borderValue.y / 100f));
        Rect downRect = new Rect(0, 0, Screen.width, Screen.height * (borderValue.y / 100f));

        cursorSide.x = leftRect.Contains(mousePosition) ? -1 : rightRect.Contains(mousePosition) ? 1 : 0;
        cursorSide.y = upRect.Contains(mousePosition) ? 1 : downRect.Contains(mousePosition) ? -1 : 0;

        return cursorSide;
    }
    #endregion

    #region Input Events
    void OnMoveCamera(InputValue pInputValue) {
        Vector2 direction = pInputValue.Get<Vector2>() * cameraMovementSpeed;
        moveDirection = direction.y * forward + direction.x * right;
    }

    void OnScroll(InputValue pInputValue) {
        Vector2 dir = pInputValue.Get<Vector2>().normalized;
        transform.Translate(new Vector3(0, 0, dir.y * Time.deltaTime * cameraZoomSpeed), Space.Self);
        ClampCameraZoom();
    }

    void OnChangeCameraMode() {
        if (mode == CameraModes.Follow)
            mode = CameraModes.Freemove;
        else
            mode = CameraModes.Follow;
    }

    void OnCenterCameraOnPlayer() {
        parent.position = new Vector3(playerTransform.position.x, parent.position.y, playerTransform.position.z);
    }
    #endregion

    #region Camera Zoom Helper Methods
    float ReturnDistanceToGround() {
        return (transform.position - parent.position).magnitude;
    }

    void ClampCameraZoom() {
        float distance = ReturnDistanceToGround();

        if (distance == 0)
            return;

        if (distance < minZoomDistance) {
            transform.Translate(0, 0, distance - minZoomDistance, Space.Self);
            return;
        }

        if (distance > maxZoomDistance) {
            transform.Translate(0, 0, distance - maxZoomDistance, Space.Self);
            return;
        }
    }
    #endregion

    #region Additional Helper Methods
    void FindAbsoluteDirections() {
        Quaternion cameraRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        right = transform.right;
        forward = transform.forward;
        transform.rotation = cameraRotation;
    }
    #endregion
}
