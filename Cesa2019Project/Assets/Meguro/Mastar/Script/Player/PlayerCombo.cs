using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// プレイヤーコンボの処理
/// </summary>
public class PlayerCombo : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ComboText = null;             // コンボ数を表示するテキスト
    [SerializeField, Header("コンボが途切れる時間")]
    float ComboTime = 4;
    float CurrentComboTime;     // 攻撃が当たってからの時間
    int ComboNum;               // 加算処理されないコンボ数
    [System.NonSerialized]
    public int CurrentComboNum; // 加算処理されるコンボ数
    bool ComboFlg;              // コンボ中のフラグ

    void Awake()
    {
        ComboFlg = false;
    }

    void Update()
    {
        CheckCombo();
        if (ComboFlg)
        {
            InCombo();
            ComboUI();
        }
    }

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
    void InCombo()
    {
        CurrentComboTime += Time.deltaTime;
        if (ComboTime < CurrentComboTime)
        {
            ComboStop();
        }
    }

    /// <summary>
    /// コンボ中断
    /// </summary>
    public void ComboStop()
    {
        ComboText.color = new Color(255, 255, 255, 0);
        ComboFlg = false;
        CurrentComboNum = 0;
        ComboNum = 0;
    }

    /// <summary>
    /// コンボUIの処理
    /// </summary>
    public void ComboUI()
    {
        float alpa = (ComboTime - CurrentComboTime) / ComboTime;
        ComboText.color = new Color(ComboText.color.r, ComboText.color.g, ComboText.color.b, alpa);
        ComboText.text = "Combo:" + ComboNum.ToString();
    }
}
