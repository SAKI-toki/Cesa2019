using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 誕生日を入力するクラス
/// </summary>
public class InputBirthday : MonoBehaviour
{
    //誕生月
    int BirthMonth = 1;
    //誕生日
    int BirthDay = 1;
    [SerializeField, Header("誕生日を表示するテキスト")]
    Text BirthdayText = null;

    //現在の入力がどれか
    enum Birth { Month, Day, None };
    Birth CurrentInputBirth = Birth.Month;

    [SerializeField, Header("入力の間隔")]
    float InputDelay = 0.3f;
    float InputTime = 0.0f;

    //それぞれの月の最大日数を格納
    static Dictionary<int, int> MaxDayDictionary = new Dictionary<int, int>()
    {
        {1,31 }, {2,29 },{3,31 }, {4,30 }, {5,31 },{6,30 },
        {7,31 }, {8,31 }, {9,30 },{10,31 },{11,30 },{12,31 }
    };

    void Start()
    {
        UpdateText();
        InputTime = InputDelay;
    }

    void Update()
    {
        InputBirth();
        SwitchMonthDay();
        UpdateText();
    }

    /// <summary>
    /// テキストの更新
    /// </summary>
    void UpdateText()
    {
        if (CurrentInputBirth == Birth.Month)
        {
            BirthdayText.text = "誕生日:<color=#00ff00>" + BirthMonth.ToString("##") + "</color>月" + BirthDay.ToString("##") + "日";
        }
        else if (CurrentInputBirth == Birth.Day)
        {
            BirthdayText.text = "誕生日:" + BirthMonth.ToString("##") + "月<color=#00ff00>" + BirthDay.ToString("##") + "</color>日";
        }
    }

    /// <summary>
    /// 誕生日の入力
    /// </summary>
    void InputBirth()
    {
        if (InputTime > InputDelay)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                IncrementBirth(1);
                InputTime = 0.0f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                IncrementBirth(-1);
                InputTime = 0.0f;
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) 
        {
            InputTime += Time.deltaTime;
        }
        else
        {
            InputTime = InputDelay;
        }
        //１～１２月に値を収める
        BirthMonth = Mathf.Clamp(BirthMonth, 1, 12);
        //月に応じて値を収める
        BirthDay = Mathf.Clamp(BirthDay, 1, MaxDayDictionary[BirthMonth]);
    }

    /// <summary>
    /// 月と日を切り替える
    /// </summary>
    void SwitchMonthDay()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentInputBirth = Birth.Month;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentInputBirth = Birth.Day;
        }
    }

    /// <summary>
    /// 現在選択中のところを指定された値だけ加算(減算)する
    /// </summary>
    /// <param name="increment">加算(減算)する数</param>
    void IncrementBirth(int increment)
    {
        switch (CurrentInputBirth)
        {
            case Birth.Month:
                BirthMonth += increment;
                break;
            case Birth.Day:
                BirthDay += increment;
                break;
        }
    }
}
