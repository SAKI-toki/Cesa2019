using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour
{
    const float MaxRandomRange = 0.3f;
    const float MinRandomRange = 0.1f;
    float RotationTime = 0.0f;
    float TimeCount = 0.0f;
    Quaternion PrevQuaternion = new Quaternion();
    Quaternion NextQuaternion = new Quaternion();
    private void Start()
    {
        RandomRotation();
    }

    void Update()
    {
        TimeCount += Time.deltaTime;
        if (TimeCount > RotationTime)
        {
            RandomRotation();
        }
    }
    void RandomRotation()
    {
        RotationTime = Random.Range(MinRandomRange, MaxRandomRange);
        PrevQuaternion = transform.rotation;
        Vector3 look = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        if (look == Vector3.zero)
        {
            look.x = 1.0f;
        }
        NextQuaternion = Quaternion.LookRotation(look);
    }
}
