using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの移動
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody PlayerRigid;                      // プレイヤーリジッドボディ
    Animator PlayerAnimator;                    // プレイヤーアニメータ
    AnimatorStateInfo AttackAniStateInfo;       // プレイヤー攻撃アニメータの情報取得
    public static Status PlayerStatus = new Status();// プレイヤーステータス
    float LeftStickH = 0;                       // コントローラー左右
    float LeftStickV = 0;                       // コントローラー上下
    float Trigger = 0;                          // コントローラートリガー
    [SerializeField, Header("ジャンプ速度")]
    float JumpVal = 0;                          // プレイヤーのジャンプ速度
    [SerializeField, Header("空中時のふわふわ感を無くす")]
    float ForceGravity = 0;                     // 空中時のふわふわ感を無くす
    [SerializeField, Header("通常時の回転速度")]
    float RoteVal = 0.1f;                       // プレイヤーの回転する速度
    [SerializeField, Header("攻撃時の回転速度")]
    float AttackRoteVal = 0.07f;                // 攻撃時の回転する速度
    public Combo ComboController = new Combo(); // コンボスクリプト
    [SerializeField, Header("コンボテキスト")]
    Text ComboText = null;                      // コンボテキスト
    PlayerAddAttack PlayerAddAttackController = new PlayerAddAttack();// 追加攻撃スクリプト
    [SerializeField, Header("タイミングUIのキャンバス")]
    GameObject TimingCanvas = null;
    [SerializeField, Header("タイミングのサークル")]
    Image TimingCircle = null;
    [SerializeField, Header("タイミングの中心")]
    Image TimingIcon = null;
    [SerializeField, Header("追加攻撃のエフェクト")]
    ParticleSystem AddAttackEffect = null;      // 追加攻撃エフェクト
    [SerializeField, Header("攻撃判定のオブジェクト")]
    GameObject AttackRange = null;              // 攻撃判定のオブジェクト
    //bool MoveDash = false;                    // ダッシュのフラグ
    bool MoveJump = false;                      // ジャンプのフラグ
    bool MoveStop = false;
    // debug用
    [SerializeField, Header("プレイヤーステータスデバッグUI")]
    GameObject PlayerStatusDebugUI = null;
    List<Text> PlayerStatusDebugText = new List<Text>();

    void Awake()
    {
        foreach (Transform child in PlayerStatusDebugUI.transform)
        {
            if (null != child.GetComponent<Text>())
            {
                PlayerStatusDebugText.Add(child.GetComponent<Text>());
            }
        }
        PlayerStatus.InitStatus(100, 5, 5, 50);// HP Attack Defense Speed
        ComboController.InitCombo(4);           // コンボの時間
        PlayerAddAttackController.InitPlayerAddAttack(TimingCanvas, TimingCircle, TimingIcon, this.transform);
        PlayerRigid = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // プレイヤー攻撃アニメーションの情報更新
        AttackAniStateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(1);
        // 移動
        LeftStickH = Input.GetAxis("L_Stick_H");
        LeftStickV = Input.GetAxis("L_Stick_V");
        if (LeftStickH != 0 || LeftStickV != 0)
        {
            PlayerAnimator.SetBool("Dashflg", true);
        }
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
        // 攻撃のアニメーション中
        if (AttackAniStateInfo.IsTag("Attack1") || AttackAniStateInfo.IsTag("Attack2") || AttackAniStateInfo.IsTag("Attack3"))
        {
            if (AttackAniStateInfo.normalizedTime < 0.7f)
            {
                LeftStickH = 0;
                LeftStickV = 0;
            }
        }
        if (AttackAniStateInfo.IsTag("Attack3") && !PlayerAddAttackController.TimingFlg)
        {
            PlayerAddAttackController.TimingUIAwake();
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

        // コンボ処理
        ComboController.CheckCombo();
        if (ComboController.ComboFlg)
        {
            ComboController.InCombo();
            ComboController.ComboUI(ComboText, 0, 0, 0);
        }

        // 追加攻撃
        PlayerAddAttackController.TargetTracking();
        if (PlayerAddAttackController.TimingAttack())
        {
            Instantiate(AddAttackEffect, transform.position, transform.rotation);
        }

        // プレイヤーデバッグ
        if (Input.GetKeyDown(KeyCode.P)) { PlayerStatusDebugSwitch(); }
        DebugPlayerStatus();
    }

    void FixedUpdate()
    {
        PlayerRigid.AddForce(Vector3.down * ForceGravity, ForceMode.Acceleration);
        if (LeftStickH != 0 || LeftStickV != 0)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * LeftStickV + Camera.main.transform.right * LeftStickH;
            Quaternion playerRotation = Quaternion.LookRotation(moveForward);
            // ダッシュ
            if (!MoveStop)
            {
                PlayerRigid.AddForce(PlayerStatus.CurrentSpeed * 10 * moveForward);
            }
            // 攻撃のアニメーション中
            if (AttackAniStateInfo.IsTag("Attack1") || AttackAniStateInfo.IsTag("Attack2") || AttackAniStateInfo.IsTag("Attack3"))
            {
                // 攻撃時の回転
                transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, AttackRoteVal);
            }
            else
            {
                // 通常時の回転
                transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, RoteVal);
            }
        }
        else
        {
            PlayerAnimator.SetBool("Dashflg", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y <= 1 && contact.normal.y > 0.1f)
            {
                PlayerAnimator.SetBool("Jumpflg", false);
                MoveJump = false;
                MoveStop = false;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // ジャンプ中に壁に当たったら動きを止める
        if (MoveJump)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal != Vector3.up)
                {
                    MoveStop = true;
                }
                else
                {
                    MoveStop = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 星の欠片取得
        if (other.gameObject.tag == "StarPiece")
        {
            HaveStarManager.AddLittleStar(other.gameObject.GetComponent<StarMove>().GetColor());
            Destroy(other.gameObject);
        }

        // プレイヤーのHPを減らす
        if (other.gameObject.tag == "EnemyAttack")
        {
            float damege = Status.Damage(other.gameObject.transform.parent.gameObject.GetComponent<Enemy>().EnemyStatus.CurrentAttack, PlayerStatus.CurrentDefense);
            Debug.Log("player,Hp: -" + damege);
            PlayerStatus.CurrentHp -= damege;
        }
    }

    /// <summary>
    /// デバッグUIの表示・非表示
    /// </summary>
    void PlayerStatusDebugSwitch()
    {
        // 表示
        if (PlayerStatusDebugUI.activeInHierarchy == false) { PlayerStatusDebugUI.SetActive(true); }
        // 非表示
        else if (PlayerStatusDebugUI.activeInHierarchy == true) { PlayerStatusDebugUI.SetActive(false); }
    }

    /// <summary>
    /// デバッグ用Textの更新
    /// </summary>
    void DebugPlayerStatus()
    {
        PlayerStatusDebugText[0].text = "Hp:      " + PlayerStatus.CurrentHp.ToString();
        PlayerStatusDebugText[1].text = "Attack:  " + PlayerStatus.CurrentAttack.ToString();
        PlayerStatusDebugText[2].text = "Defence: " + PlayerStatus.CurrentDefense.ToString();
        PlayerStatusDebugText[3].text = "Speed:   " + PlayerStatus.CurrentSpeed.ToString();
    }
}
