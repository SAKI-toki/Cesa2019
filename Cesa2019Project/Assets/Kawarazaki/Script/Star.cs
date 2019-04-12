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
    ////小さい星のテキスト(緑)
    //[SerializeField]
    //private TextMeshProUGUI LittleStarGreenText = null;
    ////大きい星のテキスト(緑)
    //[SerializeField]
    //private TextMeshProUGUI BigStarGreenText = null;

    ////小さい星のテキスト(赤)
    //[SerializeField]
    //private TextMeshProUGUI LittleStarRedText = null;
    ////大きい星のテキスト(赤)
    //[SerializeField]
    //private TextMeshProUGUI BigStarRedText = null;

    ////小さい星のテキスト(青)
    //[SerializeField]
    //private TextMeshProUGUI LittleStarBlueText = null;
    ////大きい星のテキスト(青)
    //[SerializeField]
    //private TextMeshProUGUI BigStarBlueText = null;

    //const string LittleString = "Little:";
    //const string BigString = "Big:";

    //座標(Big)
    //X
    [SerializeField]
    float RedPosX = -90.0f;
    [SerializeField]
    float BluePosX = -65.0f;
    [SerializeField]
    float GreenPosX = -40.0f;
    //Y
    [SerializeField]
    float RedPosY = 60.0f;
    [SerializeField]
    float BluePosY = 60.0f;
    [SerializeField]
    float GreenPosY = 60.0f;

    //星のサイズ(Big)
    [SerializeField]
    float Scale = 0.25f;

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

    //星のリスト（Big）
    List<GameObject> RedStarList = new List<GameObject>();
    List<GameObject> BlueStarList = new List<GameObject>();
    List<GameObject> GreenStarList = new List<GameObject>();

    ////星（Little）
    //[SerializeField]
    //GameObject[] RedLittleStar = new GameObject[5];
    //[SerializeField]
    //GameObject[] BlueLittleStar = new GameObject[5];
    //[SerializeField]
    //GameObject[] GreenLittleStar = new GameObject[5];

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
    }

    void Update()
    {
        UpdatePrevStarLittle();
        UpdatePrevStarBig();
    }

    //void FixedUpdate()
    //{
    //    LittleStarGreenText.text = LittleString +
    //        HaveStarManager.GetLittleStar(HaveStarManager.StarColorEnum.Green).ToString("00");
    //    BigStarGreenText.text = BigString +
    //        HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Green).ToString("00");

    //    LittleStarRedText.text = LittleString +
    //        HaveStarManager.GetLittleStar(HaveStarManager.StarColorEnum.Red).ToString("00");
    //    BigStarRedText.text = BigString +
    //        HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Red).ToString("00");

    //    LittleStarBlueText.text = LittleString +
    //        HaveStarManager.GetLittleStar(HaveStarManager.StarColorEnum.Blue).ToString("00");
    //    BigStarBlueText.text = BigString +
    //        HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Blue).ToString("00");
    //}

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
                //for (int i = 0; i < HaveStarManager.GetLittleStar(starColor); ++i)
                //{
                //if (RedLittleStar[i].activeSelf == false)
                //    RedLittleStar[i].SetActive(true);
                //Debug.Log(RedLittleStar[i]);
                RedLittleStar.fillAmount = HaveStarManager.GetLittleStar(starColor) / 5.0f;
                //}
                break;
            case HaveStarManager.StarColorEnum.Blue:
                //for (int i = 0; i < HaveStarManager.GetLittleStar(starColor); ++i)
                //    if (BlueLittleStar[i].activeSelf == false)
                //        BlueLittleStar[i].SetActive(true);
                BlueLittleStar.fillAmount = HaveStarManager.GetLittleStar(starColor) / 5.0f;
                break;
            case HaveStarManager.StarColorEnum.Green:
                //for (int i = 0; i < HaveStarManager.GetLittleStar(starColor); ++i)
                //    if (GreenLittleStar[i].activeSelf == false)
                //        GreenLittleStar[i].SetActive(true);
                GreenLittleStar.fillAmount = HaveStarManager.GetLittleStar(starColor) / 5.0f;
                break;
        }
        Debug.Log(HaveStarManager.GetLittleStar(starColor));
    }
        //小さい星の換算
        public void ConversionLittleStarUi(HaveStarManager.StarColorEnum starColor)
        {
            switch (starColor)
            {
                case HaveStarManager.StarColorEnum.Red:
                    //for (int i = 0; i < RedLittleStar.Length; i++)
                    //    RedLittleStar[i].SetActive(false);
                    RedLittleStar.fillAmount = 0.0f;
                    break;
                case HaveStarManager.StarColorEnum.Blue:
                    //for (int i = 0; i < BlueLittleStar.Length; i++)
                    //    BlueLittleStar[i].SetActive(false); 
                    BlueLittleStar.fillAmount = 0.0f;
                    break;
                case HaveStarManager.StarColorEnum.Green:
                    //for (int i = 0; i <GreenLittleStar.Length; i++)
                    //    GreenLittleStar[i].SetActive(false);
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
        switch (starColor)
        {
            case HaveStarManager.StarColorEnum.Red:
                GameObject redStarPrefab = Instantiate(RedBigStar);
                redStarPrefab.transform.SetParent(gameObject.transform, false);
                redStarPrefab.GetComponent<RectTransform>().localPosition = new Vector3(RedPosX, RedPosY, 1.0f);
                redStarPrefab.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, 1.0f);
                RedStarList.Add(redStarPrefab);
                RedPosY += 10.0f;
                break;
            case HaveStarManager.StarColorEnum.Blue:
                GameObject blueStarPrefab = Instantiate(BlueBigStar);
                blueStarPrefab.transform.SetParent(gameObject.transform, false);
                blueStarPrefab.GetComponent<RectTransform>().localPosition = new Vector3(BluePosX, BluePosY, 1.0f);
                blueStarPrefab.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, 1.0f);
                BlueStarList.Add(blueStarPrefab);
                BluePosY += 10.0f;
                break;
            case HaveStarManager.StarColorEnum.Green:
                GameObject greenStarPrefab = Instantiate(GreenBigStar);
                greenStarPrefab.transform.SetParent(gameObject.transform, false);
                greenStarPrefab.GetComponent<RectTransform>().localPosition = new Vector3(GreenPosX, GreenPosY, 1.0f);
                greenStarPrefab.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, 1.0f);
                GreenStarList.Add(greenStarPrefab);
                GreenPosY += 10.0f;
                break;
        }
        
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
                RedPosY -= 10.0f;
                break;
            case HaveStarManager.StarColorEnum.Blue:
                Destroy(BlueStarList[BlueStarList.Count - 1]);
                BlueStarList.RemoveAt(BlueStarList.Count - 1);
                BluePosY -= 10.0f;
                break;
            case HaveStarManager.StarColorEnum.Green:
                Destroy(GreenStarList[GreenStarList.Count - 1]);
                GreenStarList.RemoveAt(GreenStarList.Count - 1);
                GreenPosY -= 10.0f;
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
                if(PrevStarLittleNum[i] < currentStarNum)
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
        }
    }
}
