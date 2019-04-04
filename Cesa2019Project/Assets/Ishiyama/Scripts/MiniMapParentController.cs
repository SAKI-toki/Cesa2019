using UnityEngine;

/// <summary>
/// ミニマップの回転
/// </summary>
public class MiniMapParentController : MonoBehaviour
{
    [SerializeField, Header("カメラTransform")]
    Transform CameraTransform = null;
    [SerializeField, Header("回転するかどうか")]
    bool IsRotation = true;
    /// <summary>
    /// ミニマップが回転する処理
    /// </summary>
    void Update()
    {
        if (IsRotation) transform.localEulerAngles = new Vector3(0, 0, CameraTransform.eulerAngles.y);
    }
}
