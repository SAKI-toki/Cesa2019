using UnityEngine;

/// <summary>
/// 光輪のサンプル作成
/// 敵担当の人に参考として渡す
/// </summary>
public class DebugHalo : MonoBehaviour
{
    //前へのスピード
    const float ForwardSpeed = 10.0f;
    //横へのスピード
    const float SideSpeed = 10.0f;
    //前へのベクトル
    Vector3 ForwardMoveVector = new Vector3();
    //横へのベクトル
    Vector3 SideMoveVector = new Vector3();
    //ターゲットの位置(更新しない)
    Vector3 TargetPosition = new Vector3();
    //初期のターゲットと光輪(光輪を生成した敵)の距離
    float InitFromThisToTargetDitance = 0.0f;

    void Update()
    {
        //現在のターゲットとの距離
        float currentFromThisToTargetDistance = Vector3.Distance(transform.position, TargetPosition);
        transform.Translate(ForwardMoveVector * Time.deltaTime);
        transform.Translate(SideMoveVector * Time.deltaTime * SideSpeed *
            ((float)System.Math.Sinh(currentFromThisToTargetDistance / InitFromThisToTargetDitance - 0.5f)));
        if (currentFromThisToTargetDistance < 1.0f) Destroy(gameObject);
    }

    /// <summary>
    /// 光輪の初期化
    /// </summary>
    /// <param name="targetPositioin">ターゲットの位置</param>
    /// <param name="isRight">右向きかどうか</param>
    public void HaloInit(Vector3 targetPositioin, bool isRight)
    {
        TargetPosition = targetPositioin;
        InitFromThisToTargetDitance = Vector3.Distance(transform.position, TargetPosition);
        ForwardMoveVector = (TargetPosition - transform.position).normalized;
        SideMoveVector = Vector3.Cross(ForwardMoveVector, transform.up) *
            ((isRight) ? 1 : -1);
        ForwardMoveVector = ForwardMoveVector.normalized * ForwardSpeed;
    }
}

