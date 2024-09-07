using UnityEngine;

public class ZoneTier4 : MonoBehaviour
{
    public TimeManager timeManager;
    public float spawnTime = 60f;
    
    private MapArea _mapArea;
    void Awake()
    {
        _mapArea = GetComponent<MapArea>();
        SetZone(false);
    }

    void Update()
    {
        if (timeManager.currentTime >= spawnTime)
        {
            SetZone(true);
        }
    }

    public void SetZone(bool state)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
