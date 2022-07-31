using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleLockController : MonoBehaviour
{
    public Transform target;
    public Vector3 finalScale = Vector3.one * 6.0f;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x > finalScale.x)
        {
            transform.localScale = new Vector3(transform.localScale.x - 400f * Time.deltaTime, transform.localScale.y - 400f * Time.deltaTime, transform.localScale.z);
        } else if (transform.localScale.x < finalScale.x)
        {
            transform.localScale = finalScale;
        }
        transform.position = target.position;
        transform.Rotate(0, 0, 540 * Time.deltaTime);
        transform.LookAt(mainCamera.transform);
    }
}
