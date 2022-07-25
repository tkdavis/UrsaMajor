using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleController : MonoBehaviour
{
    public bool isReticleNear = false;
    public Transform playerTransform;
    public float distanceOffset = 10.0f;
    public float rotationSpeed = 0.0f;

    void Start()
    {
        if (!isReticleNear)
        {
            distanceOffset = 20.0f;
        }
    }


    void Update()
    {
        transform.position = playerTransform.position + (distanceOffset * playerTransform.forward);
        transform.rotation = playerTransform.rotation;
    }
}
