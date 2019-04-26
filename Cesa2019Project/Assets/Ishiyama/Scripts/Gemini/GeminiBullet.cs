using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiBullet : MonoBehaviour
{
    const float BulletSpeed = 10.0f;
    const float LifeTime = 10.0f;

    float LifeTimeCount = 0.0f;

    void Update()
    {
        transform.position += transform.forward * BulletSpeed * Time.deltaTime;
        LifeTimeCount += Time.deltaTime;
        if (LifeTimeCount > LifeTime) Destroy(gameObject);
    }
}
