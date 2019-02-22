using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星を置く場所の情報を保持する
/// </summary>
public class StarPlace : MonoBehaviour
{
    [System.NonSerialized]
    public Vector3 Pos = Vector3.zero;      // 位置
    [System.NonSerialized]
    public bool isActive = false;           // 星を置けるようになるフラグ
    [System.NonSerialized]
    public bool isSet = false;              // 星をセットしたフラグ

    void Awake()
    {
        Pos = transform.position;
        isActive = false;
        isSet = false;
    }
}
