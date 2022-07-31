using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform aimTarget;
    public float fireRate = 1f;
    public string fireButton = "Fire1";
    public float delay;
    private bool firePressed;
    private float fireTimer = 0.0f;
    private GameObject newLaser;

    void Start()
    {
        
    }

    void Update()
    {
        if (fireButton == "Fire2")
        {
            // Missiles need lockon first, so fire on release.
            firePressed = Input.GetButtonUp(fireButton);
        } else {
            firePressed = Input.GetAxis(fireButton) != 0;
        }

        fireTimer -= Time.deltaTime;
        if (firePressed && fireTimer <= 0) {
            newLaser = GameObject.Instantiate(laserPrefab);
            newLaser.transform.position = transform.position;
            newLaser.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Time.deltaTime);
            fireTimer = fireRate;
        }
    }
}
