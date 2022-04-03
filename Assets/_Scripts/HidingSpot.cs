using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    // relative to hiding spot
    [SerializeField] private Vector2 exitPoint = new Vector2(1f, 0f);
    
    public Vector2 GetExitPoint()
    {
        return exitPoint;
    }
}
