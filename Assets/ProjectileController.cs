using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 60f;
    public GameObject explosionPrefab;
    public PlayerController playerController;
    private float deathTimer = 4f;
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0.0f) {
            Object.Destroy(gameObject);
        }
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other) {
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     Debug.Log("PLAYER HIT BY PROJECTILE!!!");
        //     playerController.TakeDamage();
        // } else
        // {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // }
        Object.Destroy(gameObject);
    }
}
