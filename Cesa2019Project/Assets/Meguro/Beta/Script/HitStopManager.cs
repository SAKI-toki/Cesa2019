using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    [SerializeField]
    float TimeScaleNum = 0.1f;
    // 時間を遅くするフレーム数
    [SerializeField]
    int SlowFlame = 3;
    // 経過フレーム
    int ElapsedFlame = 0;
    bool IsSlowDown = false;

    void Update()
    {
        if (IsSlowDown)
        {
            if (ElapsedFlame >= SlowFlame)
            {
                SetNormalTime();
            }
            ++ElapsedFlame;
        }
    }

    /// <summary>
    /// 時間を遅らせる処理
    /// </summary>
    public void SlowDown()
    {
        ElapsedFlame = 0;
        Time.timeScale = TimeScaleNum;
        IsSlowDown = true;
    }
    /// <summary>
    /// 時間を元に戻す処理
    /// </summary>
    public void SetNormalTime()
    {
        Time.timeScale = 1f;
        IsSlowDown = false;
    }
}
