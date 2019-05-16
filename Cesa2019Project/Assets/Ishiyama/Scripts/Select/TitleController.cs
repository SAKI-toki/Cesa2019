﻿using UnityEngine;

/// <summary>
/// タイトルの制御
/// </summary>
public class TitleController : MonoBehaviour
{
    //二回目以降はタイトル画面を表示しないようにするためのフラグ
    static bool AlreadyStart = false;
    [SerializeField, Header("カメラの制御")]
    CameraController SelectCameraController = null;
    [SerializeField, Header("セレクト用のカメラ")]
    GameObject SelectCameraObject = null;
    [SerializeField, Header("セレクトから始動するプレイヤーの制御")]
    PlayerController SelectPlayerController = null;
    [SerializeField, Header("タイトル用のカメラ")]
    GameObject TitleCameraObject = null;
    [SerializeField, Header("タイトルテキスト")]
    GameObject TitleTextObject = null;
    [SerializeField, Header("遷移のスピード"), Range(0.0f, 1.0f)]
    float TranslationSpeed = 1.0f;
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
            if (AlreadyStart)
            {
                Statefunc = TranslationState;
                TitleTextObject.SetActive(false);
            }
            else
            {
                Statefunc = TitleState;
            }
        }
    }

    /// <summary>
    /// タイトルステート
    /// </summary>
    void TitleState()
    {
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
        {
            Statefunc = TranslationState;
            TitleTextObject.SetActive(false);
            AlreadyStart = true;
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
}
