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
    public FaceAnimationController FaceAnimationController;
    public GameObject AttackEffect;
    [System.NonSerialized]
    public PlayerAudio PlayerAudio;
    [SerializeField]
    Pause Pause = null;

    [System.NonSerialized]
    public float ContactNormalY;// 着地時の接地面の法線ベクトル
    [System.NonSerialized]
    public bool MoveJump;
    [System.NonSerialized]
    public bool DamegFlg;
    [System.NonSerialized]
    public bool DeathFlg;
    int StopTime;
    bool StopFlg;

    void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
        //PlayerLastAttack = GetComponent<PlayerLastAttack>();
        PlayerCombo = GetComponent<PlayerCombo>();
        FaceAnimationController = GetComponent<FaceAnimationController>();
        PlayerAudio = GetComponent<PlayerAudio>();
        PlayerStatus.InitStatus(
            PlayerStatusData.Hp,
            PlayerStatusData.Attack,
            PlayerStatusData.Defense,
            PlayerStatusData.Speed,
            PlayerStatusData.Stamina);
        CurrentState.Init(this);
        StopTime = 0;
        StopFlg = false;
    }

    private void Start()
    {
        FaceAnimationController.FaceChange(FaceAnimationController.FaceTypes.Defalut);
    }

    void Update()
    {
        if (StarPlaceManager.StarSelect) { PlayerStop(); return; }
        if (Pause.GetPauseFlg()) { PlayerStop(); return; }
        if (StopFlg)
        {
            if (StopTime < 1)
            {
                ++StopTime;
                return;
            }
            else
            {
                StopTime = 0;
                StopFlg = false;
                PlayerAudio.AudioPauseStart();
            }
        }
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
            if (contact.normal.y > 0.3f && contact.normal.y <= 1.1f)
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

    public void GenerateAttackEffect()
    {
        float eulerAngleY = transform.eulerAngles.y;
        eulerAngleY *= Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Sin(eulerAngleY), 0, Mathf.Cos(eulerAngleY));
        Instantiate(AttackEffect, transform.position + dir * 3 + Vector3.up, transform.rotation);
    }

    public void Move(Vector3 dir, float speed)
    {
        PlayerAnimator.SetBool("DashFlg", true);
        dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
        Quaternion playerRotation = Quaternion.LookRotation(dir);
        PlayerRigidbody.AddForce(dir * speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, PlayerStatusData.RoteVal);
    }

    public void Look(Vector3 dir)
    {
        dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
        Quaternion playerRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, PlayerStatusData.RoteVal);
    }

    void PlayerStop()
    {
        if (!StopFlg)
        {
            PlayerAudio.AudioPause();
            StopFlg = true;
        }
    }
}
