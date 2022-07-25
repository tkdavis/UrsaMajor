using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnManager : MonoBehaviour
{
    public Camera currentCamera;
    public GameObject reticleLockOnPrefab;
    public float randomDelayMax = 4.0f;
    float randomDelay;
    bool onScreen;
    Vector3 screenPoint;
    GameObject[] enemies;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            screenPoint = currentCamera.WorldToViewportPoint(enemy.transform.position);
            onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            if (onScreen && !enemy.GetComponent<EnemyController>().hasReticle)
            {
                randomDelay = Random.Range(0.0f, randomDelayMax);
                StartCoroutine(DelayInstantiate(randomDelay, enemy));
                enemy.GetComponent<EnemyController>().hasReticle = true;
            }
        }
    }

    IEnumerator DelayInstantiate(float delay, GameObject enemy)
    {
        yield return new WaitForSecondsRealtime(delay);
        GameObject newReticle = Instantiate(reticleLockOnPrefab);
        newReticle.GetComponent<ReticleLockController>().target = enemy.transform;
    }
}
