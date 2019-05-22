using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [SerializeField]
    Transform CameraObject = null;
    [SerializeField]
    CameraController CameraControll = null;
    [SerializeField]
    Player PlayerControll = null;
    [SerializeField]
    ClearManager Clear = null;

    //UICanvas
    [SerializeField]
    GameObject UI = null;
    //MiniMap
    [SerializeField]
    GameObject MiniMap = null;

    //背景のUI 
    [SerializeField]
    Image GameOverPanel = null;
    [SerializeField]
    Image ReStartImage = null;
    [SerializeField]
    Image StageSelectImage = null;

    //カーソル
    [SerializeField]
    GameObject CarsorRed = null;
    [SerializeField]
    GameObject CarsorBlue = null;

    //テキスト
    [SerializeField]
    TextMeshProUGUI GameOverText = null;
    [SerializeField]
    TextMeshProUGUI ReStartText = null;
    [SerializeField]
    TextMeshProUGUI StageSelectText = null;

    const int ReStart = 0;
    const int StageSelect = 1;

    bool SelectFlg = false;
    bool CameraMoveFlg = false;
    bool GameOverTextFlg = false;

    Scene LoadScene;

    void Start()
    {
        UI.SetActive(true);
        MiniMap.SetActive(true);
        CarsorRed.SetActive(false);
        CarsorBlue.SetActive(false);
        Clear.TextAlphaZero(GameOverText);
        Clear.ImageAlphaZero(GameOverPanel);
        Clear.ImageAlphaZero(ReStartImage);
        Clear.ImageAlphaZero(StageSelectImage);
        Clear.TextAlphaZero(ReStartText);
        Clear.TextAlphaZero(StageSelectText);
        LoadScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        //カメラの移動
        if (PlayerControll.DeathFlg)
        {
            UI.SetActive(false);
            MiniMap.SetActive(false);
            CameraControll.DeathRotation();
            if (!CameraMoveFlg)
            {
                if (CameraObject.localEulerAngles.x < 60)
                {
                    CameraControll.DeathMoveInit();
                    CameraControll.DeathMove();
                }
                else
                {
                    CameraMoveFlg = true;
                }
            }

            //ゲームオーバーテキストのフェードインと移動
            if (CameraMoveFlg && !GameOverTextFlg)
            {
                if (GameOverText.color.a < 1)
                {
                    Clear.TextFadeIn(GameOverText, 0.01f);
                }
                if (GameOverText.rectTransform.localPosition.y > 0)
                {
                    GameOverText.rectTransform.localPosition += new Vector3(0, -5, 0);
                }

                if (GameOverText.color.a >= 1 && GameOverText.rectTransform.localPosition.y <= 0)
                {
                    GameOverTextFlg = true;
                    CameraMoveFlg = false;
                }
            }

            //ゲームオーバー画面のUIとテキストの出現
            if (GameOverTextFlg)
            {
                Clear.TextFadeOut(GameOverText, 0.01f);
                if (GameOverText.color.a <= 0)
                {
                    Clear.ImageFadeIn(GameOverPanel, 0.05f);
                    Clear.ImageFadeIn(ReStartImage, 0.05f);
                    Clear.ImageFadeIn(StageSelectImage, 0.05f);
                    Clear.TextFadeIn(ReStartText, 0.05f);
                    Clear.TextFadeIn(StageSelectText, 0.05f);
                }
                if (GameOverText.color.a <= 0)
                {
                    CarsorRed.SetActive(true);
                    SelectFlg = true;
                }
            }
            //リスタートとステージセレクトの選択とシーン遷移
            if (SelectFlg)
            {
                Clear.SelectStick(ReStart, StageSelect);
                Clear.SelectKeyInput(ReStart, StageSelect);
                switch (Clear.GetCarsor())
                {
                    case ReStart:
                        CarsorRed.SetActive(true);
                        CarsorBlue.SetActive(false);
                        if (Input.GetKeyDown("joystick button 1"))
                        {
                            /*=================================================*/
                            //遷移
                            /*=================================================*/
                            if (!FadeController.IsFadeOut)
                            {
                                FadeController.FadeOut(LoadScene.name);
                            }
                        }
                        break;
                    case StageSelect:
                        CarsorRed.SetActive(false);
                        CarsorBlue.SetActive(true);
                        if (Input.GetKeyDown("joystick button 1"))
                        {
                            /*=================================================*/
                            //遷移
                            /*=================================================*/
                            if (!FadeController.IsFadeOut)
                            {
                                FadeController.FadeOut("SelectScene");
                            }
                        }
                        break;
                }
            }
        }
    }
}