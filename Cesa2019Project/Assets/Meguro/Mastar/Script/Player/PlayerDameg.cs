using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDameg : IPlayerState
{
    // animation frame 24
    float AnimationTime;

    void IPlayerState.Init(Player player)
    {
        player.FaceAnimationController.FaceChange(FaceAnimationController.FaceTypes.No);
        player.PlayerAnimator.SetBool("DamegFlg", true);
        player.PlayerAudio.AudioPlay(player.PlayerAudio.DamegAudio);
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        AnimationTime += Time.deltaTime;
        player.PlayerRigidbody.AddForce(Vector3.down * player.PlayerStatusData.ForceGravity);

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
        player.FaceAnimationController.FaceChange(FaceAnimationController.FaceTypes.Defalut);
        player.PlayerAnimator.SetBool("DamegFlg", false);
    }
}
