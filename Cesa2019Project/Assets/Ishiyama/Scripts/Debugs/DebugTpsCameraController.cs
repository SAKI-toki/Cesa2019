using UnityEngine;

public class DebugTpsCameraController : MonoBehaviour
{
    [SerializeField, Header("距離")]
    float TargetDistance = 10.0f;
    [SerializeField, Header("カメラTransform")]
    Transform CameraTransform = null;
    [SerializeField, Header("回転速度")]
    float RotationSpeed = 20.0f;
    float RotX = 0, RotY = 0;
    void Start()
    {
        CameraTransform.localPosition = new Vector3(0, 0, -TargetDistance);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            RotY += RotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotY -= RotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            RotX -= RotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            RotX += RotationSpeed * Time.deltaTime;
        }
        transform.localEulerAngles = new Vector3(RotX, RotY, 0);
    }
}
