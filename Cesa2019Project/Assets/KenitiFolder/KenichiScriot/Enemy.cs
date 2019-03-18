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
    Animator Animator = null;
    GameObject NearObj = null;//プレイヤーの位置取得
    GameObject Star = null;
    GameObject AttackObject = null;
    [SerializeField, Header("直接攻撃しない敵の場合true")]
    bool NonDirectAttack = false;
    [SerializeField, Header("Bulletに付ける場合true")]
    bool Bullet = false;
    [SerializeField, Header("赤の星")]
    GameObject RedStar = null;
    [SerializeField, Header("青の星")]
    GameObject BlueStar = null;
    [SerializeField, Header("緑の星")]
    GameObject YellowStar = null;
    [SerializeField, Header("蟹")]
    bool CrabFlag = false;
    [SerializeField, Header("蟹が方向を変える時間")]
    float CrabMoveChange = 5;
    [SerializeField, Header("Bossエネミーの時にtrue")]
    bool BossEnemy = false;
    [SerializeField, Header("移動力")]
    float ZMove = 5;//移動力
    [SerializeField, Header("敵のHP")]
    float EnemyHp = 30;//HPを設定する
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
    float Latency = 1;//待機時間
    [SerializeField, Header("攻撃のために止まる範囲")]
    float AttackDecision = 2f;
    [SerializeField, Header("攻撃判定を出す時間")]
    float OutPutAttackDecision = 1;
    [SerializeField, Header("攻撃の硬直時間")]
    float AttackWait = 3;//攻撃の硬直時間
    [SerializeField, Header("被弾時の硬直時間")]
    float Rigor_Cancellation = 3;//被弾時の硬直時間
    [SerializeField, Header("何回攻撃が当たったら怯むか")]
    int DamageCount = 3;
    [SerializeField, Header("スピードアタックするかどうか")]
    bool SpeedAttackFlag;
    [SerializeField, Header("星を出す最小数")]
    int MinStarCount = 1;
    [SerializeField, Header("星を出す最大数")]
    int MaxStarCount = 3;
    [SerializeField, Header("敵の回る速度")]
    float RotationPlus = 5f;
    [SerializeField, Header("回転が足される時間")]
    float RotateHours = 0.1f;
    [SerializeField, Header("攻撃判定を出す位置")]
    Vector3 Offset = new Vector3();
    [SerializeField, Header("AttackPrefabを入れる")]
    GameObject AttackPrefab = null;

    [SerializeField, Header("trueになったら破壊")]
    bool DestroyDebug = false;

    int RandomNumber = 0;//ランダムな数値を入れる
    int DerectionLow = 1;//ランダムの低値
    int DerectionHigh = 36;//ランダムの高値
    int Ran1 = 0;//RandomNumberの値を保存
    int Ran2 = 0;//Ran1の値を保存
    int DrectionNumber = 0;//方向に応じて数値を保存
    int RotationCount = 0;
    int StarRandom = 0;
    int StarCount = 5;
    int AttackCount = 0;
    int MoveDouble = 3;
    int StatusUpCount = 0;

    float PlayerRangeDifference = 0;//プレイヤーと敵の距離差
    float EnemyTime = 0;//敵の時間
    float RandomOn = 0;//移動方向変更の時間
    float SpeedAttackTime = 0;//スピードアタックの時間
    float MoveTimeLow = 1.0f;//移動している時間の低値
    float MoveTimeHigh = 3.0f;//移動している時間高値
    float AttackTime = 0;//攻撃の時間
    float YPlus = 0;//rotationを動かす角度
    float RotationTime = 0;//敵のrotation変更の時に使う時間
    float MoveChange = 1;
    float BossTime = 0;//Bossの時間

    [HideInInspector]
    public bool ReceivedDamage = false;//ダメージをうけたときtrue
    bool PlayerTracking = false;//プレイヤーに追従してるときにtrue
    bool MoveSwitch;//前に移動する
    bool Wait = false;//待機状態が解けたか
    bool AttackEnemy = false;//攻撃中か
    bool AttackOn = false;//攻撃中か
    bool First = false;//一度だけ実行させる
    bool AttackFirst = false;//攻撃を一度だけ実行
    bool AttackMotionFirst = false;//攻撃モーションを一度だけ実行
    bool DamageFlag = false;//ダメージを受けたか
    bool CrabFirst = true;//移動速度を一度だけ上げる

    public Status EnemyStatus = new Status();

    Vector3 TargetPos;

    NavMeshAgent Agent = null;

    // Start is called before the first frame update

    /// <summary>
    /// 数値初期化
    /// </summary>
    void Start()
    {
        EnemyStatus.Hp = EnemyHp;
        EnemyStatus.CurrentHp = EnemyHp;
        EnemyStatus.Attack = EnemyAttackPoint;
        EnemyStatus.Defense = EnemyDefence;
        EnemyStatus.Speed = ZMove;
        EnemyStatus.ResetStatus();
        AttackDown = 1 - (AttackDown * 0.01f);
        DefenceDown = 1 - (DefenceDown * 0.01f);
        MoveDown = 1 - (MoveDown * 0.01f);
        YPlus = RotationPlus;
        Animator = this.GetComponent<Animator>();
        RandomOn = Random.Range(MoveTimeLow, MoveTimeHigh);
        NearObj = SearchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得  
        Agent = GetComponent<NavMeshAgent>();
        WaveController.EnemyCount += 1;

    }

    // Update is called once per frame

    /// <summary>
    /// 撃破判定
    /// ダメージを受けた時に硬直時間を発生させる
    /// </summary>
    void Update()
    {
        if(this.transform.childCount==0)
        {
            Destroy(this.gameObject);
        }

        if (Time.timeScale == 0||Bullet==true)
        {
            return;
        }

        EnemyTime += Time.deltaTime;
        //敵とプレイヤーの距離差
        PlayerRangeDifference = Vector3.Distance(NearObj.transform.position, this.transform.position);

        if (EnemyStatus.CurrentHp <= 0 || DestroyDebug == true || EnemyHp <= 0)
        {
            if (BossEnemy == false) { EnemyStar(); }
            if (BossEnemy) { BossEnemyStar(); }
            WaveController.EnemyCount -= 1;
            Destroy(this.gameObject);//敵の消滅
        }

        //蟹座のボスの時HPが１/３になったら移動速度三倍
        if (EnemyStatus.Hp / 3 >= EnemyStatus.CurrentHp && CrabFirst && CrabFlag)
        {
            ZMove = MoveDouble * ZMove;
            CrabFirst = false;
        }

        if (ReceivedDamage == true)//硬直時間の解除
        {
            Animator.SetBool("EnemyWalk", false);
            if (EnemyTime >= Rigor_Cancellation)
            {
                ReceivedDamage = false;
                DamageFlag = false;
                EnemyTime = 0;
            }
        }

        if (PlayerRangeDifference <= AttackDecision
            && AttackOn == false && NonDirectAttack == false
            && ReceivedDamage == false)
        { AttackOn = true; }//攻撃中

        if (AttackOn == true) { Attack(); }

        if (ReceivedDamage == false && AttackEnemy == false)//ダメージを受けたら動かない,攻撃中も動かない
        {
            if (CrabFlag) { CrabWalk(); }
            else { Move(); }
            Following();
            DrectionChange();
        }

        StatusUp();//ステータスアップ
    }

    /// <summary>
    /// 移動の制御
    /// </summary>
    void Move()
    {
        //前に進む
        if (MoveSwitch && NonDirectAttack == false)
        {
            Animator.SetBool("EnemyWalk", true);
            transform.Translate(0, 0, ZMove * Time.deltaTime);
        }
        else
        {
            Animator.SetBool("EnemyWalk", false);
        }
    }


    /// <summary>
    /// 蟹座の敵の動き
    /// </summary>
    void CrabWalk()
    {
        if (BossTime >= CrabMoveChange)
        {
            MoveChange = Random.Range(1, 3);
            BossTime = 0;
        }

        if (MoveChange == 1) { transform.position += transform.right * ZMove * Time.deltaTime; }
        else { transform.position -= transform.right * ZMove * Time.deltaTime; }

        transform.Translate(0, 0, ZMove * Time.deltaTime / 10);
    }

    /// <summary>
    /// 敵の索敵範囲に入ったらプレイヤーに向く
    /// </summary>
    void Following()
    {
        TargetPos = NearObj.transform.position;
        //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
        TargetPos.y = this.transform.position.y;

        //敵の索敵範囲に入ったらプレイヤーに追従開始
        if (PlayerRangeDifference <= OnPlayerTracking)
        {
            MoveSwitch = true;
            PlayerTracking = true;
            transform.LookAt(TargetPos);//対象の位置方向を向く 
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
                    //どの方向に行くかを決める
                    for (; DrectionNumber == Ran1 && DrectionNumber == Ran2;)
                    {
                        RandomNumber = Random.Range(DerectionLow, DerectionHigh);
                        int random = Random.Range(1, 3);
                        if (random == 2) { RandomNumber = RandomNumber * -1; }
                        if (RandomNumber >= 1 && RandomNumber <= 18) { DrectionNumber = 1; }
                        if (RandomNumber >= 19 && RandomNumber <= 36) { DrectionNumber = 2; }
                        if (RandomNumber >= -18 && RandomNumber <= -1) { DrectionNumber = 1; }
                        if (RandomNumber >= -36 && RandomNumber <= -19) { DrectionNumber = 3; }
                    }
                    if (RandomNumber >= 0) { if (YPlus <= 0) { YPlus = RotationPlus; } }

                    if (RandomNumber <= 0)
                    {
                        RandomNumber = RandomNumber * -1;
                        YPlus = YPlus * -1;
                    }
                    First = true;
                }
                //決められた方向にむくまで移動しない
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
        if (SpeedAttackFlag == false)
        {
            return;
        }

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
            ZMove = ZMove * 2;
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

    /// <summary>
    /// 攻撃判定をだす
    /// </summary>
    void Attack()
    {
        AttackTime += Time.deltaTime;
        AttackEnemy = true;
        Animator.SetBool("EnemyWalk", false);
        if (AttackMotionFirst == false)//攻撃モーションを一度だけ実行
        {
            Animator.SetTrigger("EnemyAttack");
            AttackMotionFirst = true;
        }

        if (AttackFirst == false && AttackTime >= OutPutAttackDecision && DamageFlag == false)
        {//敵の前にオブジェクト生成
            Vector3 position = transform.position + transform.up * Offset.y +
            transform.right * Offset.x +
            transform.forward * Offset.z;
            AttackObject = (GameObject)Instantiate(AttackPrefab, position, transform.rotation);
            AttackObject.transform.parent = this.transform;
            Destroy(AttackObject, 0.1f);
            AttackFirst = true;
        }

        if (AttackTime >= AttackWait || DamageFlag == true)
        {
            AttackEnemy = false;
            AttackTime = 0;
            EnemyTime = 0;
            AttackOn = false;
            AttackFirst = false;
            AttackMotionFirst = false;
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
            EnemyStatus.CurrentHp -= 10;//HPを減らす
            AttackCount++;

            if (EnemyStatus.CurrentHp <= 0)
            {
                EnemyStatus.CurrentHp = 0;
            }


            if (BossEnemy == false && ReceivedDamage == false)
            {
                ReceivedDamage = true;/*敵を硬直させる*/
                Animator.SetTrigger("EnemyDamage");
                DamageFlag = true;
                EnemyTime = 0;
            }

            if (BossEnemy && AttackCount >= DamageCount && ReceivedDamage == false)
            {
                Animator.SetTrigger("EnemyDamage");
                ReceivedDamage = true;/*敵を硬直させる*/
                AttackCount = 0;
                DamageFlag = true;
                EnemyTime = 0;
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
            item.transform.position = transform.position;
            item.transform.Rotate(0, Random.Range(-180, 180), 0);
        }

        for (int i = 0; i < StarCount; i++)//大きい星を生成できる分だけ星を生成
        {
            Star = BlueStar;//青の星を生成させる
            GameObject item = Instantiate(Star) as GameObject;//星の生成
            item.transform.position = transform.position;
            item.transform.Rotate(0, Random.Range(-180, 180), 0);
        }

        for (int i = 0; i < StarCount; i++)//大きい星を生成できる分だけ星を生成
        {
            Star = YellowStar;//黄の星を生成させる
            GameObject item = Instantiate(Star) as GameObject;//星の生成
            item.transform.position = transform.position;
            item.transform.Rotate(0, Random.Range(-180, 180), 0);
        }
    }

    /// <summary>
    /// 星の数に応じてステータスアップ
    /// </summary>
    void StatusUp()
    {
        if (WaveController.EnemyCount == 0) { return; }

        if (NonDirectAttack)
        {
            while( StatusUpCount <= WaveController.WaveCount)
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
        }
        else
        {
            while(StatusUpCount<= WaveController.WaveCount)
            {
                BuffDefence();
                BuffMove();
                if (StatusUpCount % 2 == 0)
                {
                    BuffAttack();
                    BuffHp();
                }
                StatusUpCount++;
            }
        }
    }

    void StatusDown()
    {
        
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
        EnemyAttackPoint += AttackPlus;
    }

    /// <summary>
    /// 敵の防御力を上昇
    /// </summary>
    void BuffDefence()
    {
        EnemyStatus.Defense += DefencePlus;
        EnemyDefence += DefencePlus;
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







