using UnityEngine;

/// <summary>
/// 常にカメラのほうを向き星の形を維持する
/// </summary>
public class StarRotationController : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
