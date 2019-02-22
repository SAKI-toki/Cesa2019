using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの移動
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody PlayerRigid;              // プレイヤーリジッドボディ
    Animator PlayerAnimator;            // プレイヤーアニメータ
    AnimatorStateInfo AniStateInfo;
    public static Status PlayerStatus = new Status();
    float LeftStickH = 0;               // コントローラー左右
    float LeftStickV = 0;               // コントローラー上下
    float Trigger = 0;                  // コントローラートリガー
    public static int StarPieceHave = 0;    // 星の欠片を持っている数
    public static int StarHave = 0;         // 星を持っている数
    [SerializeField]
    float WalkVal = 0;                  // 歩く速度
    [SerializeField]
    float JumpVal = 0;                  // ジャンプ速度

    public static int AttackVal = 3;    // 攻撃力

    bool MoveDash = false;              // ダッシュのフラグ
    bool MoveJump = false;              // ジャンプのフラグ

    void Awake()
    {
        PlayerStatus.Hp = 100;
        PlayerStatus.Attack = 15;
        PlayerStatus.Defense = 5;
        PlayerStatus.Speed = 20;
        PlayerStatus.ResetStatus();
        PlayerRigid = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
        StarPieceHave = 0;
        StarHave = 0;
    }

    void Update()
    {
        // プレイヤーアニメーションの情報
        AniStateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        if (!AniStateInfo.IsTag("PlayerAttack"))
        {
            // 移動
            LeftStickH = Input.GetAxis("L_Stick_H");
            LeftStickV = Input.GetAxis("L_Stick_V");
            // ダッシュ
            Trigger = Input.GetAxis("L_R_Trigger");
            if (Trigger > 0.8f || Input.GetKey(KeyCode.LeftShift)) { MoveDash = true; }
            if (Trigger < 0.8f || Input.GetKeyUp(KeyCode.LeftShift)) { MoveDash = false; }
            // ジャンプ
            if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space)) && !MoveJump)
            {
                // 星選択中で操作させないため
                if (!StarPlaceManager.StarSelect)
                {
                    PlayerAnimator.SetBool("Jumpflg", true);
                    PlayerRigid.AddForce(JumpVal * Vector3.up, ForceMode.Impulse);
                    MoveJump = true;
                }
            }
        }
        else if (AniStateInfo.IsTag("PlayerAttack"))
        {
            LeftStickH = 0;
            LeftStickV = 0;
            Debug.Log("PlayerAttack");
            // 攻撃時の補正
            AttackCorrection();
        }
        // 攻撃
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
        {
            // 星選択中で操作させないため
            if (!StarPlaceManager.StarSelect)
            {
                PlayerAnimator.SetTrigger("Attack");
            }
        }
        if (StarPieceHave >= 5)
        {
            StarPieceHave -= 5;
            ++StarHave;
        }
        if (Input.GetKeyDown("joystick button 2"))
        {
            PlayerStatus.CurrentSpeed += 2.0f;
        }
    }

    void FixedUpdate()
    {
        if (LeftStickH != 0 || LeftStickV != 0)
        {
            PlayerAnimator.SetBool("Walkflg", true);
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * LeftStickV + Camera.main.transform.right * LeftStickH;
            Quaternion playerRotation = Quaternion.LookRotation(moveForward);
            // 歩き
            if (!MoveDash)
            {
                PlayerRigid.AddForce(WalkVal * moveForward);
            }
            // ダッシュ
            else if (MoveDash)
            {
                PlayerRigid.AddForce(PlayerStatus.CurrentSpeed * moveForward);
            }
            // 回転
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, 0.1f);
        }
        else
        {
            PlayerAnimator.SetBool("Walkflg", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerAnimator.SetBool("Jumpflg", false);
        MoveJump = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StarPiece")
        {
            ++StarPieceHave;
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// 攻撃時の補正
    /// </summary>
    void AttackCorrection()
    {

    }
}
