using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField, Header("星の管理オブジェクト")]
    GameObject StarParentObject = null;
    //星リスト
    List<StarPlace> StarPlaces = new List<StarPlace>();
    [SerializeField, Header("プレイヤーImageのRectTransform")]
    RectTransform PlayerImageRectTransform = null;
    [SerializeField, Header("星のイメージ")]
    GameObject StarImage = null;
    //星のImageリスト
    List<Image> StarImages = new List<Image>();
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
        for (int i = 0; i < StarParentObject.transform.childCount; ++i)
        {
            StarPlaces.Add(StarParentObject.transform.GetChild(i).GetComponent<StarPlace>());
        }
        foreach (var star in StarPlaces)
        {
            GameObject starObj = Instantiate(StarImage);
            //一つのオブジェクトにまとめる
            starObj.transform.SetParent(StarImageParent.transform);
            starObj.transform.localScale = new Vector3(0.08f, 0.08f, 1.0f);
            //位置をセット
            SetPositionImage(starObj.GetComponent<RectTransform>(), star.transform.position);
            //リストに追加
            StarImages.Add(starObj.GetComponent<Image>());
        }
    }
    /// <summary>
    /// ミニマップの更新
    /// </summary>
    void Update()
    {
        UpdateColor();
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

    /// <summary>
    /// 色の更新
    /// </summary>
    void UpdateColor()
    {
        for (int i = 0; i < StarPlaces.Count; ++i)
        {
            //星の色によってImageを生成する
            switch (StarPlaces[i].StarColor)
            {
                case HaveStarManager.StarColorEnum.Red:
                    {
                        StarImages[i].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    }
                case HaveStarManager.StarColorEnum.Green:
                    {
                        StarImages[i].color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                        break;
                    }
                case HaveStarManager.StarColorEnum.Blue:
                    {
                        StarImages[i].color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                        break;
                    }
                case HaveStarManager.StarColorEnum.Yellow:
                    {
                        StarImages[i].color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
                        break;
                    }
                case HaveStarManager.StarColorEnum.None:
                    {
                        StarImages[i].color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
                        break;
                    }
            }
        }
    }
}
