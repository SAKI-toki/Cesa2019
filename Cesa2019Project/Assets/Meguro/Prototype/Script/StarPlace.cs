﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星を置く場所の情報を保持する
/// </summary>
public class StarPlace : MonoBehaviour
{
    public GameObject Star = null;
    [System.NonSerialized]
    public Vector3 Pos = Vector3.zero;      // 位置
    [System.NonSerialized]
    public bool isActive = false;           // 星を置けるようになるフラグ
    [System.NonSerialized]
    public bool isSet = false;              // 星をセットしたフラグ
    [SerializeField,Header("最初からセットしておくかどうか")]
    public bool IsAwakeSet = false;
    void Awake()
    {
        Pos = transform.position;
        if(IsAwakeSet)
        {
            isActive = true;
            isSet = true;
        }
        else
        {
            isActive = false;
            isSet = false;
        }
    }
}
