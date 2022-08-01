using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleLockController : MonoBehaviour
{
    public Transform target;
    public Vector3 finalScale = Vector3.one * 6.0f;
    Camera mainCamera;
    float currentRotationZ;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (transform.localScale.x > finalScale.x)
        {
            transform.localScale = new Vector3(transform.localScale.x - 400f * Time.deltaTime, transform.localScale.y - 400f * Time.deltaTime, transform.localScale.z);
        } else if (transform.localScale.x < finalScale.x)
        {
            transform.localScale = finalScale;
        }
        // should the LockOnManager handle this and decrement targetCount? ughhhh
        if (target)
        {
            transform.position = target.position;
        } else {
            GameObject.Destroy(gameObject);
        }

        Quaternion lookRotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position, Vector3.up);
        currentRotationZ = transform.rotation.eulerAngles.z;
        Quaternion finalRotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, currentRotationZ += 360 * Time.deltaTime);
        transform.rotation = finalRotation;
    }
}
