using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiLaser : MonoBehaviour
{
    const float LaserSize = 100.0f;
    const float LifeTime = 10.0f;

    float LifeTimeCount = 0.0f;
    void Start()
    {
        transform.localScale = new Vector3(1, LaserSize, 1);
    }

    void Update()
    {
        LifeTimeCount += Time.deltaTime;
        if (LifeTimeCount > LifeTime) Destroy(gameObject);
    }

    public void LaserInit()
    {
        transform.position += transform.forward * LaserSize / 2;
    }
}
