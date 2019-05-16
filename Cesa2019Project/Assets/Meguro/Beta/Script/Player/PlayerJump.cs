using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : IPlayerState
{
    void IPlayerState.Init(Player player)
    {
        player.PlayerAnimator.SetBool("JumpFlg", true);
        player.PlayerRigidbody.AddForce(player.PlayerStatusData.JumpVal * Vector3.up, ForceMode.Impulse);
        player.MoveJump = true;
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        JumpMove(player);
        // Death
        if (Player.PlayerStatus.CurrentHp <= 0)
        {
            return new PlayerDeath();
        }
        // Dameg
        if (player.DamegFlg)
        {
            return new PlayerDameg();
        }
        if (!player.MoveJump)
        {
            return new PlayerIdle();
        }
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {
        player.PlayerAnimator.SetBool("JumpFlg", false);
    }

    void JumpMove(Player player)
    {
        player.PlayerRigidbody.AddForce(Vector3.down * player.PlayerStatusData.ForceGravity, ForceMode.Acceleration);
        if (player.Controller.LeftStickH != 0 || player.Controller.LeftStickV != 0)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * player.Controller.LeftStickV + Camera.main.transform.right * player.Controller.LeftStickH;
            player.PlayerRigidbody.AddForce(Player.PlayerStatus.CurrentSpeed * moveForward * player.ContactNormalY);
            Quaternion playerRotation = Quaternion.LookRotation(moveForward);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, playerRotation, player.PlayerStatusData.RoteVal);
        }
    }
}
