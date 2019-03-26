using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    //フェード用のcanvasとImage
    private static Canvas FadeCanvas;
    private static Image FadeImage;

    //フェードしたい時間
    float FadeSpeed = 0.02f;

    static float Alfa = 0.0f;

    //フェードインアウトのフラグ
    public static bool IsFadeOut = false;
    public static bool IsFadeIn = false;

    //偏移先のシーン名
    private static string NextScene;
    
    //canvasとImage生成
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

        Alfa = FadeImage.color.a;
    }
    

    void Update()
    {
        //フェードイン
        if (IsFadeIn)
        {
            //透明度変化
            Alfa -=  FadeSpeed;

            //フェードイン終了判定
            if(Alfa <= 0.0f)
            {
                IsFadeIn = false;
                Alfa = 0.0f;
                FadeImage.enabled = false;
            }
            SetAlpha();
        }

        //フェードアウト
        else if(IsFadeOut)
        {
            //透明度変化
            Alfa +=  FadeSpeed;

            //フェードアウト終了判定
            if(Alfa >= 1.0f)
            {
                IsFadeOut = false;
                Alfa = 1.0f;

                //次のシーンへ偏移
                SceneManager.LoadScene(NextScene);
            }
            SetAlpha();
        }
    }

    //フェードイン開始
    public static void FadeIn()
    {
        if (FadeImage == null)
            Init();
        IsFadeIn = true;
    }

    //フェードアウト開始
   public static void FadeOut(string n)
    {
        if (FadeImage == null) Init();
        NextScene = n;
        FadeImage.enabled = true;
        IsFadeOut = true;
    }

    //カラー設定
    void SetAlpha()
    {
        FadeImage.color = new Color(255, 255, 255, Alfa);
    }
}
