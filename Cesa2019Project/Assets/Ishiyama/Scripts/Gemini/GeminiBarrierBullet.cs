using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiBarrierBullet : MonoBehaviour
{
    const float MoveSpeed = 50.0f;
    Vector3 MoveVector = new Vector3();
    public void GeminiBarrierBulletInit(Vector3 moveVector)
    {
        MoveVector = moveVector.normalized;
    }

    void Update()
    {
        transform.position += MoveSpeed * MoveVector * Time.deltaTime;
    }
}
