using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float lifeTime = 10.0f;
    public GameObject lightObj;
    private Light lightExplosion;

    void Start()
    {
        lightExplosion = lightObj.GetComponent<Light>();
    }

    void Update()
    {
        lightExplosion.intensity -= 50 * Time.deltaTime;
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Object.Destroy(gameObject);
        }
    }
}
