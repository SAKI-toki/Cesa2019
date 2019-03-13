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
    //小さい星のテキスト(緑)
    [SerializeField]
    private TextMeshProUGUI LittleStarGreenText = null;
    //大きい星のテキスト(緑)
    [SerializeField]
    private TextMeshProUGUI BigStarGreenText = null;

    //小さい星のテキスト(赤)
    [SerializeField]
    private TextMeshProUGUI LittleStarRedText = null;
    //大きい星のテキスト(赤)
    [SerializeField]
    private TextMeshProUGUI BigStarRedText = null;

    //小さい星のテキスト(青)
    [SerializeField]
    private TextMeshProUGUI LittleStarBlueText = null;
    //大きい星のテキスト(青)
    [SerializeField]
    private TextMeshProUGUI BigStarBlueText = null;
    const string LittleString = "Little:";
    const string BigString = "Big:";
    

    [SerializeField]
    GameObject RedBigStar = null;
    [SerializeField]
    GameObject BlueBigStar = null;
    [SerializeField]
    GameObject GreenBigStar = null;
    
    //X座標
    [SerializeField]
    float RedPosX = -90.0f;
    [SerializeField]
    float BluePosX = -65.0f;
    [SerializeField]
    float GreenPosX = -40.0f;
    //Y座標
    [SerializeField]
    float RedPosY = 60.0f;
    [SerializeField]
    float BluePosY = 60.0f;
    [SerializeField]
    float GreenPosY = 60.0f;


    //Scale
    [SerializeField]
    float Scale = 0.25f;

    List<GameObject> RedStarList = new List<GameObject>();
    List<GameObject> BlueStarList = new List<GameObject>();
    List<GameObject> GreenStarList = new List<GameObject>();

    //1フレーム前の星の数を格納(BIG)
    int[] PrevStarNum = new int[(int)(HaveStarManager.StarColorEnum.None)];

    //static public GameObject[] RedStar = new GameObject[11];
    //static public GameObject[] BlueStar = new GameObject[11];
    //static public GameObject[] GreenStar = new GameObject[11];
    //初期化
    void Start()
    {

    }

    void Update()
    {
        UpdatePrevStar();
    }

    void FixedUpdate()
    {
        LittleStarGreenText.text = LittleString +
            HaveStarManager.GetLittleStar(HaveStarManager.StarColorEnum.Green).ToString("00");
        BigStarGreenText.text = BigString +
            HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Green).ToString("00");

        LittleStarRedText.text = LittleString +
            HaveStarManager.GetLittleStar(HaveStarManager.StarColorEnum.Red).ToString("00");
        BigStarRedText.text = BigString +
            HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Red).ToString("00");

        LittleStarBlueText.text = LittleString +
            HaveStarManager.GetLittleStar(HaveStarManager.StarColorEnum.Blue).ToString("00");
        BigStarBlueText.text = BigString +
            HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Blue).ToString("00");
    }

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

    void UpdatePrevStar()
    {
        for (int i = 0; i < PrevStarNum.Length; ++i)
        {
            //現在の星の数(BIG)
            int currentStarNum = HaveStarManager.GetBigStar((HaveStarManager.StarColorEnum)i);
            //数が違う場合
            if (PrevStarNum[i] != currentStarNum)
            {
                if (PrevStarNum[i] < currentStarNum)
                {
                    AddBigStarUI((HaveStarManager.StarColorEnum)i);
                }
                else
                {
                    SubBigStarUI((HaveStarManager.StarColorEnum)i);
                }
                PrevStarNum[i] = currentStarNum;
            }
        }
    }
}