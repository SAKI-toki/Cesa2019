//小林健一
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の移動とHP
/// 
/// </summary>

public class Enemy : MonoBehaviour
{
    GameObject Star = null;
    [SerializeField, Header("Animatorがない場合true")]
    bool NonAnimator = false;
    [SerializeField, Header("直接攻撃しない敵の場合true")]
    public bool NonDirectAttack = false;
    [SerializeField, Header("Bossの場合true")]
    bool BossEnemy = false;
    [SerializeField, Header("Bulletに付ける場合true")]
    bool Bullet = false;
    [SerializeField, Header("赤の星")]
    GameObject RedStar = null;
    [SerializeField, Header("青の星")]
    GameObject BlueStar = null;
    [SerializeField, Header("緑の星")]
    GameObject YellowStar = null;
    [SerializeField, Header("移動力")]
    public float ZMove = 5;//移動力
    [SerializeField]
    float KnockBackMove = 0.5f;
    [SerializeField, Header("敵のHP")]
    public float EnemyHp = 30;//HPを設定する
    [SerializeField, Header("敵の攻撃力")]
    float EnemyAttackPoint = 0;
    [SerializeField, Header("敵の防御力")]
    float EnemyDefence = 0;
    [SerializeField, Header("敵のHPの上昇幅")]
    float HpPlus = 2;
    [SerializeField, Header("敵の攻撃力の上昇幅")]
    float AttackPlus = 2;
    [SerializeField, Header("敵の防御力の上昇幅")]
    float DefencePlus = 2;
    [SerializeField, Header("敵の移動の上昇幅")]
    float MovePlus = 1;
    [SerializeField, Header("敵の移動速度の上限")]
    float MoveLimit = 12;
    [SerializeField, Header("敵の攻撃力の下降幅（％）")]
    float AttackDown = 20;
    [SerializeField, Header("敵の防御力の下降幅（％）")]
    float DefenceDown = 20;
    [SerializeField, Header("敵の移動力の下降幅（％）")]
    float MoveDown = 20;
    [SerializeField, Header("索敵範囲")]
    public float OnPlayerTracking = 10;//プレイヤーとの差が数値以下になったら追従開始
    [SerializeField, Header("移動後の待機時間")]
    public float Latency = 1;//待機時間
    [SerializeField, Header("攻撃のために止まる範囲")]
    public float AttackDecision = 2;
    [SerializeField, Header("攻撃判定を出す時間")]
    public float OutPutAttackDecision = 1;
    [SerializeField, Header("攻撃の硬直時間")]
    public float AttackWait = 3;//攻撃の硬直時間
    [SerializeField, Header("被弾時の硬直時間")]
    float Rigor_Cancellation = 3;//被弾時の硬直時間
    [SerializeField, Header("何回攻撃が当たったら怯むか")]
    int DamageCount = 3;
    [SerializeField, Header("星を出す最小数")]
    int MinStarCount = 1;
    [SerializeField, Header("星を出す最大数")]
    int MaxStarCount = 3;
    [SerializeField, Header("攻撃判定を出す位置")]
    public Vector3 Offset = new Vector3();
    [SerializeField, Header("AttackPrefabを入れる")]
    public GameObject AttackPrefab = null;
    HitStopManager HitStop = null;

    int StarRandom = 0;
    int StarCount = 20;
    int AttackCount = 0;
    int StatusUpCount = 1;
    int RedStarCount = 0;
    int BlueStarCount = 0;
    int GreenStarCount = 0;
    int StatusUpNum = 1;

    [SerializeField]
    public bool DamageFlag = false;//ダメージを受けたか
    [HideInInspector]
    public bool ReceivedDamage = false;//ダメージをうけたときtrue
    [HideInInspector]
    public bool PlayerTracking = false;//プレイヤーに追従してるときにtrue
    [HideInInspector]
    public bool AttackEnemy = false;//攻撃中か
    [HideInInspector]
    public float PlayerRangeDifference = 0;//プレイヤーと敵の距離差
    [HideInInspector]
    public float EnemyTime = 0;//敵の時間
    [HideInInspector]
    public float BossTime = 0;//Bossの時間
    [HideInInspector]
    public bool MoveSwitch;//前に移動する
    [HideInInspector]
    public Status EnemyStatus = new Status();
    [HideInInspector]
    public AbnormalState EnemyAbnormalState = new AbnormalState();
    [HideInInspector]
    public Vector3 TargetPos;
    [HideInInspector]
    public Animator Animator = null;
    [HideInInspector]
    public GameObject NearObj = null;//プレイヤーの位置取得
    [HideInInspector]
    public bool DestroyFlag = false;//撃破フラグを立てる
    [HideInInspector]
    public bool JampFlag = false;//ジャンプ中のフラグ
    [HideInInspector]
    public EnemySe EnemySe;
    public bool NoDamage = false;

    GameObject StarPlace = null;
    StarPlaceManager StarPlaceManager = null;
    //NavMeshAgent Agent = null;
    public PlayerController PlayerController;
    StarMove StarMove = null;
    Rigidbody GetRigidbody = null;

    /// <summary>
    /// 数値初期化
    /// </summary>
    void Start()
    {
        PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        EnemyStatus.Hp = EnemyHp;
        EnemyStatus.CurrentHp = EnemyHp;
        EnemyStatus.Attack = EnemyAttackPoint;
        EnemyStatus.Defense = EnemyDefence;
        EnemyStatus.Speed = ZMove;
        EnemyStatus.ResetStatus();
        AttackDown = 1 - (AttackDown * 0.01f);
        DefenceDown = 1 - (DefenceDown * 0.01f);
        MoveDown = 1 - (MoveDown * 0.01f);
        Animator = this.GetComponent<Animator>();
        NearObj = SearchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得  
        //Agent = GetComponent<NavMeshAgent>();
        EnemySe = this.GetComponent<EnemySe>();
        GetRigidbody = GetComponent<Rigidbody>();
        StarPlace = GameObject.Find("StarPlaceManager");
        StarPlaceManager = StarPlace.GetComponent<StarPlaceManager>();
        EnemyAbnormalState.Init(5, 10, 1, 5);
        HitStop = GameObject.Find("HitStopManager").GetComponent<HitStopManager>();
    }

    // Update is called once per frame

    /// <summary>
    /// 撃破判定
    /// ダメージを受けた時に硬直時間を発生させる
    /// </summary>
    void Update()
    {
        if (this.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }

        if (StarPlaceManager.AllPlaceSet)
        {
            Destroy(this.gameObject);
        }

        if (Time.timeScale == 0 || Bullet == true)
        {
            return;
        }
        EnemyAbnormalState.Abnormal(ref EnemyStatus.CurrentHp);
        EnemyTime += Time.deltaTime;
        BossTime += Time.deltaTime;
        //敵とプレイヤーの距離差
        PlayerRangeDifference = Vector3.Distance(NearObj.transform.position, this.transform.position);

        if (EnemyStatus.CurrentHp <= 0 || EnemyHp <= 0)
        {
            ++ClearManager.EnemyDownNum;
            if (BossEnemy == false) { EnemyStar(); }
            if (BossEnemy) { BossEnemyStar(); }
            DestroyFlag = true;
            Destroy(this.gameObject);//敵の消滅
        }

        if (ReceivedDamage == true)//硬直時間の解除
        {
            if (!NonDirectAttack && !NonAnimator) Animator.SetBool("EnemyWalk", false);
            if (EnemyTime >= Rigor_Cancellation)
            {
                JampFlag = true;
                ReceivedDamage = false;
                DamageFlag = false;
                EnemyTime = 0;
            }
        }

        /*
         * 毎フレーム実行しているためコメントアウト
         * by 石山
         */
        //StatusUp();//ステータスアップ
        //StatusDown();//ステータスダウン
    }

    /// <summary>
    /// 当たり判定とダメージ判定
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            if (!NoDamage)
            {
                HitStop.SlowDown();
                ++PlayerController.ComboController.CurrentComboNum;
                EnemyStatus.CurrentHp -= Status.Damage(PlayerController.PlayerStatus.CurrentAttack, EnemyStatus.CurrentDefense);//HPを減らす
                AttackCount++;
                if (!BossEnemy) { Animator.SetTrigger("EnemyDamage"); }
                EnemySe.DamageSES();
                transform.Translate(0, 0, -KnockBackMove);
            }


            if (EnemyStatus.CurrentHp <= 0)
            {
                EnemyStatus.CurrentHp = 0;
            }

            if (BossEnemy == false && ReceivedDamage == false)
            {
                DamageFlag = true;
                EnemyTime = 0;
                ReceivedDamage = true;/*敵を硬直させる*/
            }

            if (BossEnemy && AttackCount >= DamageCount && ReceivedDamage == false)
            {

                DamageFlag = true;
                EnemyTime = 0;
                ReceivedDamage = true;/*敵を硬直させる*/
                AttackCount = 0;
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            if (!NoDamage)
            {
                AttackCount++;
                ++PlayerController.ComboController.CurrentComboNum;
                EnemyStatus.CurrentHp -= 2;//HPを減らす
                if (EnemyStatus.CurrentHp <= 0)
                {
                    EnemyStatus.CurrentHp = 0;
                }
            }
        }
    }

    /// <summary>
    /// 星をランダムに生成
    /// </summary>
    void EnemyStar()
    {
        var randomStarNum = Random.Range(MinStarCount, MaxStarCount);

        for (int i = 0; i < randomStarNum; i++)//randomStarNumの分だけ星を生成
        {

            StarRandom = Random.Range(1, 4);//どの星を生成させるかきめる
            if (StarRandom == 1) { Star = RedStar; }//赤の星を生成させる
            if (StarRandom == 2) { Star = BlueStar; }//青の星を生成させる
            if (StarRandom == 3) { Star = YellowStar; }//黄の星を生成させる
            GameObject item = Instantiate(Star) as GameObject;//星の生成
            item.transform.position = transform.position;
            item.transform.Rotate(0, Random.Range(-180, 180), 0);
        }
    }

    /// <summary>
    /// 各星を５個づつ生成
    /// </summary>
    void BossEnemyStar()
    {

        for (int i = 0; i < StarCount; i++)//大きい星を生成できる分だけ星を生成
        {
            Star = RedStar;//赤の星を生成させる 
            GameObject item = Instantiate(Star) as GameObject;//星の生成
            StarMove = item.GetComponent<StarMove>();
            item.transform.position = transform.position;
            item.transform.Rotate(0, Random.Range(-180, 180), 0);
        }

        for (int i = 0; i < StarCount; i++)//大きい星を生成できる分だけ星を生成
        {
            Star = BlueStar;//青の星を生成させる
            GameObject item = Instantiate(Star) as GameObject;//星の生成
            StarMove = item.GetComponent<StarMove>();
            item.transform.position = transform.position;
            item.transform.Rotate(0, Random.Range(-180, 180), 0);
        }

        for (int i = 0; i < StarCount; i++)//大きい星を生成できる分だけ星を生成
        {
            Star = YellowStar;//黄の星を生成させる
            GameObject item = Instantiate(Star) as GameObject;//星の生成
            StarMove = item.GetComponent<StarMove>();
            item.transform.position = transform.position;
            item.transform.Rotate(0, Random.Range(-180, 180), 0);
        }
    }

    /// <summary>
    /// 星の数に応じてステータスアップ
    /// </summary>
    void StatusUp()
    {
        if (NonDirectAttack)
        {
            while (StatusUpCount <= StarPlaceManager.StarNum)
            {
                BuffHp();
                BuffDefence();
                if (StatusUpCount % 2 == 0)
                {
                    BuffAttack();
                    BuffMove();
                }
                StatusUpCount++;
            }

            while (StatusUpNum <= StarPlaceManager.StarNum)
            {
                BuffHp();
                BuffDefence();
                StatusUpNum++;
            }
        }
        else
        {
            while (StatusUpCount <= StarPlaceManager.StarNum)
            {
                BuffAttack();
                BuffHp();
                if (StatusUpCount % 2 == 0)
                {
                    BuffDefence();
                    BuffMove();
                }
                StatusUpCount++;
            }

            while (StatusUpNum <= StarPlaceManager.StarNum)
            {
                BuffHp();
                BuffAttack();
                StatusUpNum++;
            }
        }
    }

    /// <summary>
    /// ステータスダウン
    /// </summary>
    void StatusDown()
    {
        if (StarPlaceManager.RedStarNum == 0
            && StarPlaceManager.BlueStarNum == 0
            && StarPlaceManager.GreenStarNum == 0) { return; }

        while (RedStarCount < StarPlaceManager.RedStarNum)
        {
            DebuffAttack();
            RedStarCount++;
        }

        while (BlueStarCount < StarPlaceManager.BlueStarNum)
        {
            DebuffDefence();
            BlueStarCount++;
        }

        while (GreenStarCount < StarPlaceManager.GreenStarNum)
        {
            DebuffMove();
            GreenStarCount++;
        }
    }

    /// <summary>
    /// 敵の攻撃力をAttackDownの%分ダウン
    /// </summary>
    void DebuffAttack()
    {
        EnemyStatus.Attack *= AttackDown;
    }

    /// <summary>
    /// 敵の防御力をDefenceDownの%分ダウン
    /// </summary>
    void DebuffDefence()
    {
        EnemyStatus.Defense *= DefenceDown;
    }

    /// <summary>
    /// 敵の移動力MoveDownの%分ダウン
    /// </summary>
    void DebuffMove()
    {
        ZMove *= MoveDown;
    }

    /// <summary>
    /// 敵の攻撃力を上昇
    /// </summary>
    void BuffAttack()
    {
        EnemyStatus.Attack += AttackPlus;
    }

    /// <summary>
    /// 敵の防御力を上昇
    /// </summary>
    void BuffDefence()
    {
        EnemyStatus.Defense += DefencePlus;
    }

    /// <summary>
    /// 敵の移動速度を上昇
    /// </summary>
    void BuffMove()
    {
        ZMove += MovePlus;
        if (ZMove >= MoveLimit) { ZMove = MoveLimit; }
    }

    /// <summary>
    /// 敵のHPを上昇
    /// </summary>
    void BuffHp()
    {
        EnemyStatus.Hp += HpPlus;
    }

    /// <summary>
    /// 指定したtagのオブジェクトを拾得
    /// </summary>
    /// <param name="nowObj"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    GameObject SearchTag(GameObject nowObj, string tagName)//指定されたtagの中で最も近いものを取得
    {
        float tmpDis = 0;//距離用一時変数
        float nearDis = 0;//最も近いオブジェクトの距離
        GameObject targetObj = null;//オブジェクト
        //tag指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);//自身と取得したオブジェクトの距離を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }
        }
        return targetObj;
    }

}

