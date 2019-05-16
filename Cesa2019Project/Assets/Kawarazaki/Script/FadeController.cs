using System.Collections;
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
    //アルファ値
    static float Alpha = 0.0f;

    //フェードインアウトのフラグ
    public static bool IsFadeOut = false;
    public static bool IsFadeIn = false;

    //偏移先のシーン名
    private static string NextSceneName;
    private static int NextSceneNumber;
    static bool NumberFlg = false;
    bool SceneTranslationPermission = false;

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
        SetAlpha();
    }

    private void Start()
    {
        StartCoroutine("LoadScene");
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
                SceneTranslationPermission = true;//シーン遷移を許可
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
    /// フェードアウトの実装
    /// </summary>
    private static void FadeOutImpl()
    {
        if (FadeImage == null) Init();
        FadeImage.enabled = true;
        IsFadeOut = true;
        Alpha = 0.0f;
        SetAlpha();
    }

    /// <summary>
    /// フェードアウト開始(string)
    /// </summary>
    /// <param name="n"></param>
    public static void FadeOut(string n)
    {
        FadeOutImpl();
        NextSceneName = n;
        NumberFlg = false;
    }

    /// <summary>
    /// フェードアウト開始(int)
    /// </summary>
    /// <param name="n"></param>
    public static void FadeOut(int n)
    {
        FadeOutImpl();
        NextSceneNumber = n;
        NumberFlg = true;
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad;
        if (NumberFlg)
            asyncLoad = SceneManager.LoadSceneAsync(NextSceneNumber);
        else
            asyncLoad = SceneManager.LoadSceneAsync(NextSceneName);

        asyncLoad.allowSceneActivation = false;//シーン遷移を許可しない
        while (asyncLoad.progress < 0.9f || !SceneTranslationPermission)
        {
            Debug.Log(asyncLoad.progress);
            yield return new WaitForEndOfFrame();
        }
        asyncLoad.allowSceneActivation = true;
    }


    /// <summary>
    /// FadeImageのカラー設定
    /// </summary>
    static void SetAlpha()
    {
        FadeImage.color = new Color(0, 0, 0, Alpha);
    }
}
