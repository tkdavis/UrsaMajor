using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float lifeTime = 10.0f;
    public GameObject lightObj;
    private Light lightExplosion;
    // Start is called before the first frame update
    void Start()
    {
        lightExplosion = lightObj.GetComponent<Light>();
    }

    // Update is called once per frame
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
