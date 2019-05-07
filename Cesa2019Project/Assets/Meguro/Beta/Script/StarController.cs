using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星の動き
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class StarController : MonoBehaviour
{
    GameObject Player = null;           // プレイヤー
    Rigidbody Rigid;                    // 星の欠片のリジッドボディ
    Vector3 PlayerPos = Vector3.zero;   // プレイヤー位置
    [SerializeField]
    float Dis = 0;                      // プレイヤーとの距離がこの長さより小さかったら追尾
    [SerializeField]
    float Speed = 0;                    // 星の欠片の追尾スピード
    [SerializeField]
    float AwakeTime = 0;
    float time = 0;

    void Awake()
    {
        Player = GameObject.Find("Player");
        Rigid = GetComponent<Rigidbody>();
        Vector3 playerForward = Vector3.Scale(Player.transform.forward, new Vector3(1, 0, 1)).normalized;
        Rigid.AddForce(
            new Vector3(
                playerForward.x * Random.Range(-9, 10),
                Random.Range(5, 8),
                playerForward.z * Random.Range(-9, 10)),
            ForceMode.Impulse);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (AwakeTime < time)
        {
            PlayerPos = Player.transform.position;
            float distance = Vector3.Distance(transform.position, PlayerPos);

            if (distance < Dis)
            {
                Vector3 direction = (PlayerPos - transform.position).normalized;

                Rigid.AddForce(direction * Speed);
            }
        }
    }
}
