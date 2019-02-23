using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI TimerText;
    //制限時間(分)
    [SerializeField]
    private int Minute;
    //制限時間(秒)
    [SerializeField]
    private float Seconds;
    //前回Update時の秒数
    private float OldSeconds;

    //トータル制限時間
    private float TotalTime;

    //タイマー初期化
    void Start()
    {
        TotalTime = Minute * 60 + Seconds;
        OldSeconds = 0.0f;
        TimerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //制限時間が0秒以下なら何もしない
        if (TotalTime <= 0.0f)
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
        if ((int)Seconds != (int)OldSeconds)
        {
            TimerText.text = Minute.ToString("00") + ":" + ((int)Seconds).ToString("00");
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
            GetComponent<RectTransform>().localPosition = new Vector3(0, 200, 0);
            GetComponent<RectTransform>().localScale = new Vector3(2, 2, 0);
            TimerText.color = new Color(1, 0, 0, 1);
            TimerText.text = ((int)Seconds).ToString("0");
        }

        //制限時間以下になった時の処理
        if (TotalTime <= 0.0f)
        {
            TimerText.color = new Color(1, 1, 1, 1);
            TimerText.text = "END";
            Debug.Log("制限時間終了");
        }
    }
}
