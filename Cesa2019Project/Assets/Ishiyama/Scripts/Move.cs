using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    CameraController CameraCtrl;
    [SerializeField]
    Transform CameraTransform;
    void Start()
    {
        CameraCtrl.CameraStart();
    }
    void Update()
    {
        Vector3 pos = transform.position;
        if(Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 cameraForward = CameraTransform.forward;
            cameraForward.y = 0;
            pos += cameraForward.normalized * 10 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 cameraForward = CameraTransform.forward;
            cameraForward.y = 0;
            pos -= cameraForward.normalized * 10 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 cameraRight = CameraTransform.right;
            cameraRight.y = 0;
            pos += cameraRight.normalized * 10 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 cameraRight = CameraTransform.right;
            cameraRight.y = 0;
            pos -= cameraRight.normalized * 10 * Time.deltaTime;
        }
        transform.position = pos;

        CameraCtrl.CameraUpdate();
    }
}
