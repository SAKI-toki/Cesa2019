using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ぐるぐる回るエフェクト
/// </summary>
public class SummonEffect : MonoBehaviour
{
    [SerializeField, Header("生成するエフェクト")]
    GameObject EffectObject = null;
    [SerializeField, Header("生成する数")]
    int GenerateNum = 3;
    [SerializeField, Header("中心からの距離")]
    float CenterDistance = 1.0f;
    [SerializeField, Header("回転速度")]
    float RotationSpeed = 3.0f;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        float angle = 0.0f;
        float angleAdd = Mathf.PI * 2 / GenerateNum;
        for (int i = 0; i < GenerateNum; ++i)
        {
            GameObject effectObj = Instantiate(EffectObject, transform);
            effectObj.transform.position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * CenterDistance;
            angle += angleAdd;
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, RotationSpeed, 0.0f));
    }
}
