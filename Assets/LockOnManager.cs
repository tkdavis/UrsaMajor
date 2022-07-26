using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnManager : MonoBehaviour
{
    public Camera currentCamera;
    public GameObject reticleLockOnPrefab;
    public float randomDelayMax = 4.0f;
    public int maxTargets = 4;
    int targetCount = 0;
    bool missileBtnPressed;
    float randomDelay;
    bool onScreen;
    Vector3 screenPoint;
    GameObject[] enemies;
    EnemyController enemyController;

    void Update()
    {
        missileBtnPressed = Input.GetButton("Fire2");

        UpdateLockOnReticles();
    }

    void UpdateLockOnReticles()
    {
        // this is expensive call for update, let's try to do it only on button pressed instead of update.
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemyController = enemy.GetComponent<EnemyController>();
            CheckEnemyIsOnScreen(enemy);
        
            if (missileBtnPressed && onScreen && !enemyController.reticle)
            {
                CreateReticle(enemy);
            } else if (enemyController.reticle && (!missileBtnPressed || !onScreen))
            {
                DestroyReticle();
            }
        }
    }

    void CheckEnemyIsOnScreen(GameObject enemy)
    {
        screenPoint = currentCamera.WorldToViewportPoint(enemy.transform.position);
        onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    void CreateReticle(GameObject enemy)
    {
        randomDelay = Random.Range(0.0f, randomDelayMax);
        StartCoroutine(DelayInstantiateCoroutine(randomDelay, enemy));
    }

    void DestroyReticle()
    {
        targetCount -= 1;
        Object.Destroy(enemyController.reticle);
        enemyController.reticle = null;
    }

    IEnumerator DelayInstantiateCoroutine(float delay, GameObject enemy)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (!enemy)
        {
            // Debug.LogWarning("Enemy is null after delayed lockon.");
        } else if (missileBtnPressed && !enemy.GetComponent<EnemyController>().reticle && targetCount < maxTargets)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            targetCount += 1;
            GameObject newReticle = Instantiate(reticleLockOnPrefab);
            newReticle.GetComponent<ReticleLockController>().target = enemy.transform;
            enemyController.reticle = newReticle;
            enemyController.markedForTracking = true;
        }
    }
}
