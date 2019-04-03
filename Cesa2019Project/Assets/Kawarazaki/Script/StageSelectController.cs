using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージセレクト
/// </summary>
public class StageSelectController : MonoBehaviour
{
    [SerializeField]
    GameObject SelectFream = null;

    [SerializeField]
    GameObject Spring = null;
    [SerializeField]
    GameObject Summer = null;
    [SerializeField]
    GameObject Autumn = null;
    [SerializeField]
    GameObject Winter = null;

    [SerializeField]
    GameObject SpringStageNumber = null;
    [SerializeField]
    GameObject SummerStageNumber = null;
    [SerializeField]
    GameObject AutumnStageNumber = null;
    [SerializeField]
    GameObject WinterStageNumber = null;

    //選択しているフレームのポジション
    float FreamPosX = 0.0f;

    //各シーズンのポジション
    float SpringPosX = -240.0f, SummerPosX = -80.0f, AutumnPosX = 80.0f, WinterPosX = 240.0f; 

    //各ステージのポジション
    float Stage1PosX = -200.0f, Stage2PosX = 0.0f, Stage3PosX = 200.0f;

    //ステージナンバー
    static int SeasonNumber = 1;
    static int StageNumber = 1;

    bool Seasonflg = true;
    static bool Stageflg = false;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Seasonflgがtrueだったらシーズンの選択
        if (Seasonflg)
            SelectSeason();
        //falseだったらステージの選択
        else
            SelectStage();
    }

    /// <summary>
    /// シーズン選択
    /// </summary>
    void SelectSeason()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --SeasonNumber;
            if (SeasonNumber < 1)
                SeasonNumber = 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++SeasonNumber;
            if (SeasonNumber > 4)
                SeasonNumber = 4;
        }

        switch (SeasonNumber)
        {
            case 1:
                FreamPosX = SpringPosX;
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    SeasonDelete();
                }
                StageActive(SpringStageNumber);
                break;
            case 2:
                FreamPosX = SummerPosX;
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    SeasonDelete();
                }
                StageActive(SummerStageNumber);
                break;
            case 3:
                FreamPosX = AutumnPosX;
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    SeasonDelete();
                }
                StageActive(AutumnStageNumber);
                break;
            case 4:
                FreamPosX = WinterPosX;
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    SeasonDelete();
                }
                StageActive(WinterStageNumber);
                break;
        }
        FreamPos(FreamPosX);
    }

    /// <summary>
    /// ステージセレクト
    /// </summary>
    void SelectStage()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --StageNumber;
            if (StageNumber < 1)
                StageNumber = 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++StageNumber;
            if (StageNumber > 3)
                StageNumber = 3;
        }

        switch (StageNumber)
        {
            case 1:
                FreamPosX = Stage1PosX;
                break;
            case 2:
                FreamPosX = Stage2PosX;
                break;
            case 3:
                FreamPosX = Stage3PosX;
                break;
        }
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
        {
            Stageflg = true;
        }

        if(Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Space))
        {
            SeasonActive();
        }

        FreamPos(FreamPosX);
    }

    /// <summary>
    /// 各ステージのアクティブを変更する
    /// </summary>
    /// <param name="gameObject"></param>
   void StageActive(GameObject gameObject)
    {
        if (Seasonflg)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    /// <summary>
    /// 各シーズンのアクティブをfalseにする
    /// </summary>
    void SeasonDelete()
    {
        Spring.SetActive(false);
        Summer.SetActive(false);
        Autumn.SetActive(false);
        Winter.SetActive(false);
        Seasonflg = false;
        FreamPosX = Stage1PosX;
    }

    /// <summary>
    /// 各シーズンのアクティブをtrueにする
    /// </summary>
    void SeasonActive()
    {
        Spring.SetActive(true);
        Summer.SetActive(true);
        Autumn.SetActive(true);
        Winter.SetActive(true);
        Seasonflg = true;
        FreamPosX = SpringPosX;
    }

    /// <summary>
    /// フレームのポジション設定
    /// </summary>
    /// <param name="FreamPosX"></param>
    void FreamPos(float FreamPosX)
    {
        SelectFream.GetComponent<RectTransform>().localPosition = new Vector3(FreamPosX, 0, 0);
    }


    public static bool GetStageFlg()
    {
        return Stageflg;
    }

    public static float GetSeasonNumber()
    {
        return SeasonNumber;
    }

    public static float GetStageNumber()
    {
        return StageNumber;
    }
}