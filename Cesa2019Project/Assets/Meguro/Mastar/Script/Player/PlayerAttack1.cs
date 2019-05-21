using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerAttack1
/// </summary>
public class PlayerAttack1 : IPlayerState
{
    // animation frame 24
    float NextAttackStartTime = 0.416f; // 攻撃判定 0:09～0:10 10/24 = 0.416...sec
    float NextAttackEndTime = 0.75f;    // 戻り 0:18 18/24 = 0.75sec
    float AnimationTime;

    void IPlayerState.Init(Player player)
    {
        AnimationTime = 0;
        player.PlayerAnimator.SetBool("Attack1", true);
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        AnimationTime += Time.deltaTime;
        AttackMove(player);
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
        // Attack2
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
        {
            if (AnimationTime > NextAttackStartTime && AnimationTime < NextAttackEndTime)
            {
                return new PlayerAttack2();
            }
        }

        // 連続攻撃なし
        // アニメーション戻り 1:01 1 + 1/24 = 1.0416...sec
        if (AnimationTime > 1.041f)
        {
            return new PlayerIdle();
        }
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {
        player.PlayerAnimator.SetBool("Attack1", false);
    }

    void AttackMove(Player player)
    {
        if (player.Controller.LeftStickH != 0 || player.Controller.LeftStickV != 0)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * player.Controller.LeftStickV + Camera.main.transform.right * player.Controller.LeftStickH;
            player.PlayerRigidbody.AddForce(player.PlayerStatusData.AttackMoveSpeed * moveForward * player.ContactNormalY);
            Quaternion playerRotation = Quaternion.LookRotation(moveForward);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, playerRotation, player.PlayerStatusData.AttackRoteVal);
        }
    }
}
