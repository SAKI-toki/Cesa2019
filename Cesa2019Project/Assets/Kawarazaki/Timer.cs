using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
/// タイマー
/// </summary>
public class Timer : MonoBehaviour
{
    private TextMeshProUGUI TimerText;
    //制限時間(分)
    [SerializeField, Header("分")]
    private int Minute;
    //制限時間(秒)
    [SerializeField, Header("秒")]
    private int Seconds;
    //前回Update時の秒数
    private int OldSeconds;

    //カウントダウンテキストポジション
    [SerializeField,Header("カウントダウンポジションX")]
    private float TextPosX;
    [SerializeField, Header("カウントダウンポジションY")]
    private float TextPosY;

    //カウントダウンテキストスケール
    [SerializeField, Header("カウントダウンスケールX")]
    private float TextScaleX;
    [SerializeField, Header("カウントダウンスケールY")]
    private float TextScaleY;


    //トータル制限時間
    private float TotalTime;

    //タイマー初期化
    void Start()
    {
        TotalTime = Minute * 60 + Seconds;
        OldSeconds = 0;
        TimerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //制限時間が0秒以下なら何もしない
        if (TotalTime <= 0.0f)
        {
            TotalTime = 0.0f;
            return;
        }
        //トータルの制限時間を計測
        TotalTime = Minute * 60 + Seconds;
        TotalTime -= Time.deltaTime;

        //再設定
        Minute = (int)TotalTime / 60;
        Seconds = (int)TotalTime % 60;

        //タイマー表示用UIテキストに時間を表示する
        if (Seconds != OldSeconds)
        {
            TimerText.text = Minute.ToString("00") + ":" + (Seconds).ToString("00");
        }
        OldSeconds = Seconds;

        //60秒切ったら文字の色を黄色に変更
        if (TotalTime <= 60.0f)
        {
            TimerText.color = Color.yellow;
        }

        //10秒切ったら文字の色を赤色に変更
        if (TotalTime <= 10.0f)
        {
            TimerText.color = Color.red;
        }

        //6秒切ったらテキストを画面中央に移動
        if (TotalTime <= 6.0f)
        {
            GetComponent<RectTransform>().localPosition = new Vector3(TextPosX, TextPosY, 0);
            GetComponent<RectTransform>().localScale = new Vector3(TextScaleX, TextScaleY, 0);
            TimerText.color = new Color(1, 0, 0, 1);
            TimerText.text = Seconds.ToString("0");
        }

        //制限時間になった時の処理
        if (TotalTime <= 1.0f)
        {
            TimerText.color = new Color(1, 1, 1, 1);
            TimerText.text = "END";
            Debug.Log("制限時間終了");
        }
    }
}
