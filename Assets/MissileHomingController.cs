using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHomingController : MonoBehaviour
{
    public GameObject target;
    public GameObject player;
    public GameObject explosionEffect;
    public float maxSpeed = 100;
    public float animeMin = 0;
    public float animeMax = 40;
    public GameObject[] enemiesLockedOn;
    GameObject[] enemies;
    float randomNum;
    Vector3 randomDir;
    Rigidbody rb;
    bool hasExploded;
    float animeCircleFrequency;
    float animeCircleRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        TargetLockedOnEnemy();

        randomNum = Random.Range(0,3);
        if (randomNum == 0)
        {
            randomDir = Vector3.down;
        } else if (randomNum == 1)
        {
            randomDir = Vector3.down;
        } else if (randomNum == 2)
        {
            randomDir = Vector3.down;
        } else {
            randomDir = Vector3.down;
        }
        hasExploded = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        RandomAnimeCircle();
    }

    private void Update() {
        if (Vector3.Distance(player.transform.position, transform.position) > 2000)
        {
            Explode();
        }
    }


    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        float currentDistFromTarget = Vector3.Distance(target.transform.position, transform.position);
        Vector3 direction;
        Vector3 desiredVelocity;
        Vector3 steeringForce;

        direction = (target.transform.position - transform.position).normalized;
        desiredVelocity = direction * maxSpeed;
        steeringForce = desiredVelocity - rb.velocity;
        rb.AddForce(steeringForce);
        rb.AddForce(transform.right * animeCircleRadius);
        transform.RotateAround(transform.position, transform.forward, animeCircleFrequency);
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!hasExploded && !other.gameObject.CompareTag("Missile") && !other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        hasExploded = true;
    }

    void RandomAnimeCircle()
    {
        float randomSign = Mathf.Sign(Random.Range(-1, 1));
        animeCircleFrequency = Random.Range(-10, 10);
        animeCircleRadius = Random.Range(animeMin, animeMax) * randomSign;
    }

    void TargetLockedOnEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            Debug.Log(enemyController.markedForTracking.ToString() + " : " + enemy.name);
            if (enemyController.markedForTracking)
            {
                enemyController.markedForTracking = false;
                target = enemy;
                Debug.LogWarning("Target set to: " + enemy.name);
                break;
            }
        }
    }
}
