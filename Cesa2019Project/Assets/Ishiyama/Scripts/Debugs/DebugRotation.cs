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

    enum Axis { X, Y, Z };
    [SerializeField, Header("軸")]
    Axis RotationAxis = Axis.X;
    /// <summary>
    /// z軸の回転
    /// </summary>
    void Update()
    {
        var rot = transform.eulerAngles;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            SwitchAxis(ref rot, RotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            SwitchAxis(ref rot, -RotationSpeed * Time.deltaTime);
        }
        transform.eulerAngles = rot;
    }

    /// <summary>
    /// 変数に応じて回転する軸を変更する
    /// </summary>
    /// <param name="rot">回転</param>
    /// <param name="rotationValue">回転量</param>
    void SwitchAxis(ref Vector3 rot, float rotationValue)
    {
        switch(RotationAxis)
        {
            case Axis.X:
                rot.x += rotationValue;
                break;
            case Axis.Y:
                rot.y += rotationValue;
                break;
            case Axis.Z:
                rot.z += rotationValue;
                break;
        }
    }
}
