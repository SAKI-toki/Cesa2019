using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星の動き
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class StarController : MonoBehaviour
{
    [SerializeField]
    GameObject Player = null;

    Rigidbody Rigid;

    Vector3 PlayerPos = Vector3.zero;
    [SerializeField]
    float Dis = 0;
    [SerializeField]
    float Speed = 0;

    void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerPos = Player.transform.position;
        float distance = Vector3.Distance(transform.position, PlayerPos);

        if(distance < Dis)
        {
            Vector3 direction = (PlayerPos - transform.position).normalized;

            Rigid.AddForce(direction * Speed);
        }
    }
}
