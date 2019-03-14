using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コンボ
/// </summary>
public class Combo
{
    float ComboTime { get; set; }               // コンボが途切れる時間
    float CurrentComboTime { get; set; }        // 攻撃が当たってからの時間
    int ComboNum { get; set; }                  // 加算処理されないコンボ数
    public int CurrentComboNum { get; set; }    // 加算処理されるコンボ数
    public bool ComboFlg { get; private set; }  // コンボ中のフラグ

    /// <summary>
    /// コンボが途切れる時間を入れる
    /// </summary>
    /// <param name="comboTime"></param>
    public void InitCombo(float comboTime)
    {
        ComboTime = comboTime;
        ComboFlg = false;
    }

    /// <summary>
    /// コンボが続いているか判定
    /// </summary>
    public void CheckCombo()
    {
        if (CurrentComboNum != ComboNum)
        {
            ComboNum = CurrentComboNum;
            CurrentComboTime = 0;
            if (!ComboFlg) { ComboFlg = true; }
        }
    }

    /// <summary>
    /// コンボ中
    /// </summary>
    public void InCombo()
    {
        CurrentComboTime += Time.deltaTime;
        if (ComboTime < CurrentComboTime)
        {
            CurrentComboNum = 0;
            ComboNum = 0;
            ComboFlg = false;
        }
    }

    /// <summary>
    /// コンボUIの処理
    /// </summary>
    /// <param name="text"></param>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    public void ComboUI(Text text, float red, float green, float blue)
    {
        float alpa = (ComboTime - CurrentComboTime) / ComboTime;
        text.color = new Color(red, green, blue, alpa);
        text.text = "Combo:" + ComboNum.ToString();
    }
}
