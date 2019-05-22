using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerAttack1
/// </summary>
public class PlayerAttack1 : IPlayerState
{
    // animation frame 24
    float NextAttackStartTime = 0.458f; // 攻撃判定 0:09～0:10 11/24 = 0.4583...sec
    float NextAttackEndTime = 0.75f;    // 戻り 0:18 18/24 = 0.75sec
    float AnimationTime;
    bool IsAttackEffect;

    void IPlayerState.Init(Player player)
    {
        AnimationTime = 0;
        player.PlayerAnimator.SetBool("Attack1", true);
        IsAttackEffect = false;
        player.PlayerAudio.AudioPlay(player.PlayerAudio.Attack1Audio);
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        // AttackEffect
        if (AnimationTime > NextAttackStartTime && !IsAttackEffect)
        {
            IsAttackEffect = true;
            player.GenerateAttackEffect();
        }
        if(AnimationTime > NextAttackStartTime)
        {
            // Avoid
            if (player.Controller.TriggerButtonDown() && Player.PlayerStatus.CurrentStamina > 0)
            {
                return new PlayerAvoid();
            }
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
        AnimationTime += Time.deltaTime;
        AttackMove(player);
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {
        player.PlayerAnimator.SetBool("Attack1", false);
    }

    void AttackMove(Player player)
    {
        player.PlayerRigidbody.AddForce(Vector3.down * player.PlayerStatusData.ForceGravity);
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
