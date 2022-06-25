using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFeedDirection : MonoBehaviour
{
    [SerializeField] public Spoon spoon;
    LineRenderer lineRenderer;

    // Number of points on the line


    // distance between those points on the line
    public float timeBetweenPoints = 0.1f;

    // The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;
    void Start()
    {
        lineRenderer = spoon.line;

    }


    void Update()
    {
        
    }
}
