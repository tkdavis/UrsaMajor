using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform aimTarget;
    public float fireRate = 1f;
    private bool firePressed;
    private float fireTimer = 0.0f;

    void Start()
    {
        
    }

    void Update()
    {
        GameObject newLaser;
        firePressed = Input.GetAxis("Fire1") != 0;
        fireTimer -= Time.deltaTime;
        if (firePressed && fireTimer <= 0) {
            newLaser = GameObject.Instantiate(laserPrefab);
            newLaser.transform.position = transform.position;
            newLaser.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Time.deltaTime);
            fireTimer = fireRate;
        }
    }
}