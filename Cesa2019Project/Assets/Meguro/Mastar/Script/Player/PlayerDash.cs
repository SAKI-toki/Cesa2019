using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerDash
/// </summary>
public class PlayerDash : IPlayerState
{
    void IPlayerState.Init(Player player)
    {
        player.PlayerAnimator.SetBool("DashFlg", true);
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        // Idle
        if (player.Controller.LeftStickH == 0 && player.Controller.LeftStickV == 0)
        {
            return new PlayerIdle();
        }
        // Jump
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space))
        {
            return new PlayerJump();
        }
        // Attack
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
        {
            return new PlayerAttack1();
        }
        // Avoid
        if (player.Controller.TriggerButtonDown() && Player.PlayerStatus.CurrentStamina > 0)
        {
            return new PlayerAvoid();
        }
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
        PlayerMove(player);
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {
        player.PlayerAnimator.SetBool("DashFlg", false);
    }

    void PlayerMove(Player player)
    {
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * player.Controller.LeftStickV + Camera.main.transform.right * player.Controller.LeftStickH;
        player.PlayerRigidbody.AddForce(Player.PlayerStatus.CurrentSpeed * moveForward * player.ContactNormalY);
        Quaternion playerRotation = Quaternion.LookRotation(moveForward);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, playerRotation, player.PlayerStatusData.RoteVal);
    }
}
