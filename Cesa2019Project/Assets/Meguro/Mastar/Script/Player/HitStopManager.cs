using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    [SerializeField]
    Player Player = null;
    // 遅くなっているときのTime.timeScaleの値
    [SerializeField, Header("遅くなるスピード")]
    float TimeScaleNum = 0.1f;
    // 時間を遅くするフレーム数
    [SerializeField, Header("フレーム数")]
    int SlowFlame = 3;
    // 経過フレーム
    int ElapsedFlame = 0;
    // 遅くなっているときのフラグ
    bool IsSlowDown = false;
    [SerializeField]
    bool IsSlowAttack1 = false;
    [SerializeField]
    bool IsSlowAttack2 = false;
    [SerializeField]
    bool IsSlowAttack3 = false;

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

    public bool HitStopRestriction()
    {
        if (Player.PlayerAnimatorStateInfo.IsTag("Attack1") && IsSlowAttack1)
        {
            return true;
        }
        if (Player.PlayerAnimatorStateInfo.IsTag("Attack2") && IsSlowAttack2)
        {
            return true;
        }
        if (Player.PlayerAnimatorStateInfo.IsTag("Attack3") && IsSlowAttack3)
        {
            return true;
        }
        return false;
    }
}
