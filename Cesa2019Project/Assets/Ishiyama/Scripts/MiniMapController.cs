using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミニマップ
/// プレイヤーと星の位置が分かり、ステージ全体を移す
/// </summary>
public class MiniMapController : MonoBehaviour
{
    [SerializeField, Header("位置の比")]
    float PositionRatio = 2.0f;
    [SerializeField, Header("UIの位置")]
    Vector3 UiPosition = new Vector3();
    [SerializeField, Header("プレイヤーTransform")]
    Transform PlayerTransform = null;
    [SerializeField, Header("星リスト")]
    List<StarPlace> StarPlaces = new List<StarPlace>();
    [SerializeField, Header("プレイヤーImageのRectTransform")]
    RectTransform PlayerImageRectTransform = null;
    [SerializeField, Header("赤星ImageObject")]
    GameObject RedStarImage = null;
    [SerializeField, Header("緑星ImageObject")]
    GameObject GreenStarImage = null;
    [SerializeField, Header("青星ImageObject")]
    GameObject BlueStarImage = null;
    [SerializeField, Header("まだはまっていないImageObject")]
    GameObject NoneStarImage = null;
    //星のImageリスト
    List<GameObject> StarImages = new List<GameObject>();
    [SerializeField, Header("ImageObjectの親オブジェクト")]
    GameObject ImageParentObject = null;
    [SerializeField, Header("StarImageの親オブジェクト")]
    GameObject StarImageParent = null;
    /// <summary>
    /// ミニマップの初期化
    /// </summary>
    void Start()
    {
        ImageParentObject.transform.localPosition = UiPosition;
        foreach (var star in StarPlaces)
        {
            GameObject starObj = null;
            //星の色によってImageを生成する
            switch (star.StarColor)
            {
                case HaveStarManager.StarColorEnum.Red:
                    {
                        starObj = Instantiate(RedStarImage);
                        break;
                    }
                case HaveStarManager.StarColorEnum.Green:
                    {
                        starObj = Instantiate(GreenStarImage);
                        break;
                    }
                case HaveStarManager.StarColorEnum.Blue:
                    {
                        starObj = Instantiate(BlueStarImage);
                        break;
                    }
                case HaveStarManager.StarColorEnum.None:
                    {
                        starObj = Instantiate(NoneStarImage);
                        break;
                    }
            }
            //一つのオブジェクトにまとめる
            starObj.transform.SetParent(StarImageParent.transform);
            starObj.transform.localScale = new Vector3(0.08f, 0.08f, 1.0f);
            //位置をセット
            SetPositionImage(starObj.GetComponent<RectTransform>(), star.transform.position);
            //リストに追加
            StarImages.Add(starObj);
        }
    }
    /// <summary>
    /// ミニマップの更新
    /// </summary>
    void Update()
    {
        UpdatePlayer();
    }

    /// <summary>
    /// プレイヤーの位置と向きでImageのRectTransformも更新
    /// </summary>
    void UpdatePlayer()
    {
        SetPositionImage(PlayerImageRectTransform, PlayerTransform.position);
        PlayerImageRectTransform.localEulerAngles = new Vector3(0, 0, -PlayerTransform.localEulerAngles.y);
    }

    /// <summary>
    /// 位置の設定
    /// </summary>
    /// <param name="rectTransform">設定するRectTransform</param>
    /// <param name="pos">位置</param>
    void SetPositionImage(RectTransform rectTransform, Vector3 pos)
    {
        rectTransform.transform.localPosition = new Vector3(pos.x, pos.z, 0) * PositionRatio;
    }
}
