using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの移動
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody Rigid;            // プレイヤーリジッドボディ
    Animator Ani;               // プレイヤーアニメータ

    float LeftStickH = 0;       // コントローラー左右
    float LeftStickV = 0;       // コントローラー上下
    float Trigger = 0;          // コントローラートリガー
    int StarHave = 0;           // 星の欠片を持っている数
    int BigStarHave = 0;        // 星を持っている数
    [SerializeField]
    float WalkVal = 0;          // 歩く速度
    [SerializeField]
    float DashVal = 0;          // ダッシュ速度
    [SerializeField]
    float JumpVal = 0;          // ジャンプ速度

    public static int AttackVal = 3;       // 攻撃力

    bool MoveDash;              // ダッシュのフラグ
    bool MoveJump;              // ジャンプのフラグ

    void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        Ani = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log("StarHave:" + StarHave);
        LeftStickH = Input.GetAxis("L_Stick_H");
        LeftStickV = Input.GetAxis("L_Stick_V");
        Trigger = Input.GetAxis("L_R_Trigger");
        if (Trigger > 0.8f || Input.GetKey(KeyCode.LeftShift)) { MoveDash = true; }
        if (Trigger < 0.8f || Input.GetKeyUp(KeyCode.LeftShift)) { MoveDash = false; }
        if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space)) && !MoveJump)
        {
            Ani.SetBool("Jumpflg", true);
            Rigid.AddForce(JumpVal * Vector3.up, ForceMode.Impulse);
            MoveJump = true;
        }
        if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.Return))
        {
            Ani.SetTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        if (LeftStickH != 0 || LeftStickV != 0)
        {
            Ani.SetBool("Walkflg", true);
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * LeftStickV + Camera.main.transform.right * LeftStickH;
            Quaternion playerRotation = Quaternion.LookRotation(moveForward);
            if (!MoveDash)
            {
                Rigid.AddForce(WalkVal * moveForward);
            }
            else if (MoveDash)
            {
                Rigid.AddForce(DashVal * moveForward);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, 0.1f);
        }
        else
        {
            Ani.SetBool("Walkflg", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Ani.SetBool("Jumpflg", false);
        MoveJump = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Star")
        {
            ++StarHave;
            Destroy(other.gameObject);
        }
    }
}
