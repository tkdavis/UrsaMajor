using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float speed = 8;
    public Transform aimTarget;
    public Transform tiltTarget;
    public float lookSpeed = 0.01f;
    public float maxHealth = 15.0f;
    public float currentHealth = 15.0f;
    public GameObject fireParticleObj;
    public GameObject fireParticleObj2;
    public GameObject fireParticleObj3;
    public GameObject CineVCamObj;
    private CinemachineVirtualCamera CineVCam;
    private ParticleSystem fireDamageEffect;
    private ParticleSystem fireDamageEffect2;
    private ParticleSystem fireDamageEffect3;
    private Rigidbody rb;
    private float hMove;
    private float vMove;
    private float lerpTime = 0.02f;
    private float leanLimit = 80.0f;
    private bool isDead = false;
    private float restartSceneTimer = 6.0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        CineVCam = CineVCamObj.GetComponent<CinemachineVirtualCamera>();
        fireDamageEffect = fireParticleObj.GetComponent<ParticleSystem>();
        fireDamageEffect2 = fireParticleObj2.GetComponent<ParticleSystem>();
        fireDamageEffect3 = fireParticleObj3.GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (isDead)
        {
            restartSceneTimer -= Time.deltaTime;

            if (restartSceneTimer <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                currentHealth = maxHealth;
                isDead = false;
            }
            return;
        }

        hMove = Input.GetAxis("Horizontal");
        vMove = Input.GetAxis("Vertical");

        LocalMove();
        ClampPosition();
        RotationLook();
        HorizontalLean();
    }

    void LocalMove()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + new Vector3(hMove, vMove, 0) * speed, Time.deltaTime * 0.9f);
    }

    void ClampPosition()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
        position.x = Mathf.Clamp01(position.x);
        position.y = Mathf.Clamp01(position.y);
        position.z = 8.0f;
        transform.position = Vector3.Lerp(transform.position, Camera.main.ViewportToWorldPoint(position), 13.0f * Time.deltaTime);
    }

    void RotationLook()
    {
        aimTarget.localPosition = new Vector3(hMove, vMove, 4);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * lookSpeed * Time.deltaTime);
    }

    void HorizontalLean()
    {
        Vector3 targetEulerAngles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(Mathf.LerpAngle(targetEulerAngles.x, -vMove * leanLimit, lerpTime), Mathf.LerpAngle(targetEulerAngles.y, hMove * leanLimit, lerpTime / 4), Mathf.LerpAngle(targetEulerAngles.z, -hMove * leanLimit, lerpTime));
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Missile"))
        {
            return;
        }

        if (currentHealth <= 0)
        {
            PlayerDeath();
        }

        TakeDamage();
    }

    public void TakeDamage()
    {
        currentHealth -= 1;
        CatchFire();
    }

    void CatchFire()
    {
        if (currentHealth < Mathf.Floor(maxHealth / 4))
        {
            fireDamageEffect.Play();
        } else if (currentHealth < Mathf.Floor(maxHealth / 2))
        {
            fireDamageEffect2.Play();
        } else
        {
            fireDamageEffect3.Play();
        }
    }

    void PlayerDeath()
    {
        transform.parent = null;
        rb.constraints = RigidbodyConstraints.None;
        rb.angularDrag = 0.0f;
        rb.AddTorque((transform.forward) * 300.0f);
        rb.AddForce(transform.forward * 8.0f, ForceMode.Impulse);
        isDead = true;
        CineVCam.m_LookAt = transform;
    }
}
