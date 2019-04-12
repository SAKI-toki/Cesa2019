using UnityEngine;

public class DebugHalo : MonoBehaviour
{
    const float ForwardSpeed = 10.0f;
    const float SideSpeed = 10.0f;
    Vector3 ForwardMoveVector = new Vector3();
    Vector3 SideMoveVector = new Vector3();
    Vector3 TargetPosition = new Vector3();
    float InitFromThisToTargetDitance = 0.0f;

    void Update()
    {
        float fromThisToTargetDistance = Vector3.Distance(transform.position, TargetPosition);
        transform.Translate(ForwardMoveVector * Time.deltaTime);
        transform.Translate(SideMoveVector * Time.deltaTime * SideSpeed *
            ((float)System.Math.Sinh(fromThisToTargetDistance / InitFromThisToTargetDitance - 0.5f)));
        if (fromThisToTargetDistance < 1.0f) Destroy(gameObject);
    }

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
