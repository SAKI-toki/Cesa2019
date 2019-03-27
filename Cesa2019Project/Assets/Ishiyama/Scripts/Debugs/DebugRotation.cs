using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回転するデバッグ用クラス
/// </summary>
public class DebugRotation : MonoBehaviour
{
    [SerializeField, Header("回転速度")]
    float RotationSpeed = 1.0f;

    /// <summary>
    /// z軸の回転
    /// </summary>
    void Update()
    {
        var rot = transform.eulerAngles;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rot.z += RotationSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rot.z -= RotationSpeed * Time.deltaTime;
        }
        transform.eulerAngles = rot;
    }
}
