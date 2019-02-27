using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デバッグ用にカメラを自由に動かせる
/// </summary>
public class DebugCameraController : MonoBehaviour
{
    [SerializeField,Header("移動速度")]
    float MoveSpeed = 1.0f;

    //回転
    float RotX = 0, RotY = 0;

    /// <summary>
    /// カメラの更新
    /// </summary>
    void Update()
    {
        //移動
        {
            var pos = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                pos += transform.forward * MoveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                pos -= transform.forward * MoveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                pos += transform.right * MoveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                pos -= transform.right * MoveSpeed * Time.deltaTime;
            }
            transform.position = pos;
        }
        //回転
        {
            float moveX, moveY;
            moveX = Input.GetAxis("Mouse X");
            moveY = Input.GetAxis("Mouse Y");
            RotX -= moveY * Time.deltaTime * 10;
            RotY += moveX * Time.deltaTime * 10;
            transform.eulerAngles = new Vector3(RotX, RotY, 0);
        }
    }
}
