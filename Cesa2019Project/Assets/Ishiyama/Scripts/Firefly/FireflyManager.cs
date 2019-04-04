using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蛍を管理するクラス
/// </summary>
public class FireflyManager : MonoBehaviour
{
    [SerializeField, Header("最大の蛍の数")]
    int MaxFireflyNum = 10;
    [SerializeField, Header("蛍オブジェクト")]
    GameObject FireflyObject = null;
    [SerializeField, Header("ターゲットからの距離")]
    float ActiveRange = 10.0f;
    [SerializeField, Header("ターゲットTransform")]
    Transform TargetTransform = null;
    //蛍リスト
    List<GameObject> FireflyList = new List<GameObject>();
    GameObject FireflyParentObject = null;

    void Start()
    {
        FireflyParentObject = new GameObject("FireflyParentObject");
        FireflyParentObject.transform.position = Vector3.zero;
        //最初に蛍を最大数追加する
        for (int i = 0; i < MaxFireflyNum; ++i)
        {
            FireflyList.Add(FireflyGenerate());
        }
    }

    void Update()
    {
        for (int i = 0; i < FireflyList.Count; ++i)
        {
            //範囲外になったらDestroyし生成する
            if (Vector3.Distance(FireflyList[i].transform.position, TargetTransform.position) > ActiveRange)
            {
                Destroy(FireflyList[i]);
                //蛍生成
                FireflyList[i] = FireflyGenerate();
            }
        }
    }

    /// <summary>
    /// 蛍を生成する
    /// </summary>
    /// <returns>生成した蛍のGameObject</returns>
    GameObject FireflyGenerate()
    {
        //位置はターゲットからある程度離れた位置にランダムに生成
        return Instantiate(FireflyObject,
               new Vector3(
                   Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)
                   ).normalized * ActiveRange * 0.7f + TargetTransform.position,
               Quaternion.identity, FireflyParentObject.transform);
    }
}
