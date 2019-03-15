using UnityEngine;

public class DebugPlayerMoveRotation : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    float MoveSpeed = 10.0f;
    [SerializeField, Header("カメラTransform")]
    Transform CameraTransform = null;
    [SerializeField, Header("プレイヤーTransform")]
    Transform PlayerTransform = null;
    void Update()
    {
        var pos = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 vector = new Vector3(CameraTransform.forward.x, 0, CameraTransform.forward.z);
            pos += vector.normalized * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 vector = new Vector3(CameraTransform.forward.x, 0, CameraTransform.forward.z);
            pos -= vector.normalized * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 vector = new Vector3(CameraTransform.right.x, 0, CameraTransform.right.z);
            pos += vector.normalized * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 vector = new Vector3(CameraTransform.right.x, 0, CameraTransform.right.z);
            pos -= vector.normalized * MoveSpeed * Time.deltaTime;
        }
        PlayerTransform.LookAt(new Vector3(pos.x, PlayerTransform.position.y, pos.z));
        transform.position = pos;
    }
}
