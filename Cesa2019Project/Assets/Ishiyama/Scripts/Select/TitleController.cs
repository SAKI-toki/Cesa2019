using UnityEngine;

/// <summary>
/// タイトルの制御
/// </summary>
public class TitleController : MonoBehaviour
{
    [SerializeField, Header("カメラの制御")]
    CameraController SelectCameraController = null;
    [SerializeField, Header("セレクト用のカメラ")]
    GameObject SelectCameraObject = null;
    [SerializeField, Header("セレクトから始動するプレイヤーの制御")]
    Player SelectPlayerController = null;
    [SerializeField, Header("タイトル用のカメラ")]
    GameObject TitleCameraObject = null;
    [SerializeField, Header("タイトルテキスト")]
    GameObject TitleTextObject = null;
    [SerializeField, Header("タイトルロゴのメッシュレンダラー")]
    MeshRenderer TitleLogoMeshRenderer = null;
    [SerializeField, Header("遷移のスピード"), Range(0.0f, 1.0f)]
    float TranslationSpeed = 1.0f;
    //タイトルロゴのアルファ値
    float TitleLogoAlpha = 1.0f;
    //ステートのデリゲート
    delegate void StateFunction();
    //ステート
    StateFunction Statefunc;
    //初期位置、回転
    Vector3 StartCameraPosition, EndCameraPosition;
    //終了位置、回転
    Quaternion StartCameraRotation, EndCameraRotation;
    //遷移時間
    float TranslationTime = 0.0f;
    const float TitleFadeSpeed = 1.0f;
    private void Start()
    {
        SelectCameraController.CameraInit();
        SelectCameraObject.SetActive(false);
        TitleCameraObject.SetActive(true);
        TitleTextObject.SetActive(true);
        SelectCameraController.enabled = false;
        SelectPlayerController.enabled = false;
        StartCameraPosition = TitleCameraObject.transform.position;
        StartCameraRotation = TitleCameraObject.transform.rotation;
        EndCameraPosition = SelectCameraObject.transform.position;
        EndCameraRotation = SelectCameraObject.transform.rotation;
        StarPlaceManager.AllPlaceSet = false;
        StarPlaceManager.StarSelect = false;
        Statefunc = FadeState;
        UpdateTitleLogoAlpha();
        FadeController.FadeIn();
    }
    void Update()
    {
        Statefunc();
    }

    /// <summary>
    /// フェードステート
    /// </summary>
    void FadeState()
    {
        //フェードが終了したらステート遷移
        if (!FadeController.IsFadeIn)
        {
            Statefunc = TitleState;
        }
    }

    /// <summary>
    /// タイトルステート
    /// </summary>
    void TitleState()
    {
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
        {
            Statefunc = TitleLogoFade;
        }
    }

    /// <summary>
    /// タイトルロゴのフェード
    /// </summary>
    void TitleLogoFade()
    {
        TitleLogoAlpha -= Time.deltaTime * TitleFadeSpeed;
        UpdateTitleLogoAlpha();
        if (TitleLogoAlpha <= 0.0f)
        {
            Statefunc = TranslationState;
            TitleTextObject.SetActive(false);
        }
    }

    /// <summary>
    /// 遷移ステート
    /// </summary>
    void TranslationState()
    {
        TranslationTime += Time.deltaTime * TranslationSpeed;
        TitleCameraObject.transform.position =
        Vector3.Lerp(StartCameraPosition, EndCameraPosition, TranslationTime);
        TitleCameraObject.transform.rotation =
        Quaternion.Lerp(StartCameraRotation, EndCameraRotation, TranslationTime);
        //着いたら終了
        if (TranslationTime >= 1.0f)
        {
            SelectCameraObject.SetActive(true);
            TitleCameraObject.SetActive(false);
            SelectCameraController.enabled = true;
            SelectPlayerController.enabled = true;
        }
    }

    void UpdateTitleLogoAlpha()
    {
        if (TitleLogoAlpha < 0.0f) TitleLogoAlpha = 0.0f;
        TitleLogoMeshRenderer.material.SetFloat("_Alpha", TitleLogoAlpha);
    }
}
