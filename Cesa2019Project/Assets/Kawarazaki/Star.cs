using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 星のUI
/// </summary>
public class Star : MonoBehaviour
{
    //小さい星のテキスト
    [SerializeField]
    private Text LittleStarText;
    //大きい星のテキスト
    [SerializeField]
    private Text BigStarText;
    //小さい星
    [SerializeField]
    public int LittleStar;
    //大きい星
    [SerializeField]
    public int BigStar;

    //初期化
    void Start()
    {
        LittleStar = 0;
        BigStar = 0;
        LittleStarText = GameObject.Find("LittleStar").GetComponent<Text>();
        BigStarText = GameObject.Find("BigStar").GetComponent<Text>();
    }
    
    void Update()
    {
        //小さい星加算
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LittleStar++;
            //Debug.Log(LittleStar);
        }
        //小さい星が10個貯まったら大きい星加算
        if(LittleStar==10)
        {
            BigStar++;
            LittleStar = 0;
            //Debug.Log(BigStar);
        }
        //UIテキストに表示
        LittleStarText.text = "Little:" + LittleStar.ToString("00");
        BigStarText.text = "Big:" + BigStar.ToString("00");
    }
}
