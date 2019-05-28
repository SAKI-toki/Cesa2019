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
    [SerializeField]
    SelectSE SE = null;

    //UICanvas
    [SerializeField, Header("UIのCanvas")]
    GameObject UI = null;
    //MiniMap
    [SerializeField, Header("ミニマップ")]
    GameObject MiniMap = null;

    //背景のUI 
    [SerializeField, Header("ゲームオーバー時の背景")]
    Image GameOverPanel = null;
    [SerializeField, Header("リスタートテキストの背景")]
    Image ReStartImage = null;
    [SerializeField, Header("ステージセレクトテキストの背景")]
    Image StageSelectImage = null;

    //カーソル
    [SerializeField, Header("カーソル(赤)")]
    GameObject CarsorRed = null;
    [SerializeField, Header("カーソル(青)")]
    GameObject CarsorBlue = null;

    //テキスト
    [SerializeField, Header("ゲームオーバーテキスト")]
    TextMeshProUGUI GameOverText = null;
    [SerializeField, Header("リスタートテキスト")]
    TextMeshProUGUI ReStartText = null;
    [SerializeField, Header("ステージセレクトテキスト")]
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
            //ゲームオーバー時ゲーム画面のUIのアクティブをfalseにする
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
                //入力処理
                Clear.SelectStick(ReStart, StageSelect);
                Clear.SelectKeyInput(ReStart, StageSelect);
                switch (Clear.GetCarsor())
                {
                    //「リスタート」
                    case ReStart:
                        //カーソル(赤)trueにカーソル(青)をfalse
                        CarsorRed.SetActive(true);
                        CarsorBlue.SetActive(false);
                        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                        {
                            //決定音
                            SE.Dec();
                            /*=================================================*/
                            //遷移
                            /*=================================================*/
                            if (!FadeController.IsFadeOut)
                            {
                                FadeController.FadeOut(LoadScene.name);
                            }
                        }
                        break;
                    //「ステージセレクト」
                    case StageSelect:
                        CarsorRed.SetActive(false);
                        CarsorBlue.SetActive(true);
                        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                        {
                            //決定音
                            SE.Dec();
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