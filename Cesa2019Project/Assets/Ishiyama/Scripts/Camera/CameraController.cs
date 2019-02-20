using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの制御クラス
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField, Header("追尾するオブジェクトのTransform")]
    Transform TargetTransform = null;
    [SerializeField, Header("カメラのTransform")]
    Transform CameraTransform = null;
    [SerializeField, Header("水平方向のカメラの回転スピード")]
    float HorizontalRotationSpeed = 0.0f;
    [SerializeField, Header("垂直方向のカメラの回転スピード")]
    float VerticalRotationSpeed = 0.0f;
    [SerializeField, Header("カメラの移動(追尾)速度")]
    float MoveSpeed = 0.0f;
    [SerializeField, Header("垂直方向の最小値"), Range(-90, 0)]
    float VerticalLimitMin = 0.0f;
    [SerializeField, Header("垂直方向の最大値"), Range(0, 90)]
    float VerticalLimitMax = 0.0f;
    [SerializeField, Header("ターゲットとの距離の最大値")]
    float TargetDistanceMax = 0.0f;
    [SerializeField, Header("このレイヤーのオブジェクトが間にあるときはそれより前にカメラを出す")]
    LayerMask PushOutMask = new LayerMask();
    [SerializeField, Header("カメラのオフセット")]
    Vector3 OffSet = new Vector3(0, 0, 0);
    [SerializeField, Header("ターゲットを透過させる距離")]
    float TargetTransparentDistance = 0.0f;
    [SerializeField, Header("初期の回転")]
    float Horizontal = 0.0f, Vertical = 0.0f;
    [SerializeField, Header("カメラ")]
    Camera CameraObject = null;

    /// <summary>
    /// カメラの初期化
    /// </summary>
    public void CameraStart()
    {
        //カメラを一定距離離す
        transform.position = TargetTransform.position + OffSet;
        CameraTransform.localPosition = new Vector3(0, 0, -TargetDistanceMax);
    }

    /// <summary>
    /// カメラの更新
    /// </summary>
    public void CameraUpdate()
    {
        //カメラの回転
        CameraRotation();
        //押し出し処理
        CameraPushOut();
        //追尾処理
        TargetTracking();
    }

    /// <summary>
    /// ターゲットを軸にカメラを回転する関数
    /// </summary>
    void CameraRotation()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vertical -= VerticalRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vertical += VerticalRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Horizontal += HorizontalRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Horizontal -= HorizontalRotationSpeed * Time.deltaTime;
        }
        //縦方向を範囲内に収める
        Vertical = Mathf.Clamp(Vertical, VerticalLimitMin, VerticalLimitMax);
        //角度の更新
        Vector3 cameraObjEulerAngles = transform.eulerAngles;
        cameraObjEulerAngles.x = Vertical;
        cameraObjEulerAngles.y = Horizontal;
        transform.eulerAngles = cameraObjEulerAngles;
    }

    /// <summary>
    /// 押し出し処理
    /// </summary>
    void CameraPushOut()
    {
        var cameraPos = CameraTransform.localPosition;
        //レイを飛ばし、当たったオブジェクトより前にカメラを押し出す
        Ray pushOutRay = new Ray(transform.position, -transform.forward);
        RaycastHit pushOutHit;
        //当たったら当たったオブジェクトより前に出す
        if (Physics.Raycast(pushOutRay, out pushOutHit, -(cameraPos.z - MoveSpeed * Time.deltaTime), PushOutMask))
        {
            cameraPos.z = -pushOutHit.distance;

        }
        //当たらなかったらカメラを元の位置に戻す
        else
        {
            cameraPos.z += -MoveSpeed * Time.deltaTime;
            if (-cameraPos.z > TargetDistanceMax)
            {
                cameraPos.z = -TargetDistanceMax;
            }
        }
        CameraTransform.localPosition = cameraPos;
    }

    /// <summary>
    /// ターゲットに追尾する処理
    /// </summary>
    void TargetTracking()
    {
        //ターゲットのワールド座標をスクリーン座標に変換
        Vector2 rectPos = RectTransformUtility.WorldToScreenPoint(CameraObject, TargetTransform.position);
        rectPos.x = rectPos.x / Screen.width;
        rectPos.y = rectPos.y / Screen.height;
        //中心からの距離を求める
        float distance = Vector2.Distance(new Vector2(0.5f, 0.5f), rectPos);
        //移動するベクトル
        Vector3 moveVector = (TargetTransform.position + OffSet) - transform.position;
        Vector3 cameraObjPos = transform.position;
        Vector3 addVector = moveVector * MoveSpeed;
        cameraObjPos += addVector * Time.deltaTime;
        transform.position = cameraObjPos;
    }

    /// <summary>
    /// ターゲットが一定より近くなったら透過させる
    /// </summary>
    void TargetTransparent()
    {
        if (CameraTransform.position.z < TargetTransparentDistance)
        {
            //TODO:透過処理
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void MoveCamera()
    {
        Vector3 cameraForward = CameraTransform.forward;
        Vector3 targetForward = TargetTransform.forward;

    }
}
