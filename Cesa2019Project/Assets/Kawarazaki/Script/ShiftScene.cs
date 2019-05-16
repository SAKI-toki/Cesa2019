using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// シーン遷移
/// </summary>
public class ShiftScene : MonoBehaviour
{
    int Time;
    void Start()
    {
        Time = 60;
        FadeController.FadeIn();
    }

    void Update()
    {
        TitleScene();
        StageScene();
        SelectScene();
        ResultScene();
        //GameOverScene();
    }

    /// <summary>
    /// ゲームオーバーシーンへ遷移
    /// </summary>
    void GameOverScene()
    {
        if (SceneManager.GetActiveScene().name == "UiTestScene")
        {
            if (Player.PlayerStatus.CurrentHp <= 0)
            {
                if (--Time <= 0)
                {
                    FadeController.FadeOut("GameOverTestScene");
                }
            }
        }
    }

    /// <summary>
    /// リザルトシーンへ遷移
    /// </summary>
    void ResultScene()
    {
        if(SceneManager.GetActiveScene().name=="UiTestScene")
            if(StarPlaceManager.AllPlaceSet)
                if(--Time <= 0)
                {
                    FadeController.FadeOut("ResultScene");
                }
    }

    /// <summary>
    /// タイトルシーンへ遷移
    /// </summary>
    void TitleScene()
    {
        if(SceneManager.GetActiveScene().name== "GameOverTestScene"|| SceneManager.GetActiveScene().name == "ResultScene")
        {
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
            {
                FadeController.FadeOut("TitleTestScene");
            }
        }
    }

    /// <summary>
    /// ステージシーンへ遷移
    /// </summary>
    void SelectScene()
    {
        if (SceneManager.GetActiveScene().name == "TitleTestScene")
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
            {
                FadeController.FadeOut("StageSelectScene");
            }
    }

    /// <summary>
    ///　各ステージシーンへ遷移
    /// </summary>
    void StageScene()
    {
        if(SceneManager.GetActiveScene().name== "StageSelectScene")
            if(StageSelectController.GetStageFlg())
            {
                FadeController.FadeOut("Stage" + StageSelectController.GetSeasonNumber() + "-" + StageSelectController.GetStageNumber());
            }
    }
}
