using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : IPlayerState
{
    void IPlayerState.Init(Player player)
    {
        player.FaceAnimationController.FaceChange(FaceAnimationController.FaceTypes.No);
        player.PlayerAnimator.SetBool("DeathFlg", true);
        player.PlayerRigidbody.isKinematic = true;
        player.DeathFlg = true;
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {

    }
}
