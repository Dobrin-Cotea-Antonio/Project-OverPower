using System;
using UnityEngine;

public class PlayerMapAreas : MonoBehaviour
{
    public Color playerColor;
    [HideInInspector] public bool objBuff = false;
    
    public enum Team
    {
        Red,
        Blue,
        Green
    }

    public Team team;

    private Color _mat;
    
    void Awake()
    { 
        _mat = GetComponent<Renderer>().material.color = playerColor;
    }

    //Debug
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _mat = GetComponent<Renderer>().material.color = playerColor;
        }
    }
}
