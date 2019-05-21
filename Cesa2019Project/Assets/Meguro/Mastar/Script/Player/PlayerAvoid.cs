using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvoid : IPlayerState
{
    float AvoidCurrentTime = 0;
    bool StartAvoidFlg = false;
    bool AvoidFlg = false;

    void IPlayerState.Init(Player player)
    {
        --Player.PlayerStatus.CurrentStamina;
        AvoidCurrentTime = 0;
        StartAvoidFlg = false;
        AvoidFlg = false;
    }


    IPlayerState IPlayerState.Update(Player player)
    {
        if (Avoid(player))
        {
            return new PlayerIdle();
        }
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {

    }

    /// <summary>
    /// 回避
    /// </summary>
    public bool Avoid(Player player)
    {
        AvoidCurrentTime += Time.deltaTime;
        if (!StartAvoidFlg)
        {
            if (player.PlayerStatusData.StartAvoidTime < AvoidCurrentTime)
            {
                StartAvoidFlg = true;
                AvoidCurrentTime = 0;
            }
        }
        else if (!AvoidFlg)
        {
            if (player.PlayerStatusData.AvoidTime < AvoidCurrentTime)
            {
                AvoidFlg = true;
                AvoidCurrentTime = 0;
            }
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * player.Controller.LeftStickV + Camera.main.transform.right * player.Controller.LeftStickH;
            player.PlayerRigidbody.AddForce(moveForward * player.PlayerStatusData.AvoidSpeed * player.ContactNormalY,ForceMode.VelocityChange);
        }
        else
        {
            if (player.PlayerStatusData.EndAvoidTime < AvoidCurrentTime)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// スタミナ回復
    /// </summary>
    public static float AvoidHealingCurrentTime = 0;
    public static void StaminaRecovering(PlayerStatusData playerStatusData)
    {
        if (Player.PlayerStatus.CurrentStamina < Player.PlayerStatus.Stamina)
        {
            AvoidHealingCurrentTime += Time.deltaTime;
            if (playerStatusData.AvoidHealingTime < AvoidHealingCurrentTime)
            {
                ++Player.PlayerStatus.CurrentStamina;
                AvoidHealingCurrentTime = 0;
            }
        }
    }
}