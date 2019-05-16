using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerStatusData",
    menuName = "ScriptableObject/PlayerStatusData",
    order = 1)]
public class PlayerStatusData : ScriptableObject
{
    [Header("体力")]
    public float Hp = 100;
    [Header("攻撃力")]
    public float Attack = 15;
    [Header("防御力")]
    public float Defense = 5;
    [Header("スピード")]
    public float Speed = 75;
    [Header("スタミナ")]
    public float Stamina = 6;
    [Header("ジャンプ速度")]
    public float JumpVal = 30;
    [Header("空中時のふわふわ感を無くす")]
    public float ForceGravity = 20;
    [Header("通常時の回転速度")]
    public float RoteVal = 0.1f;
    [Header("攻撃時の移動スピード")]
    public float AttackMoveSpeed = 20;
    [Header("攻撃時の回転速度")]
    public float AttackRoteVal = 0.07f;
    [Header("回避開始までの溜め時間")]
    public float StartAvoidTime = 0.1f;
    [Header("回避時間")]
    public float AvoidTime = 0.3f;
    [Header("回避後の硬直時間")]
    public float EndAvoidTime = 0.1f;
    [SerializeField, Header("回避スピード")]
    public float AvoidSpeed = 100;
    [Header("回避の回復スピード")]
    public float AvoidHealingTime = 6;
    [Header("ノックバック時の移動量")]
    public float Zforword = 30;
}
