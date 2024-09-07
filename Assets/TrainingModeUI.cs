using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TrainingModeUI : MonoBehaviour {

    //public static TrainingModeUI instance;

    [Header("UI Data")]
    [SerializeField] private Toggle dummyDeleteToggle;
    [SerializeField] private Image dummyDeleteImage;

    [Header("Data")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private LayerMask unplacebleLayerMask;
    [SerializeField] private LayerMask dummyLayerMask;

    [Header("Prefabs")]
    [SerializeField] private GameObject reticlePrefab;

    [Header("Reticle Materials")]
    [SerializeField] private Material placebleMaterial;
    [SerializeField] private Material unplacebleMaterial;

    [Header("Dummy Delete Button Colors")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;

    private MeshRenderer reticleMeshRenderer;
    private GameObject reticle = null;
    private GameObject targetSpawnPrefab = null;

    private Vector3 targetLocation = Vector3.zero;
    private bool canPlace = false;

    #region Unity Events
    private void Awake() {
        dummyDeleteToggle.isOn = false;
    }

    private void Update() {
        RaycastForTarget();
        UpdateReticlePosition();
    }
    #endregion

    #region Input
    private void OnPlayerLeftClick() {

        if (dummyDeleteToggle.isOn) {
            AtteptDeleteDummy();
            return;
        }

        if (reticle == null)
            return;

        if (!canPlace)
            return;

        Destroy(reticle);
        reticle = null;

        GameObject target = Instantiate(targetSpawnPrefab);

        target.transform.position = targetLocation;
        targetSpawnPrefab = null;
    }

    public void SpawnObjectReticle(GameObject pTargetSpawnPrefab) {
        if (reticle != null)
            return;

        dummyDeleteToggle.isOn = false;

        reticle = Instantiate(reticlePrefab);
        reticleMeshRenderer = reticle.GetComponentInChildren<MeshRenderer>(); ;

        reticle.transform.position = targetLocation;
        targetSpawnPrefab = pTargetSpawnPrefab;
    }
    #endregion

    #region Helper Methods
    private void UpdateReticlePosition() {
        if (reticle == null)
            return;

        reticle.transform.position = targetLocation;
    }

    private void RaycastForTarget() {
        RaycastHit hit;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, unplacebleLayerMask)) {
            if (reticleMeshRenderer != null)
                reticleMeshRenderer.material = unplacebleMaterial;

            canPlace = false;
        } else {
            if (reticleMeshRenderer != null)
                reticleMeshRenderer.material = placebleMaterial;

            canPlace = true;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask))
            targetLocation = hit.point;
    }
    #endregion

    private void AtteptDeleteDummy() {

        RaycastHit hit;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, dummyLayerMask)) {
            Debug.Log("test");

            PlayerData data = hit.collider.transform.root.gameObject.GetComponent<PlayerData>();

            if (!data.isDummy)
                return;

            data.gameObject.transform.position = new Vector3(0, -1000000, 0);

            Destroy(data.gameObject, 0.2f);
            dummyDeleteToggle.isOn = false;
        }
    }

    public void ChangeDummyDeleteColor(bool pState) {
        dummyDeleteImage.color = (dummyDeleteToggle.isOn) ? selectedColor : normalColor;
    }
}
