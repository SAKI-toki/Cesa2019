using UnityEngine;

public class DebugHaloEnemy : MonoBehaviour
{
    [SerializeField, Header("ターゲット")]
    Transform TargetTransform = null;
    [SerializeField, Header("光輪")]
    GameObject HaloObject = null;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShotHalo();
        }
    }

    void ShotHalo()
    {
        GameObject haloObjectRight = Instantiate(HaloObject, transform.position, Quaternion.identity);
        var haloRight = haloObjectRight.GetComponent<DebugHalo>();
        haloRight.HaloInit(TargetTransform.position, true);

        GameObject haloObjectLeft = Instantiate(HaloObject, transform.position, Quaternion.identity);
        var haloLeft = haloObjectLeft.GetComponent<DebugHalo>();
        haloLeft.HaloInit(TargetTransform.position, false);
    }
}
