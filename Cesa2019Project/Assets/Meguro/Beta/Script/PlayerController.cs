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
    AnimatorStateInfo AniStateInfo;       // プレイヤー攻撃アニメータの情報取得
    public static Status PlayerStatus = new Status();// プレイヤーステータス
    public static AbnormalState PlayerAbnormalState = new AbnormalState();
    float LeftStickH = 0;                       // コントローラー左右
    float LeftStickV = 0;                       // コントローラー上下
    float Trigger = 0;                          // コントローラートリガー
    bool RTriggerActive = false;                // 右トリガーが押されている判定
    [SerializeField, Header("体力")]
    float Hp = 0;                               // 体力
    [SerializeField, Header("攻撃力")]
    float Attack = 0;                           // 攻撃力
    [SerializeField, Header("防御力")]
    float Defense = 0;                          // 防御力
    [SerializeField, Header("スピード")]
    float Speed = 0;                            // スピード
    [SerializeField, Header("スタミナ")]
    float Stamina = 0;                          // スタミナ
    [SerializeField, Header("ジャンプ速度")]
    float JumpVal = 0;                          // プレイヤーのジャンプ速度
    [SerializeField, Header("空中時のふわふわ感を無くす")]
    float ForceGravity = 0;                     // 空中時のふわふわ感を無くす
    [SerializeField, Header("通常時の回転速度")]
    float RoteVal = 0.1f;                       // プレイヤーの回転する速度
    [SerializeField, Header("攻撃時の回転速度")]
    float AttackRoteVal = 0.07f;                // 攻撃時の回転する速度
    [SerializeField, Header("ノックバック時の移動量")]
    float Zforword = 30;
    [SerializeField, Header("攻撃時の移動速度"), Range(0, 1)]
    float AttackMoveVal = 0;                    // 攻撃時の移動量
    public static Combo ComboController = new Combo(); // コンボスクリプト
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
    float ContactNormalY = 0;                   // 接地面の法線ベクトルのY
    Vector3 AvoidForce = Vector3.zero;          // 回避する力
    float AvoidCurrentTime = 0;
    [SerializeField, Header("回避開始までの溜め時間")]
    float StartAvoidTime = 0;                   // 回避開始までの溜め時間
    bool StartAvoidFlg = false;
    [SerializeField, Header("回避時間")]
    float AvoidTime = 0;                        // 回避時間
    bool AvoidFlg = false;
    [SerializeField, Header("回避後の硬直時間")]
    float EndAvoidTime = 0;                     // 回避後の硬直時間
    [SerializeField, Header("回避スピード")]
    float AvoidSpeed = 0;                       // 回避スピード
    [SerializeField, Header("回避の回復スピード")]
    float AvoidHealingTime = 0;                 // 回避の回復スピード
    float AvoidHealingCurrentTime = 0;
    bool MoveJump = false;                      // ジャンプのフラグ
    bool MoveAvoid = false;                     // 回避のフラグ
    public bool DeathFlg = false;                      // 死亡フラグ
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
        PlayerStatus.InitStatus(Hp, Attack, Defense, Speed, Stamina);   // ステータス初期化
        PlayerAbnormalState.Init(5, 10, 1, 5);                          // 異常状態初期化
        ComboController.InitCombo(4);           // コンボの時間
        PlayerAddAttackController.InitPlayerAddAttack(TimingCanvas, TimingCircle, TimingIcon, this.transform);
        PlayerRigid = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // コンボ処理
        ComboController.CheckCombo();
        // プレイヤーデバッグ
        if (Input.GetKeyDown(KeyCode.P)) { PlayerStatusDebugSwitch(); }
        DebugPlayerStatus();
        if (ComboController.ComboFlg)
        {
            ComboController.InCombo(ComboText);
            ComboController.ComboUI(ComboText, 255, 255, 255);
        }
        if (DeathFlg) { MoveStop(); return; }
        if (StarPlaceManager.AllPlaceSet) { MoveStop(); return; }
        // 異常状態
        PlayerAbnormalState.Abnormal(ref PlayerStatus.CurrentHp);
        if (PlayerAbnormalState.ParalysisFlg) { MoveStop(); return; }
        // プレイヤー攻撃アニメーションの情報更新
        AniStateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        // 移動
        LeftStickH = Input.GetAxis("L_Stick_H");
        LeftStickV = Input.GetAxis("L_Stick_V");
        // 回避
        Trigger = Input.GetAxis("L_R_Trigger");
        // ジャンプ
        if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space)) && !MoveJump && !MoveAvoid)
        {
            // 星選択中で操作させないため
            if (!StarPlaceManager.StarSelect)
            {
                //PlayerAnimator.SetBool("Jumpflg", true);
                PlayerRigid.AddForce(JumpVal * Vector3.up, ForceMode.Impulse);
                MoveJump = true;
            }
        }
        // 攻撃
        if ((Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return)) && !MoveAvoid)
        {
            // 星選択中で操作させないため
            if (!StarPlaceManager.StarSelect)
            {
                PlayerAnimator.SetTrigger("AttackTrigger");
            }
        }
        if (AniStateInfo.IsTag("Attack1"))
        {
            PlayerStatus.CurrentAttack = PlayerStatus.Attack;
        }
        if (AniStateInfo.IsTag("Attack2"))
        {
            PlayerStatus.CurrentAttack = PlayerStatus.Attack + 5;
        }
        if (AniStateInfo.IsTag("Attack3"))
        {
            PlayerStatus.CurrentAttack = PlayerStatus.Attack + 10;
        }
        // 攻撃のアニメーション中
        if (AniStateInfo.IsTag("Attack1") || AniStateInfo.IsTag("Attack2") || AniStateInfo.IsTag("Attack3"))
        {
            if (AniStateInfo.normalizedTime < 0.7f)
            {
                LeftStickH = Mathf.Clamp(LeftStickH, -AttackMoveVal, AttackMoveVal);
                LeftStickV = Mathf.Clamp(LeftStickV, -AttackMoveVal, AttackMoveVal);
            }
        }
        if (AniStateInfo.IsTag("Attack3") && !PlayerAddAttackController.TimingFlg)
        {
            PlayerAddAttackController.TimingUIAwake();
        }
        // スタミナ回復
        if (!MoveAvoid)
        {
            if (PlayerStatus.CurrentStamina < PlayerStatus.Stamina)
            {
                AvoidHealingCurrentTime += Time.deltaTime;
                if (AvoidHealingTime < AvoidHealingCurrentTime)
                {
                    ++PlayerStatus.CurrentStamina;
                    AvoidHealingCurrentTime = 0;
                }
            }
        }

        // 追加攻撃
        PlayerAddAttackController.TargetTracking();
        if (PlayerAddAttackController.TimingAttack())
        {
            Instantiate(AddAttackEffect, transform.position, transform.rotation);
        }

        // 死亡処理
        if (PlayerStatus.CurrentHp <= 0 && !DeathFlg)
        {
            Death();
        }

        // ステータス初期化
        if (Input.GetKeyDown(KeyCode.R)) { PlayerStatus.InitStatus(Hp, Attack, Defense, Speed, Stamina); }
    }

    void FixedUpdate()
    {
        PlayerRigid.AddForce(Vector3.down * ForceGravity, ForceMode.Acceleration);
        if ((LeftStickH != 0 || LeftStickV != 0) && !MoveAvoid)
        {
            PlayerAnimator.SetBool("RunFlg", true);
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * LeftStickV + Camera.main.transform.right * LeftStickH;
            Quaternion playerRotation = Quaternion.LookRotation(moveForward);
            // 移動
            PlayerRigid.AddForce(PlayerStatus.CurrentSpeed * moveForward * ContactNormalY);
            // 攻撃のアニメーション中
            if (AniStateInfo.IsTag("Attack1") || AniStateInfo.IsTag("Attack2") || AniStateInfo.IsTag("Attack3"))
            {
                // 攻撃時の回転
                transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, AttackRoteVal);
            }
            else
            {
                // 通常時の回転
                transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, RoteVal);
            }
            if (!MoveAvoid && TriggerButtonDown(Trigger, ref RTriggerActive) && PlayerStatus.CurrentStamina > 0)    // 右トリガー
            {
                MoveAvoid = true;
                --PlayerStatus.CurrentStamina;
                AvoidForce = moveForward.normalized;
            }
        }
        else
        {
            PlayerAnimator.SetBool("RunFlg", false);
        }
        // 回避
        if (MoveAvoid)
        {
            AvoidCurrentTime += Time.deltaTime;
            if (!StartAvoidFlg)
            {
                if (StartAvoidTime < AvoidCurrentTime)
                {
                    StartAvoidFlg = true;
                    AvoidCurrentTime = 0;
                }
            }
            else if (!AvoidFlg)
            {
                if (AvoidTime < AvoidCurrentTime)
                {
                    AvoidFlg = true;
                    AvoidCurrentTime = 0;
                }
                PlayerRigid.AddForce(AvoidForce * AvoidSpeed * ContactNormalY);
            }
            else
            {
                if (EndAvoidTime < AvoidCurrentTime)
                {
                    MoveAvoid = false;
                    StartAvoidFlg = false;
                    AvoidFlg = false;
                    AvoidCurrentTime = 0;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y <= 1 && contact.normal.y > 0.0f)
            {
                //PlayerAnimator.SetBool("Jumpflg", false);
                MoveJump = false;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            ContactNormalY = Mathf.Clamp(Mathf.Abs(contact.normal.y), 0.1f, 1);
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
            ComboController.ComboStop(ComboText);
            float damege = Status.Damage(other.gameObject.transform.parent.gameObject.GetComponent<Enemy>().EnemyStatus.CurrentAttack, PlayerStatus.CurrentDefense);
            PlayerStatus.CurrentHp -= damege;
        }
    }
    public void KnockBack()
    {
        Vector3 force = this.transform.forward * -Zforword;
        PlayerRigid.AddForce(force, ForceMode.Impulse);
    }

    void MoveStop()
    {
        LeftStickH = 0;
        LeftStickV = 0;
    }
    /// <summary>
    /// トリガーの押されたときだけの判定処理
    /// </summary>
    /// <param name="triggerNum"></param>
    /// <param name="triggerActive"></param>
    /// <returns></returns>
    bool TriggerButtonDown(float triggerNum, ref bool triggerActive)
    {
        if (!triggerActive && triggerNum > 0)
        {
            triggerActive = true;
            return true;
        }
        else if (triggerActive && triggerNum == 0)
        {
            triggerActive = false;
            return false;
        }
        else
            return false;
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    void Death()
    {
        DeathFlg = true;
        //PlayerAnimator.SetBool("Deathflg", true);
        LeftStickH = 0;
        LeftStickV = 0;
        ComboController.ComboUIHidden(ComboText);
        PlayerAddAttackController.TimingUIHidden();
    }

    public void Move(Vector3 dir, float speed)
    {
        PlayerAnimator.SetBool("RunFlg", true);
        dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
        Quaternion playerRotation = Quaternion.LookRotation(dir);
        PlayerRigid.AddForce(dir * speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, RoteVal);
    }

    public void Look(Vector3 dir)
    {
        dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
        Quaternion playerRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, RoteVal);
    }

    //public void ClearAniPlay()
    //{
    //    PlayerAnimator.SetBool("Clearflg", true);
    //}
    //public void ClearAniStop()
    //{
    //    PlayerAnimator.SetBool("Clearflg", false);
    //}

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
        PlayerStatusDebugText[4].text = "Stamina: " + PlayerStatus.CurrentStamina.ToString();
    }
}
