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

    static float RedPosY = 60.0f;
    static float BluePosY = 60.0f;
    static float GreenPosY = 60.0f;

    const string LittleString = "Little:";
    const string BigString = "Big:";

    [SerializeField]
    static public List<GameObject> RedStar = new List<GameObject>();
    [SerializeField]
    static public List<GameObject> BlueStar = new List<GameObject>();
    [SerializeField]
    static public List<GameObject> GreenStar = new List<GameObject>();

    //初期化
    void Start()
    {

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

    static public void AddBigStarUI(HaveStarManager.StarColorEnum starColor)
    {
        switch (starColor)
        {
            case HaveStarManager.StarColorEnum.Red:

                GameObject redBigStar = new GameObject("RedBigStar");
                redBigStar.transform.parent = GameObject.Find("Canvas/Star").transform;
                redBigStar.AddComponent<RectTransform>().localPosition = new Vector3(-90.0f, RedPosY, 1.0f);
                redBigStar.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 1.0f);
                redBigStar.AddComponent<Image>().sprite = Resources.Load<Sprite>("RedStar");
                RedStar.Add(redBigStar);
                RedPosY += 10.0f;
                break;
            case HaveStarManager.StarColorEnum.Blue:
                GameObject blueBigStar = new GameObject("BlueBigStar");
                blueBigStar.transform.parent = GameObject.Find("Canvas/Star").transform;
                blueBigStar.AddComponent<RectTransform>().localPosition = new Vector3(-65.0f, BluePosY, 1.0f);
                blueBigStar.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 1.0f);
                blueBigStar.AddComponent<Image>().sprite = Resources.Load<Sprite>("BlueStar");
                BlueStar.Add(blueBigStar);
                BluePosY += 10.0f;
                break;
            case HaveStarManager.StarColorEnum.Green:
                GameObject greenBigStar = new GameObject("GreenBigStar");
                greenBigStar.transform.parent = GameObject.Find("Canvas/Star").transform;
                greenBigStar.AddComponent<RectTransform>().localPosition = new Vector3(-40.0f, GreenPosY, 1.0f);
                greenBigStar.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 1.0f);
                greenBigStar.AddComponent<Image>().sprite = Resources.Load<Sprite>("GreenStar");
                GreenStar.Add(greenBigStar);
                GreenPosY += 10.0f;
                break;
        }
    }

    static public void SubBigStarUI(HaveStarManager.StarColorEnum starColor, int StarNum)
    {
        switch (starColor)
        {
            case HaveStarManager.StarColorEnum.Red:
                Destroy(RedStar[StarNum]);
                RedPosY -= 10.0f;
                break;
            case HaveStarManager.StarColorEnum.Blue:
                Destroy(BlueStar[StarNum]);
                BluePosY -= 10.0f;
                break;
            case HaveStarManager.StarColorEnum.Green:
                Destroy(GreenStar[StarNum]);
                GreenPosY -= 10.0f;
                break;
        }
    }
}