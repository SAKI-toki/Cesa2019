using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// 星のUI
/// </summary>
public class Star : MonoBehaviour
{
    //小さい星のテキスト(緑)
    [SerializeField]
    private TextMeshProUGUI LittleStarGreenText;
    //大きい星のテキスト(緑)
    [SerializeField]
    private TextMeshProUGUI BigStarGreenText;
    //小さい星のテキスト(赤)
    [SerializeField]
    private TextMeshProUGUI LittleStarRedText;
    //大きい星のテキスト(赤)
    [SerializeField]
    private TextMeshProUGUI BigStarRedText;
    //小さい星のテキスト(青)
    [SerializeField]
    private TextMeshProUGUI LittleStarBlueText;
    //大きい星のテキスト(青)
    [SerializeField]
    private TextMeshProUGUI BigStarBlueText;

    //小さい星
    [SerializeField]
    public int LittleStarGreen,LittleStarRed,LittleStarBlue;
    //大きい星
    [SerializeField]
    public int BigStarGreen,BigStarRed,BigStarBlue;

    //小さい星から大きい星に変換するときの数
    [SerializeField, Header("大きい星に変換")]
    private int Conversion;

    int LittleGreen,LittleRed,LittleBlue;
    int BigGreen,BigRed,BigBlue;
    //初期化
    void Start()
    {
        LittleStarGreen = 0; LittleStarRed = 0;LittleStarBlue = 0;
        BigStarGreen = 0; BigStarRed = 0; BigBlue = 0;
        //緑の星
        LittleStarGreenText = GameObject.Find("LittleStarGreen").GetComponent<TextMeshProUGUI>();
        BigStarGreenText = GameObject.Find("BigStarGreen").GetComponent<TextMeshProUGUI>();
        //赤の星
        LittleStarRedText = GameObject.Find("LittleStarRed").GetComponent<TextMeshProUGUI>();
        BigStarRedText = GameObject.Find("BigStarRed").GetComponent<TextMeshProUGUI>();
        //青の星
        LittleStarBlueText = GameObject.Find("LittleStarBlue").GetComponent<TextMeshProUGUI>();
        BigStarBlueText = GameObject.Find("BigStarBlue").GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        //↓ここから
        //小さい星加算
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LittleGreen++;
            AddLittleStarGreen(LittleGreen);

            //小さい星が7個貯まったら大きい星加算
            if (LittleStarGreen >= Conversion)
            {
                LittleGreen -= Conversion;
                AddLittleStarGreen(LittleGreen);
                BigGreen++;
                AddBigStarGreen(BigGreen);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LittleRed++;
            AddLittleStarRed(LittleRed);

            //小さい星が7個貯まったら大きい星加算
            if (LittleStarRed >= Conversion)
            {
                LittleRed -= Conversion;
                AddLittleStarRed(LittleRed);
                BigRed++;
                AddBigStarRed(BigRed);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LittleBlue++;
            AddLittleStarBlue(LittleBlue);

            //小さい星が7個貯まったら大きい星加算
            if (LittleStarBlue >= Conversion)
            {
                LittleBlue -= Conversion;
                AddLittleStarBlue(LittleBlue);
                BigBlue++;
                AddBigStarBlue(BigBlue);
            }
        }
        //↑ここまで繋げるときに消して

        //UIテキストに表示
        LittleStarGreenText.text = "Little:" + LittleStarGreen.ToString("00");
        BigStarGreenText.text = "Big:" + BigStarGreen.ToString("00");

        LittleStarRedText.text = "Little:" + LittleStarRed.ToString("00");
        BigStarRedText.text = "Big:" + BigStarRed.ToString("00");

        LittleStarBlueText.text = "Little:" + LittleStarBlue.ToString("00");
        BigStarBlueText.text = "Big:" + BigStarBlue.ToString("00");
    }

    /// <summary>
    /// 小さい星(緑)
    /// </summary>
    /// <param name="little"></param>
    /// <returns>LittleStar</returns>
    public int AddLittleStarGreen(int little)
    {
        return LittleStarGreen = little;
    }
    
    /// <summary>
    /// 大きい星(緑)
    /// </summary>
    /// <param name="Big"></param>
    /// <returns></returns>
    public int AddBigStarGreen(int Big)
    {
        return BigStarGreen = Big;
    }

    /// <summary>
    /// 小さい星(赤)
    /// </summary>
    /// <param name="little"></param>
    /// <returns>LittleStar</returns>
    public int AddLittleStarRed(int little)
    {
        return LittleStarRed = little;
    }

    /// <summary>
    /// 大きい星(赤)
    /// </summary>
    /// <param name="Big"></param>
    /// <returns></returns>
    public int AddBigStarRed(int Big)
    {
        return BigStarRed = Big;
    }

    /// <summary>
    /// 小さい星(青)
    /// </summary>
    /// <param name="little"></param>
    /// <returns>LittleStar</returns>
    public int AddLittleStarBlue(int little)
    {
        return LittleStarBlue = little;
    }

    /// <summary>
    /// 大きい星(緑)
    /// </summary>
    /// <param name="Big"></param>
    /// <returns></returns>
    public int AddBigStarBlue(int Big)
    {
        return BigStarBlue = Big;
    }
}
