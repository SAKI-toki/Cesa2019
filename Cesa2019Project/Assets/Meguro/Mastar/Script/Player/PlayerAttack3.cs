using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack3 : IPlayerState
{
    // animation frame 24
    float AnimationTime;

    void IPlayerState.Init(Player player)
    {
        player.PlayerLastAttack.Init();
        AnimationTime = 0;
        player.PlayerAnimator.SetBool("Attack3", true);
    }

    IPlayerState IPlayerState.Update(Player player)
    {
        Player.PlayerStatus.CurrentAttack = Player.PlayerStatus.Attack + 20;
        AnimationTime += Time.deltaTime;
        player.PlayerLastAttack.LastAttackUpdate();
        AttackMove(player);
        // Death
        if (Player.PlayerStatus.CurrentHp <= 0)
        {
            player.PlayerLastAttack.UIHidden();
            return new PlayerDeath();
        }
        // Dameg
        if (player.DamegFlg)
        {
            player.PlayerLastAttack.UIHidden();
            return new PlayerDameg();
        }
        // 連続攻撃なし
        // アニメーション戻り 1:10 1 + 10/24 = 1.416...sec
        if (AnimationTime > 1.416f)
        {
            player.PlayerLastAttack.UIHidden();
            return new PlayerIdle();
        }
        return this;
    }

    void IPlayerState.Destroy(Player player)
    {
        player.PlayerAnimator.SetBool("Attack3", false);
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
