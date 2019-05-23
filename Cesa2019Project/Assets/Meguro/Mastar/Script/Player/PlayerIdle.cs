using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerIdle
/// </summary>
public class PlayerIdle : IPlayerState
{
    void IPlayerState.Init(Player player)
    {

    }

    IPlayerState IPlayerState.Update(Player player)
    {
        // Dash
        if (player.Controller.LeftStickH != 0 || player.Controller.LeftStickV != 0)
        {
            return new PlayerDash();
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
        // Clear
        if (StarPlaceManager.AllPlaceSet)
        {
            return new PlayerClear();
        }
        //// StarPut
        //if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.F))
        //{
        //    return new PlayerStarPut();
        //}
        PlayerMove(player);
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {

    }

    void PlayerMove(Player player)
    {
        player.PlayerRigidbody.AddForce(Vector3.down * player.PlayerStatusData.ForceGravity);
    }
}
