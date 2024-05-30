using System.Collections.Generic;
using UnityEngine;

public class ZoneTierManager : MonoBehaviour
{
    private List<MapArea> _zoneTier1 = new List<MapArea>();
    private List<MapArea> _zoneTier2 = new List<MapArea>();
    private List<MapArea> _zoneTier3 = new List<MapArea>();

    private bool _tier2Access;
    // private bool _tier3Access;

    private int zonesT2;
    private void Awake()
    {
        foreach (MapArea mapArea in GetComponentsInChildren(typeof(MapArea)))
        {
            switch (mapArea.zoneTier)
            {
                case 1:
                    _zoneTier1.Add(mapArea);
                    break;
                case 2:
                    _zoneTier2.Add(mapArea);
                    break;
                // case 3:
                //     _zoneTier3.Add(mapArea);
                //     break;
            }
        }
        
        Debug.Log(_zoneTier1.Count);
        Debug.Log(_zoneTier2.Count);

        foreach (MapArea zone2 in _zoneTier2)
        {
            zone2.enabled = false;
        }
    }

    private void Update()
    {
        zonesT2 = 0;
        
        foreach (MapArea zone1 in _zoneTier1)
        {
            if (zone1.state == MapArea.State.Captured) zonesT2++;
        
            if (zonesT2 == _zoneTier1.Count)
            {
                _tier2Access = true;
            }
            else
            {
                _tier2Access = false;
            }
        }

        if (!_tier2Access) return;
        foreach (MapArea zone2 in _zoneTier2)
        {
            Debug.Log("Happens");
            zone2.enabled = true;
        }

    }
}
