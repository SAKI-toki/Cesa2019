using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// シーン遷移
/// </summary>
public class ShiftScene : MonoBehaviour
{
    int Time = 60;
    void Start()
    {
        FadeController.FadeIn();
    }

    void Update()
    {
        GameOverScene();
        GameScene();
        TitleScene();
        StageScene();
    }

    void GameOverScene()
    {
        if (SceneManager.GetActiveScene().name == "UiTestScene")
            if (PlayerController.PlayerStatus.CurrentHp <= 0  )
            {
                if (--Time <= 0)
                {
                    FadeController.FadeOut("GameOverTestScene");
                }
            }
    }
    
    void GameScene()
    {
        if(SceneManager.GetActiveScene().name == "GameOverTestScene")
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
            {
                FadeController.FadeOut("TitleTestScene");
            }
    }

    void TitleScene()
    {
        if (SceneManager.GetActiveScene().name == "TitleTestScene")
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
            {
                FadeController.FadeOut("StageSelectScene");
            }
    }

    void StageScene()
    {
        if(SceneManager.GetActiveScene().name== "StageSelectScene")
            if(StageSelectController.GetStageFlg())
            {
                FadeController.FadeOut("Stage" + StageSelectController.GetSeasonNumber() + "-" + StageSelectController.GetStageNumber());
            }
    }
}
