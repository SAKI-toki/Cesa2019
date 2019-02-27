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
    GameObject NearObj;//プレイヤーの位置取得
    [SerializeField]
    GameObject Star = null;
    [SerializeField, Header("移動力")]
    float ZMove = 5;//移動力
    [SerializeField, Header("敵のHP")]
    float EnemyHp = 30;//HPを設定する
    [SerializeField, Header("敵の攻撃力")]
    float EnemyAttackPoint = 0;
    [SerializeField, Header("敵の防御力")]
    float EnemyDefence = 0;
    [SerializeField, Header("索敵範囲")]
    float OnPlayerTracking = 10;//プレイヤーとの差が数値以下になったら追従開始
    [SerializeField, Header("移動後の待機時間")]
    float Latency = 1;//待機時間
    [SerializeField, Header("攻撃のために止まる範囲")]
    float AttackDecision = 2f;
    [SerializeField, Header("攻撃の硬直時間")]
    float AttackWait = 3;//攻撃の硬直時間
    [SerializeField, Header("被弾時の硬直時間")]
    float Rigor_Cancellation = 1;//被弾時の硬直時間
    [SerializeField, Header("スピードアタックするかどうか")]
    bool SpeedAttackFlag;
    [SerializeField, Header("直接攻撃しない敵の場合true")]
    bool NonDirectAttack = false;
    [SerializeField, Header("星を出す数")]
    int StarCount = 1;
    [SerializeField, Header("敵の回る速度")]
    float RotationPlus = 5f;
    [SerializeField, Header("RotationPlusが足される時間")]
    float RotateHours = 0.1f;
    [SerializeField, Header("攻撃判定を出す位置")]
    Vector3 Offset = new Vector3();
    [SerializeField]
    GameObject AttackPrefab = null;

    [SerializeField, Header("trueになったら破壊")]
    bool DestroyDebug = false;

    int RandomNumber;//ランダムな数値を入れる
    int DerectionLow;//ランダムの低値
    int DerectionHigh;//ランダムの高値
    int Ran1;//RandomNumberの値を保存
    int Ran2;//Ran1の値を保存
    int DrectionNumber;//方向に応じて数値を保存
    int RotationCount = 0;

    float PlayerRangeDifference;//プレイヤーと敵の距離差
    float EnemyTime;//敵の時間
    float RandomOn;//移動方向変更の時間
    float SpeedAttackTime;//スピードアタックの時間
    float MoveTimeLow;//移動している時間の低値
    float MoveTimeHigh;//移動している時間高値
    float AttackTime;//攻撃の時間
    float YPlus;//rotationを動かす角度
    float RotationTime = 0;//敵のrotation変更の時に使う時間

    bool ReceivedDamage;//ダメージをうけたときtrue
    bool PlayerTracking;//プレイヤーに追従してるときにtrue
    bool MoveSwitch;//前に移動する
    bool Wait;//待機状態が解けたか
    bool AttackEnemy;//攻撃中か
    bool AttackOn;//攻撃中か
    bool First = false;//一度だけ実行させる
    bool AttackFirst = false;

    Status EnemyStatus = new Status();

    Vector3 TargetPos;//
    Quaternion From;

    NavMeshAgent Agent = null;

    // Start is called before the first frame update

    /// <summary>
    /// 数値初期化
    /// </summary>
    void Start()
    {
        EnemyStatus.Hp = EnemyHp;
        EnemyStatus.Attack = EnemyAttackPoint;
        EnemyStatus.Defense = EnemyDefence;
        EnemyStatus.Speed = ZMove;
        EnemyStatus.ResetStatus();
        Wait = false;
        AttackOn = false;
        MoveTimeLow = 1.0f;
        MoveTimeHigh = 3.0f;
        DerectionLow = 1;
        DerectionHigh = 36;
        YPlus = RotationPlus;
        RandomOn = Random.Range(MoveTimeLow, MoveTimeHigh);
        NearObj = searchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得  
        Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame

    /// <summary>
    /// 撃破判定
    /// ダメージを受けた時に硬直時間を発生させる
    /// </summary>
    void Update()
    {
        if (Time.timeScale != 0)
        {
            EnemyTime += Time.deltaTime;

            PlayerRangeDifference = Vector3.Distance(NearObj.transform.position, this.transform.position);

            if (EnemyStatus.Hp <= 0 || DestroyDebug == true || EnemyHp <= 0)
            {
                for (int i = 0; StarCount != i; i++)
                {
                    GameObject item = Instantiate(Star) as GameObject;
                    item.transform.position = transform.position;
                    item.transform.Rotate(0, Random.Range(-180, 180), 0);
                }

                Destroy(this.gameObject);
            }

            if (ReceivedDamage == true)//硬直時間の解除
            {
                if (EnemyTime >= Rigor_Cancellation)
                {
                    ReceivedDamage = false;
                    EnemyTime = 0;
                }
            }

            if (PlayerRangeDifference <= AttackDecision && AttackOn == false && NonDirectAttack == false && ReceivedDamage == false)//攻撃中
            { AttackOn = true; }

            if (AttackOn == true) { Attack(); }

            if (ReceivedDamage == false && AttackEnemy == false)//ダメージを受けたら動かない,攻撃中も動かない
            {

                Move();

                DrectionChange();
            }
        }
    }

    /// <summary>
    /// 移動の制御
    /// </summary>
    void Move()
    {
        if (PlayerTracking == false)
        {
            //前に進む
            if (MoveSwitch) { transform.Translate(0, 0, ZMove * Time.deltaTime); }
        }

        TargetPos = NearObj.transform.position;
        //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
        TargetPos.y = this.transform.position.y;

        //敵の索敵範囲に入ったらプレイヤーに追従開始
        if (PlayerRangeDifference <= OnPlayerTracking)
        {
            MoveSwitch = false;
            PlayerTracking = true;
            transform.LookAt(TargetPos);//対象の位置方向を向く 
            transform.Translate(0, 0, ZMove * Time.deltaTime);

            SpeedAttack();
        }
        else
        {
            PlayerTracking = false;
        }
    }

    /// <summary>
    /// 一定間隔で方向を変えてます
    /// </summary>
    void DrectionChange()
    {

        RandomOn = Random.Range(MoveTimeLow, MoveTimeHigh);
        //一定間隔で移動方向を変更
        if (EnemyTime >= RandomOn && PlayerTracking == false)
        {
            MoveSwitch = false;
            Ran2 = Ran1;
            Ran1 = DrectionNumber;

            if (EnemyTime >= RandomOn + Latency)
            {
                RotationTime += Time.deltaTime;
                if (First == false)
                {
                    for (; DrectionNumber == Ran1 && DrectionNumber == Ran2;)
                    {
                        RandomNumber = Random.Range(DerectionLow, DerectionHigh);

                        //int random = Random.Range(2, 2);
                        RandomNumber = RandomNumber * -1;
                        if (RandomNumber >= 1 && RandomNumber <= 18) { DrectionNumber = 1; }
                        if (RandomNumber >= 19 && RandomNumber <= 36) { DrectionNumber = 2; }
                        if (RandomNumber >= -18 && RandomNumber <= -1) { DrectionNumber = 1; }
                        if (RandomNumber >= -36 && RandomNumber <= -19) { DrectionNumber = 3; }

                    }
                    if (RandomNumber >= 0)
                    {
                        if (YPlus <= 0) { YPlus = RotationPlus; }
                    }
                    if (RandomNumber <= 0)
                    {
                        RandomNumber = RandomNumber * -1;
                        YPlus = YPlus * -1;
                    }
                    First = true;
                }

                if (RandomNumber != RotationCount & RotationTime >= RotateHours)
                {
                    transform.Rotate(0, YPlus, 0);
                    RotationCount++;
                    if (RandomNumber == RotationCount)
                    {
                        First = false;
                        EnemyTime = 0;
                        MoveSwitch = true;
                        RotationCount = 0;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 一旦止まって高速で突撃する攻撃をするときに使う
    /// </summary>
    void SpeedAttack()
    {
        if (SpeedAttackFlag)
        {
            float taiki = 0.5f;
            if (PlayerRangeDifference <= taiki && Wait == false)
            {
                ZMove = 0;
                int speedTime = 1;
                SpeedAttackTime += Time.deltaTime;
                if (SpeedAttackTime >= speedTime)
                {
                    Wait = true;
                    SpeedAttackTime = 0;
                }
            }

            if (Wait == true)
            {
                ZMove = 0.7f;
                SpeedAttackTime += Time.deltaTime;
                float speedTime2 = 5;
                int speedAttackMove = 3;
                transform.LookAt(TargetPos);//対象の位置方向を向く 
                transform.Translate(0, 0, ZMove * speedAttackMove * Time.deltaTime);
                if (SpeedAttackTime >= speedTime2)
                {
                    Wait = false;
                    SpeedAttackFlag = false;
                }
            }
        }
    }

    /// <summary>
    /// 攻撃判定をだす
    /// </summary>
    void Attack()
    {
        AttackTime += Time.deltaTime;
        AttackEnemy = true;
        if (AttackFirst == false)
        {
            Vector3 position = transform.position + transform.up * Offset.y +
            transform.right * Offset.x +
            transform.forward * Offset.z;
            Instantiate(AttackPrefab, position, transform.rotation);
            AttackFirst = true;
        }
        if (AttackTime >= AttackWait)
        {
            AttackEnemy = false;
            AttackTime = 0;
            EnemyTime = 0;
            AttackOn = false;
            AttackFirst = false;
        }
    }

    /// <summary>
    /// 当たり判定とダメージ判定
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            EnemyStatus.Hp -= 10;//HPを減らす
            if (EnemyStatus.Hp <= 0)
            {
                EnemyStatus.Hp = 0;
            }

            ReceivedDamage = true;//敵を硬直させる
            EnemyTime = 0;
        }
    }

    /// <summary>
    /// プレイヤーの位置取得
    /// </summary>
    /// <param name="nowObj"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    GameObject searchTag(GameObject nowObj, string tagName)//指定されたtagの中で最も近いものを取得
    {
        float tmpDis = 0;//距離用一時変数
        float nearDis = 0;//最も近いオブジェクトの距離
        //string nearObjName="";//オブジェクト名称
        GameObject targetObj = null;//オブジェクト
        //tag指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);//自身と取得したオブジェクトの距離を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                //nearObjName=obs.name;
                targetObj = obs;
            }
        }
        //最も近かったオブジェクトを返す
        //return GameObject.Find(nearObjName);
        return targetObj;
    }
}







