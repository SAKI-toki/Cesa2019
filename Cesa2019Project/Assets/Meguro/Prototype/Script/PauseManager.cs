using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 停止処理を管轄する
/// </summary>
public class PauseManager : MonoBehaviour
{
    [SerializeField]
    GameObject PauseUI = null;
    float TimeScaleVal = 0;
    bool isPause = false;

    void Update()
    {
        // ポーズ
        if (Input.GetKeyDown("joystick button 7") && Time.timeScale != 0)
        {
            Pause();
        }
        // 再開
        else if (Input.GetKeyDown("joystick button 7") && isPause)
        {
            Play();
        }
    }

    /// <summary>
    /// ゲーム再開処理
    /// </summary>
    void Play()
    {
        isPause = false;
        Time.timeScale = TimeScaleVal;
        PauseUI.SetActive(false);
    }

    /// <summary>
    /// ゲームの停止処理
    /// </summary>
    void Pause()
    {
        isPause = true;
        TimeScaleVal = Time.timeScale;
        Time.timeScale = 0;
        PauseUI.SetActive(true);
    }
}
