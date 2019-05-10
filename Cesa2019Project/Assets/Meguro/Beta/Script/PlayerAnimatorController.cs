using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController
{
    public Animator PlayerAnimator;
    public AnimatorStateInfo AniStateInfo;

    public void Init(Animator animator)
    {
        PlayerAnimator = animator;
    }

    public void Update()
    {
        AniStateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
    }

    public void AttackAniState()
    {
        // 星選択中で操作させないため
        if (!StarPlaceManager.StarSelect)
        {
            // 攻撃をしていなかったらAttack1
            if (NotAttacking())
            {
                PlayerAnimator.SetBool("Attack1", true);
            }
            // Attack2
            else if (AniStateInfo.IsTag("Attack1"))
            {
                if (AniStateInfo.normalizedTime > 0.3f && AniStateInfo.normalizedTime < 0.9f)
                {
                    PlayerAnimator.SetBool("Attack2", true);
                }
            }
            // Attack 3
            else if (AniStateInfo.IsTag("Attack2"))
            {
                if (AniStateInfo.normalizedTime > 0.3f && AniStateInfo.normalizedTime < 0.9f)
                {
                    PlayerAnimator.SetBool("Attack3", true);
                }
            }
        }
    }

    public void AttackAniStop()
    {
        if (Attacking() && AniTime() > 0.95f)
        {
            if (PlayerAnimator.GetBool("Attack1"))
            {
                PlayerAnimator.SetBool("Attack1", false);
            }
            if (PlayerAnimator.GetBool("Attack2"))
            {
                PlayerAnimator.SetBool("Attack2", false);
            }
            if (PlayerAnimator.GetBool("Attack3"))
            {
                PlayerAnimator.SetBool("Attack3", false);
            }
        }
    }

    /// <summary>
    /// 攻撃中
    /// </summary>
    /// <returns></returns>
    public bool Attacking()
    {
        if (AniStateInfo.IsTag("Attack1") || AniStateInfo.IsTag("Attack2") || AniStateInfo.IsTag("Attack3"))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 攻撃以外のとき
    /// </summary>
    /// <returns></returns>
    public bool NotAttacking()
    {
        if (!AniStateInfo.IsTag("Attack1") && !AniStateInfo.IsTag("Attack2") && !AniStateInfo.IsTag("Attack3"))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 現在行っているアニメーションの時間
    /// </summary>
    /// <returns></returns>
    public float AniTime()
    {
        return AniStateInfo.normalizedTime;
    }
}
