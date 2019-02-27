using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// 星のUI
/// </summary>
public class Star : MonoBehaviour
{
    //小さい星のテキスト
    [SerializeField]
    private TextMeshProUGUI LittleStarText;
    //大きい星のテキスト
    [SerializeField]
    private TextMeshProUGUI BigStarText;
    //小さい星
    [SerializeField]
    public int LittleStar;
    //大きい星
    [SerializeField]
    public int BigStar;

    Pause pause;
    int little;
    int Big;
    //初期化
    void Start()
    {
        LittleStar = 0;
        BigStar = 0;
        LittleStarText = GameObject.Find("LittleStar").GetComponent<TextMeshProUGUI>();
        BigStarText = GameObject.Find("BigStar").GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        //↓ここから
        //小さい星加算
        if (Input.GetKeyDown(KeyCode.Space))
        {
            little++;
            AddLittleStar(little);

            //小さい星が10個貯まったら大きい星加算
            if (LittleStar >= 10)
            {
                little -= 10;
                AddLittleStar(little);
                Big++;
                AddBigStar(Big);
            }
        }
        //↑ここまで繋げるときに消して

        //UIテキストに表示
        LittleStarText.text = "Little:" + LittleStar.ToString("00");
        BigStarText.text = "Big:" + BigStar.ToString("00");
    }

    /// <summary>
    /// 小さい星
    /// </summary>
    /// <param name="little"></param>
    /// <returns>LittleStar</returns>
    public int AddLittleStar(int little)
    {
        return LittleStar = little;
    }
    
    /// <summary>
    /// 大きい星
    /// </summary>
    /// <param name="Big"></param>
    /// <returns></returns>
    public int AddBigStar(int Big)
    {
        return BigStar = Big;
    }
}
