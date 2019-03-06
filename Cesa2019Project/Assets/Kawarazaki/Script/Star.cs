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

    ////小さい星
    //[SerializeField]
    //public int LittleStarGreen,LittleStarRed,LittleStarBlue;
    ////大きい星
    //[SerializeField]
    //public int BigStarGreen,BigStarRed,BigStarBlue;

    const string LittleString = "Little:";
    const string BigString = "Big:";

    //public int LittleGreen,LittleRed,LittleBlue;
    //public int BigGreen,BigRed,BigBlue;
    //初期化
    void Start()
    {
        //LittleStarGreen = 0; LittleStarRed = 0;LittleStarBlue = 0;
        //BigStarGreen = 0; BigStarRed = 0; BigBlue = 0;
    }

    void FixedUpdate()
    {
        /////////////↓ここから
        ////小さい星加算
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    LittleGreen++;
        //    AddLittleStarGreen(LittleGreen);

        //    //小さい星が7個貯まったら大きい星加算
        //    if (LittleStarGreen >= Constant.ConstNumber.StarConversion)
        //    {
        //        LittleGreen -= Constant.ConstNumber.StarConversion;
        //        AddLittleStarGreen(LittleGreen);
        //        BigGreen++;
        //        AddBigStarGreen(BigGreen);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    LittleRed++;
        //    AddLittleStarRed(LittleRed);

        //    //小さい星が7個貯まったら大きい星加算
        //    if (LittleStarRed >= Constant.ConstNumber.StarConversion)
        //    {
        //        LittleRed -= Constant.ConstNumber.StarConversion;
        //        AddLittleStarRed(LittleRed);
        //        BigRed++;
        //        AddBigStarRed(BigRed);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    LittleBlue++;
        //    AddLittleStarBlue(LittleBlue);

        //    //小さい星が7個貯まったら大きい星加算
        //    if (LittleStarBlue >= Constant.ConstNumber.StarConversion)
        //    {
        //        LittleBlue -= Constant.ConstNumber.StarConversion;
        //        AddLittleStarBlue(LittleBlue);
        //        BigBlue++;
        //        AddBigStarBlue(BigBlue);
        //    }
        //}
        /////////////↑ここまで繋げるときに消して

        //UIテキストに表示
        //LittleStarGreenText.text = LittleString + LittleStarGreen.ToString("00");
        //BigStarGreenText.text = BigString + BigStarGreen.ToString("00");

        //LittleStarRedText.text = LittleString + LittleStarRed.ToString("00");
        //BigStarRedText.text = BigString + BigStarRed.ToString("00");

        //LittleStarBlueText.text = LittleString + LittleStarBlue.ToString("00");
        //BigStarBlueText.text = BigString + BigStarBlue.ToString("00");


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

    ///// <summary>
    ///// 小さい星(緑)
    ///// </summary>
    ///// <param name="little"></param>
    ///// <returns>LittleStar</returns>
    //public int AddLittleStarGreen(int little)
    //{
    //    return LittleStarGreen = little;
    //}
    
    ///// <summary>
    ///// 大きい星(緑)
    ///// </summary>
    ///// <param name="Big"></param>
    ///// <returns></returns>
    //public int AddBigStarGreen(int Big)
    //{
    //    return BigStarGreen = Big;
    //}

    ///// <summary>
    ///// 小さい星(赤)
    ///// </summary>
    ///// <param name="little"></param>
    ///// <returns>LittleStar</returns>
    //public int AddLittleStarRed(int little)
    //{
    //    return LittleStarRed = little;
    //}

    ///// <summary>
    ///// 大きい星(赤)
    ///// </summary>
    ///// <param name="Big"></param>
    ///// <returns></returns>
    //public int AddBigStarRed(int Big)
    //{
    //    return BigStarRed = Big;
    //}

    ///// <summary>
    ///// 小さい星(青)
    ///// </summary>
    ///// <param name="little"></param>
    ///// <returns>LittleStar</returns>
    //public int AddLittleStarBlue(int little)
    //{
    //    return LittleStarBlue = little;
    //}

    ///// <summary>
    ///// 大きい星(緑)
    ///// </summary>
    ///// <param name="Big"></param>
    ///// <returns></returns>
    //public int AddBigStarBlue(int Big)
    //{
    //    return BigStarBlue = Big;
    //}
}
