using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2 : IPlayerState
{
    // animation frame 24
    float NextAttackStartTime = 0.333f;// 攻撃判定 0:07～0:08 8/24 = 0.3...sec
    float NextAttackEndTime = 0.791f;// 戻り 0:19 19/24 = 0.7916...sec
    float AnimationTime;

    void IPlayerState.Init(Player player)
    {
        Player.PlayerStatus.CurrentAttack = Player.PlayerStatus.Attack + 10;
        AnimationTime = 0;
        player.PlayerAnimator.SetBool("Attack2", true);
    }

    IPlayerState IPlayerState.Update(Player player)
    {
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
        // Attack3
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
        {
            if (AnimationTime > NextAttackStartTime && AnimationTime < NextAttackEndTime)
            {
                return new PlayerAttack3();
            }
        }
        // 連続攻撃なし
        // アニメーション戻り 1:07 1 + 7/24 = 1.2916...sec
        if (AnimationTime > 1.291f)
        {
            return new PlayerIdle();
        }
        AnimationTime += Time.deltaTime;
        AttackMove(player);
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {
        player.PlayerAnimator.SetBool("Attack2", false);
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
