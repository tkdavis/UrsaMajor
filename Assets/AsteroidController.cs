using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameObject brokenAsteroidPrefab;
    private Rigidbody rb;
    private float speed = 0.5f;
    private Vector3 torque = Vector3.zero;
     
    void Awake()
    {
        torque.x = Random.Range (-100, 100);
        torque.y = Random.Range (-100, 100);
        torque.z = Random.Range (-100, 100);
        speed = Random.Range(0.0f, 0.5f);
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Random.onUnitSphere * speed;
        rb.AddTorque(torque);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Projectile"))
        {
            GameObject brokenAsteroidObj = Instantiate(brokenAsteroidPrefab, transform.position, transform.rotation);
            brokenAsteroidObj.transform.localScale = transform.localScale;
            brokenAsteroidObj.transform.DetachChildren();
            Object.Destroy(gameObject);
        }
    }
}
