using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDameg : IPlayerState
{
    // animation frame 24
    float AnimationTime;

    void IPlayerState.Init(Player player)
    {
        player.PlayerAnimator.SetBool("DamegFlg", true);
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        AnimationTime += Time.deltaTime;

        // アニメーション戻り 0:17 17/24 = 0.7083...sec
        if (AnimationTime > 0.708)
        {
            return new PlayerIdle();
        }
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {
        player.DamegFlg = false;
        player.PlayerAnimator.SetBool("DamegFlg", false);
    }
}
