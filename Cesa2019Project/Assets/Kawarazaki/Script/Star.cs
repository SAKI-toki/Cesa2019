using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 星のUI
/// </summary>
public class Star : MonoBehaviour
{
    //座標(Big)
    //X
    [SerializeField]
    float StarPosX = -0.0f;
    //Y
    [SerializeField]
    float StarPosY = 0.0f;

    //星のサイズ(Big)
    [SerializeField]
    float StarScale = 0.25f;

    //星(Big)
    [SerializeField]
    GameObject RedBigStar = null;
    [SerializeField]
    GameObject BlueBigStar = null;
    [SerializeField]
    GameObject GreenBigStar = null;

    //星(Little)
    [SerializeField]
    Image RedLittleStar = null;
    [SerializeField]
    Image BlueLittleStar = null;
    [SerializeField]
    Image GreenLittleStar = null;

    [SerializeField]
    ParticleSystem StarParticle = null;

    //星のリスト（Big）
    List<GameObject> RedStarList = new List<GameObject>();
    List<GameObject> BlueStarList = new List<GameObject>();
    List<GameObject> GreenStarList = new List<GameObject>();

    //1フレーム前の星の数を格納(BIG)
    int[] PrevStarBigNum = new int[(int)(HaveStarManager.StarColorEnum.None)];

    //1フレーム前の星の数を格納(LITTLE)
    int[] PrevStarLittleNum = new int[(int)(HaveStarManager.StarColorEnum.None)];

    //初期化
    void Start()
    {
        HaveStarManager.AllZeroReset();
        RedLittleStar.fillAmount = 0.0f;
        BlueLittleStar.fillAmount = 0.0f;
        GreenLittleStar.fillAmount = 0.0f;
        StarParticle.Stop();
    }

    void Update()
    {
        //デバッグ
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HaveStarManager.AddLittleStar(HaveStarManager.StarColorEnum.Red);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HaveStarManager.AddLittleStar(HaveStarManager.StarColorEnum.Blue);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HaveStarManager.AddLittleStar(HaveStarManager.StarColorEnum.Green);
        }
#endif
        UpdatePrevStarLittle();
        UpdatePrevStarBig();
    }
    /// <summary>
    /// 小さい星の増加
    /// </summary>
    /// <param name="starColor"></param>
    /// <param name="num"></param>
    public void AddLittleStarUi(HaveStarManager.StarColorEnum starColor)
    {
        switch (starColor)
        {
            case HaveStarManager.StarColorEnum.Red:
                RedLittleStar.fillAmount = HaveStarManager.GetLittleStar(starColor) / 15.0f;
                break;
            case HaveStarManager.StarColorEnum.Blue:
                BlueLittleStar.fillAmount = HaveStarManager.GetLittleStar(starColor) / 15.0f;
                break;
            case HaveStarManager.StarColorEnum.Green:
                GreenLittleStar.fillAmount = HaveStarManager.GetLittleStar(starColor) / 15.0f;
                break;
        }
    }
    //小さい星の換算
    public void ConversionLittleStarUi(HaveStarManager.StarColorEnum starColor)
    {
        switch (starColor)
        {
            case HaveStarManager.StarColorEnum.Red:
                RedLittleStar.fillAmount = 0.0f;
                break;
            case HaveStarManager.StarColorEnum.Blue:
                BlueLittleStar.fillAmount = 0.0f;
                break;
            case HaveStarManager.StarColorEnum.Green:
                GreenLittleStar.fillAmount = 0.0f;
                break;
        }
    }

    /// <summary>
    /// 大きい星の増加
    /// </summary>
    /// <param name="starColor"></param>
    public void AddBigStarUI(HaveStarManager.StarColorEnum starColor)
    {
        System.Func<GameObject, bool> starGeneration =
            (GameObject obj) =>
            {
                obj.transform.SetParent(gameObject.transform, false);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(StarPosX, StarPosY, 1.0f);
                obj.GetComponent<RectTransform>().localScale = new Vector3(StarScale, StarScale, 1.0f);
                return true;
            };
        switch (starColor)
        {
            case HaveStarManager.StarColorEnum.Red:
                GameObject redStarPrefab = Instantiate(RedBigStar);
                starGeneration(redStarPrefab);
                RedStarList.Add(redStarPrefab);
                break;
            case HaveStarManager.StarColorEnum.Blue:
                GameObject blueStarPrefab = Instantiate(BlueBigStar);
                starGeneration(blueStarPrefab);
                BlueStarList.Add(blueStarPrefab);
                break;
            case HaveStarManager.StarColorEnum.Green:
                GameObject greenStarPrefab = Instantiate(GreenBigStar);
                starGeneration(greenStarPrefab);
                GreenStarList.Add(greenStarPrefab);
                break;
        }

        //大きい星が生成された時のパーティクル
        StarParticle.Play();
        if (!StarParticle.IsAlive())
            StarParticle.Stop();
    }

    /// <summary>
    /// 大きい星の減少
    /// </summary>
    /// <param name="starColor"></param>
    public void SubBigStarUI(HaveStarManager.StarColorEnum starColor)
    {
        switch (starColor)
        {
            case HaveStarManager.StarColorEnum.Red:
                Destroy(RedStarList[RedStarList.Count - 1]);
                RedStarList.RemoveAt(RedStarList.Count - 1);
                break;
            case HaveStarManager.StarColorEnum.Blue:
                Destroy(BlueStarList[BlueStarList.Count - 1]);
                BlueStarList.RemoveAt(BlueStarList.Count - 1);
                break;
            case HaveStarManager.StarColorEnum.Green:
                Destroy(GreenStarList[GreenStarList.Count - 1]);
                GreenStarList.RemoveAt(GreenStarList.Count - 1);
                break;
        }
    }

    void UpdatePrevStarLittle()
    {
        for (int i = 0; i < PrevStarLittleNum.Length; ++i)
        {
            //現在の星の数(Little)
            int currentStarNum = HaveStarManager.GetLittleStar((HaveStarManager.StarColorEnum)i);
            //数が違う場合
            if (PrevStarLittleNum[i] != currentStarNum)
            {
                if (PrevStarLittleNum[i] < currentStarNum)
                {
                    AddLittleStarUi((HaveStarManager.StarColorEnum)i);
                }

                PrevStarLittleNum[i] = currentStarNum;
            }

            //小さい星換算
            if (PrevStarLittleNum[i] == 0)
            {
                ConversionLittleStarUi((HaveStarManager.StarColorEnum)i);
            }
        }
    }

    void UpdatePrevStarBig()
    {
        for (int i = 0; i < PrevStarBigNum.Length; ++i)
        {
            //現在の星の数(BIG)
            int currentStarNum = HaveStarManager.GetBigStar((HaveStarManager.StarColorEnum)i);
            //数が違う場合
            if (PrevStarBigNum[i] != currentStarNum)
            {
                //星を増加
                if (PrevStarBigNum[i] < currentStarNum)
                {
                    AddBigStarUI((HaveStarManager.StarColorEnum)i);
                }
                //星減少
                else
                {
                    SubBigStarUI((HaveStarManager.StarColorEnum)i);
                }
                PrevStarBigNum[i] = currentStarNum;
            }

            if (HaveStarManager.Conversionflg[i])
            {
                SubBigStarUI((HaveStarManager.StarColorEnum)i);
                AddBigStarUI((HaveStarManager.StarColorEnum)i);
                HaveStarManager.Conversionflg[i] = false;
            }
        }
    }
}