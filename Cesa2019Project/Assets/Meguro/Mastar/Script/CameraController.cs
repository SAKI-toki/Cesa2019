using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform Player = null;
    [SerializeField]
    Transform LookAt = null;
    [SerializeField]
    float RotXmax = 0;
    [SerializeField]
    float RotXmin = 0;
    [SerializeField]
    float RotXSpeed = 0;
    [SerializeField]
    bool XReverse = false;
    [SerializeField]
    float RotYSpeed = 0;
    [SerializeField]
    bool YReverse = false;
    [SerializeField]
    public float Distance = 0.0f;
    [SerializeField]
    Transform CameraTransform = null;
    [SerializeField]
    LayerMask Mask = 0;
    float RightStickH = 0, RightStickV = 0;
    float LeftStickH = 0;
    float RotX, RotY;

    void Start()
    {
        CameraTransform.localPosition = new Vector3(0, 0, -Distance);
        RotX = transform.eulerAngles.x;
        RotY = transform.eulerAngles.y;
    }

    void Update()
    {
        if (StarPlaceManager.StarSelect)
        {
            MoveStop();
            return;
        }
        if (StarPlaceManager.AllPlaceSet)
        {
            MoveStop();
            return;
        }
        if(Player.GetComponent<Player>().DeathFlg)
        {
            MoveStop();
            return;
        }

        transform.position = LookAt.position;

        RightStickH = Input.GetAxis("R_Stick_H");
        RightStickV = Input.GetAxis("R_Stick_V");
        LeftStickH = Input.GetAxis("L_Stick_H");

        if (XReverse)
        {
            RotX -= Mathf.Sign(RightStickV) * Mathf.Pow(Mathf.Abs(RightStickV), 3) * RotXSpeed * Time.deltaTime;
        }
        else
        {
            RotX += Mathf.Sign(RightStickV) * Mathf.Pow(Mathf.Abs(RightStickV), 3) * RotXSpeed * Time.deltaTime;
        }
        if (RightStickH != 0)
        {
            if (YReverse)
            {
                RotY -= RightStickH * RotYSpeed * Time.deltaTime;
            }
            else
            {
                RotY += RightStickH * RotYSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (YReverse)
            {
                RotY -= LeftStickH * Time.deltaTime;
            }
            else
            {
                RotY += LeftStickH * Time.deltaTime;
            }
        }
        RotX = Mathf.Clamp(RotX, RotXmin, RotXmax);

        transform.eulerAngles = new Vector3(RotX, RotY, 0);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, CameraTransform.position - transform.position);
        if (Physics.Raycast(ray, out hit, Distance, Mask))
        {
            CameraTransform.localPosition = new Vector3(0, 0, -hit.distance);
        }
        else
        {
            CameraTransform.localPosition = new Vector3(0, 0, -Distance);
        }
    }

    void MoveStop()
    {
        RightStickH = 0;
        RightStickV = 0;
    }

    Quaternion InitQuaternion;
    Quaternion PlayerQuaternion;
    float RotationTime = 0.0f;
    public void ClearMoveInit()
    {
        InitQuaternion = this.transform.rotation;
        Quaternion quat = new Quaternion();
        quat.eulerAngles = new Vector3(-5, 180, 0);
        PlayerQuaternion = Player.rotation * quat;
    }

    public void ClearMove()
    {
        transform.position = LookAt.position;
        RotationTime += Time.deltaTime / 20.0f;
        transform.rotation = Quaternion.Lerp(InitQuaternion, PlayerQuaternion, RotationTime);
    }

    public void ZoomIn(float num)
    {
        Distance -= num;
        CameraTransform.localPosition = new Vector3(0, 0, -Distance);
    }

    public void DeathMoveInit()
    {
        InitQuaternion = this.transform.rotation;
        Quaternion quat = new Quaternion();
        quat.eulerAngles = new Vector3(60, 0, 0);
        PlayerQuaternion = Player.rotation * quat;
    }

    public void DeathMove()
    {
        transform.position = LookAt.position;
        RotationTime += Time.deltaTime / 20.0f;
        transform.rotation = Quaternion.Lerp(InitQuaternion, PlayerQuaternion, RotationTime);
    }

    public void DeathRotation()
    {
        transform.position = LookAt.position;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 0.5f, transform.eulerAngles.z);
    }
}
