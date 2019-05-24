using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void Init(Player player);
    IPlayerState Update(Player player);
    void Destroy(Player player);
}

public class Player : MonoBehaviour
{
    public IPlayerState CurrentState = new PlayerIdle();
    [System.NonSerialized]
    public Rigidbody PlayerRigidbody;
    [System.NonSerialized]
    public Animator PlayerAnimator;
    [System.NonSerialized]
    public AnimatorStateInfo PlayerAnimatorStateInfo;
    //[System.NonSerialized]
    //public PlayerLastAttack PlayerLastAttack;
    [System.NonSerialized]
    public PlayerCombo PlayerCombo;
    [System.NonSerialized]
    public Controller Controller = new Controller();
    public PlayerStatusData PlayerStatusData;
    public static Status PlayerStatus = new Status();
    public CameraController CameraController;

    [System.NonSerialized]
    public float ContactNormalY;// 着地時の接地面の法線ベクトル
    [System.NonSerialized]
    public bool MoveJump;
    [System.NonSerialized]
    public bool DamegFlg;
    [System.NonSerialized]
    public bool DeathFlg;

    public bool NotMove = true;

    void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
        //PlayerLastAttack = GetComponent<PlayerLastAttack>();
        PlayerCombo = GetComponent<PlayerCombo>();
        PlayerStatus.InitStatus(
            PlayerStatusData.Hp,
            PlayerStatusData.Attack,
            PlayerStatusData.Defense,
            PlayerStatusData.Speed,
            PlayerStatusData.Stamina);
        CurrentState.Init(this);
    }

    void Update()
    {
        Controller.Update();
        PlayerAnimatorStateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        PlayerAvoid.StaminaRecovering(PlayerStatusData);

        var nextState = CurrentState.Update(this);
        if (nextState != CurrentState)
        {
            CurrentState.Destroy(this);
            CurrentState = nextState;
            CurrentState.Init(this);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.1f && contact.normal.y <= 1)
            {
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
        if ((other.gameObject.tag == "EnemyAttack" || other.gameObject.tag == "LeoAttack") && !DamegFlg)
        {
            DamegFlg = true;
            PlayerCombo.ComboStop();
            float damege = Status.Damage(other.gameObject.transform.parent.gameObject.GetComponent<Enemy>().EnemyStatus.CurrentAttack, PlayerStatus.CurrentDefense);
            PlayerStatus.CurrentHp -= damege;
        }
        // ノックバック
        if (other.gameObject.tag == "LeoAttack")
        {
            Debug.Log("ノックバック");
            Vector3 force = this.transform.forward * -PlayerStatusData.Zforword;
            PlayerRigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}
