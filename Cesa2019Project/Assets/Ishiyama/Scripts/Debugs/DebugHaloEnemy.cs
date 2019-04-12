using UnityEngine;

/// <summary>
/// 光輪を出す敵のサンプル
/// </summary>
public class DebugHaloEnemy : MonoBehaviour
{
    [SerializeField, Header("ターゲット")]
    Transform TargetTransform = null;
    [SerializeField, Header("光輪")]
    GameObject HaloObject = null;
    void Update()
    {
        //エンターを押したら光輪を出す
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShotHalo();
        }
    }

    /// <summary>
    /// 光輪を出す
    /// </summary>
    void ShotHalo()
    {
        //右に出ていく光輪の生成
        {
            GameObject haloObjectRight = Instantiate(HaloObject, transform.position, Quaternion.identity);
            haloObjectRight.GetComponent<DebugHalo>().HaloInit(TargetTransform.position, true);
        }
        //左に出ていく光輪の生成
        {
            GameObject haloObjectLeft = Instantiate(HaloObject, transform.position, Quaternion.identity);
            haloObjectLeft.GetComponent<DebugHalo>().HaloInit(TargetTransform.position, false);
        }
    }
}
