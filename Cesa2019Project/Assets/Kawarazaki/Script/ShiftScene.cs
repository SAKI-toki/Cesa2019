using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ShiftScene : MonoBehaviour
{
    void Start()
    {
        FadeController.FadeIn();
    }

    void Update()
    {
        GameOverScene();
        GameScene();
    }

    void GameOverScene()
    {
        if (SceneManager.GetActiveScene().name == "UiTestScene")
            if (PlayerController.PlayerStatus.CurrentHp == 0  )
            {
                FadeController.FadeOut("GameOverTestScene");
            }
    }
    
    void GameScene()
    {
        if(SceneManager.GetActiveScene().name == "GameOverTestScene")
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
            {
                FadeController.FadeOut("UiTestScene");
            }
    }
}
