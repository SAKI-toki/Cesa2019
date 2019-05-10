﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// フェード管理
/// </summary>
public class FadeController : MonoBehaviour
{
    //フェード用のcanvasとImage
    private static Canvas FadeCanvas;
    private static Image FadeImage;

    //フェードしたい時間
    float FadeSpeed = 0.02f;

    static float Alpha = 0.0f;

    //フェードインアウトのフラグ
    public static bool IsFadeOut = false;
    public static bool IsFadeIn = false;

    //偏移先のシーン名
    private static string NextScene;

    /// <summary>
    ///  canvasとImage生成
    /// </summary>
    static void Init()
    {
        //canvas生成
        GameObject FadeCanvasObject = new GameObject("Fade");
        FadeCanvas = FadeCanvasObject.AddComponent<Canvas>();
        FadeCanvasObject.AddComponent<GraphicRaycaster>();
        FadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        FadeCanvasObject.AddComponent<FadeController>();

        //最前面になるように設定
        FadeCanvas.sortingOrder = 100;

        //Image生成
        FadeImage = new GameObject("FadeImage").AddComponent<Image>();
        FadeImage.transform.SetParent(FadeCanvas.transform, false);
        FadeImage.rectTransform.anchoredPosition = Vector3.zero;

        //Imageのサイズ設定
        FadeImage.rectTransform.sizeDelta = new Vector2(9999, 9999);
    }


    void Update()
    {
        //フェードイン
        if (IsFadeIn)
        {
            //透明度変化
            Alpha -= FadeSpeed;

            //フェードイン終了判定
            if (Alpha <= 0.0f)
            {
                IsFadeIn = false;
                Alpha = 0.0f;
                FadeImage.enabled = false;
            }
            SetAlpha();
        }

        //フェードアウト
        else if (IsFadeOut)
        {
            //透明度変化
            Alpha += FadeSpeed;

            //フェードアウト終了判定
            if (Alpha >= 1.0f)
            {
                IsFadeOut = false;
                Alpha = 1.0f;

                //次のシーンへ偏移
                SceneManager.LoadScene(NextScene);
            }
            SetAlpha();
        }
    }

    /// <summary>
    /// フェードイン開始
    /// </summary>
    public static void FadeIn()
    {
        if (FadeImage == null)
            Init();
        IsFadeIn = true;
        Alpha = 1.0f;
        SetAlpha();
    }

    /// <summary>
    /// フェードアウト開始
    /// </summary>
    /// <param name="n"></param>
    public static void FadeOut(string n)
    {
        if (FadeImage == null) Init();
        NextScene = n;
        FadeImage.enabled = true;
        IsFadeOut = true;
        Alpha = 0.0f;
        SetAlpha();
    }

    /// <summary>
    /// FadeImageのカラー設定
    /// </summary>
    static void SetAlpha()
    {
        FadeImage.color = new Color(0, 0, 0, Alpha);
    }
}
