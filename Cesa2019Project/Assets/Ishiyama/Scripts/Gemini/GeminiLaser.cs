using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiLaser : MonoBehaviour
{
    Vector3 Offset = new Vector3(0, 2, 0);
    const float LaserSize = 100.0f;
    const float LifeTime = 2.0f;

    float LifeTimeCount = 0.0f;
    void Start()
    {
        transform.localScale = new Vector3(1, 1, LaserSize);
    }

    void Update()
    {
        LifeTimeCount += Time.deltaTime;
        if (LifeTimeCount > LifeTime) Destroy(gameObject);
    }

    public void LaserInit(Vector3 initPos)
    {
        transform.position = transform.forward * LaserSize + Offset + initPos;
    }
}
