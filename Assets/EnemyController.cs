using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform targetZoneTransform;
    public Transform playerTransform;
    public Transform leadTargetTransform;
    public float speed = 20.0f;
    public float maxSpeed = 24.0f;
    public float lookSpeed = 0.01f;
    public GameObject laserPrefab;
    public GameObject reticle;
    public bool markedForTracking = false;
    private bool inRangeOfPlayer = false;
    private bool isAvoiding = false;
    private bool isAttacking = false;
    private bool wasShotDown = false;
    private bool isDisabled = false;
    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;
    private float throttle = 1.0f;
    private Transform targetTransform;

    /**
      Enemy States:
      Explore
      Avoid
      Chase
      Attack
      (Later try Retreat?)
    **/

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        targetTransform = targetZoneTransform;
    }

    void Update()
    {
        inRangeOfPlayer = true;

        if (!wasShotDown && !isAttacking)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-rb.velocity), Time.deltaTime * 4);
        } else if (isAttacking)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - leadTargetTransform.position), Time.deltaTime * 4);
        }
    }

    void FixedUpdate()
    {
        if (wasShotDown && !isDisabled)
        {
            rb.angularDrag = 0.0f;
            rb.AddTorque((transform.forward) * 300.0f);
            moveDirection = rb.velocity.normalized;
            isDisabled = true;
        } else if (inRangeOfPlayer && !isAvoiding && !isAttacking)
        {
            ChasePlayer();
        } else if (isAttacking && !isAvoiding)
        {
            Attack();
            // temp vv
            // ChasePlayer();
        }

        rb.AddForce(moveDirection * speed * throttle);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TargetZone"))
        {
            isAttacking = true;
        } else if (other.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
        }
    }

    void OnTriggerStay(Collider other) 
    {
        if (!other.gameObject.CompareTag("Projectile"))
        {
            Avoid(other);
            isAvoiding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        isAvoiding = false;
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Missile"))
        {
            wasShotDown = true;
            markedForTracking = false;
            GameObject.Destroy(gameObject, 3.0f);
        }
    }

    void ChasePlayer()
    {
        if (wasShotDown) return;
        targetTransform = targetZoneTransform;
        moveDirection = (targetTransform.position - transform.position).normalized;
        throttle = 1.0f;
    }

    void Avoid(Collider other)
    {
        if (wasShotDown) return;
        moveDirection = ((transform.position + targetTransform.position) - other.transform.position * 2.0f).normalized;
        throttle = 3.0f;
    }

    void Attack()
    {
        if (wasShotDown) return;
        targetTransform = playerTransform;
        moveDirection = (targetTransform.position - transform.position).normalized;

        if (Random.Range(0.0f, 1.0f) > 0.9f)
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject newLaser = GameObject.Instantiate(laserPrefab);
        newLaser.transform.position = transform.position - transform.forward * 4;
        newLaser.transform.rotation = Quaternion.LookRotation(-transform.forward);
    }
}
