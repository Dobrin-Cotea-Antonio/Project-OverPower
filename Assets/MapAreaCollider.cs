using System.Collections.Generic;
using UnityEngine;

public class MapAreaCollider : MonoBehaviour
{
    
    private List<PlayerMapAreas> _playerMapAreasList = new List<PlayerMapAreas>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMapAreas>(out PlayerMapAreas playerMapAreas))
        {
            _playerMapAreasList.Add(playerMapAreas);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMapAreas>(out PlayerMapAreas playerMapAreas))
        {
            _playerMapAreasList.Remove(playerMapAreas);
        }
    }

    public List<PlayerMapAreas> GetPlayerAreasList()
    {
        return _playerMapAreasList;
    }
}
