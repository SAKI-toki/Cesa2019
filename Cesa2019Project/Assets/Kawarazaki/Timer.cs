using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// カウントダウン処理
/// </summary>
public class Timer : MonoBehaviour
{
    //制限時間(分)
    [SerializeField]
    private int Minute;
    //制限時間(秒)
    [SerializeField]
    private float Seconds;
    //トータル制限時間
    private float TotalTime;
    //前回Update時の秒数
    private float OldSeconds;
    private Text TimerText;

    //タイマー初期化
    void Start()
    {
        TotalTime = Minute * 60 + Seconds;
        OldSeconds = 0.0f;
        TimerText = GetComponentInChildren<Text>();
    }
    
    void Update()
    {
        //制限時間が0秒以下なら何もしない
        if(TotalTime <= 0.0f)
        {
            return;
        }
        //トータルの制限時間を計測
        TotalTime = Minute * 60 + Seconds;
        TotalTime -= Time.deltaTime;

        //再設定
        Minute = (int)TotalTime / 60;
        Seconds = TotalTime - Minute * 60;

        //タイマー表示用UIテキストに時間を表示する
        if((int)Seconds != (int)OldSeconds)
        {
            TimerText.text = Minute.ToString("00") + ":" + ((int)Seconds).ToString("00");
        }
        OldSeconds = Seconds;

        //制限時間以下になった時の処理
        if(TotalTime <= 0.0f)
        {
            Debug.Log("制限時間終了");
        }
    }
}
