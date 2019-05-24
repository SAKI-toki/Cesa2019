using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStarPut : IPlayerState
{
    float AnimationTime;
    float NextAttackEndTime = 1.027f;// 戻り 3:20 3 + 20/24 = 3.083...sec

    void IPlayerState.Init(Player player)
    {
        AnimationTime = 0;
        player.PlayerAnimator.SetBool("StarPutFlg", true);
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        AnimationTime += Time.deltaTime;
        if (AnimationTime > NextAttackEndTime)
        {
            return new PlayerIdle();
        }
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {
        player.PlayerAnimator.SetBool("StarPutFlg", false);
    }
}
