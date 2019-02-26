using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{
    [SerializeField]
    float MoveSpeed = 1.0f;

    float RotX = 0, RotY = 0;
    void Update()
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
        float moveX, moveY;
        moveX = Input.GetAxis("Mouse X");
        moveY = Input.GetAxis("Mouse Y");
        RotX -= moveY * Time.deltaTime * 10;
        RotY += moveX * Time.deltaTime * 10;
        transform.eulerAngles = new Vector3(RotX, RotY, 0);
    }
}
