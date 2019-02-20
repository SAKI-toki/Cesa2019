//小林健一
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の移動とHP
/// 
/// </summary>

public class Enemy : MonoBehaviour
{
    GameObject NearObj;//プレイヤーの位置取得
    [SerializeField]
    GameObject Star;
    [SerializeField]
    float XMove,ZMove;//X.Y方向の移動力
    [SerializeField]
    int EnemyHp;//HPを設定する
    [SerializeField]
    bool SpeedAtack;
   
 　 int RandomNumber;//ランダムな数値を入れる
    int DerectionLow;//ランダムの低値
    int DerectionHigh;//ランダムの高値
    int Latency;//待機時間
    int Ran1;//RandomNumberの値を保存
    int Ran2;//Ran1の値を保存
    int DrectionNumber;//方向に応じて数値を保存
    [SerializeField]
    float PlayerRangeDifference;//プレイヤーと敵の距離差
    float OnPlayerTracking;//プレイヤーとの差が数値以下になったら追従開始
    float EnemyTime;//敵の時間
    float RandomOn;//移動方向変更の時間
    float Rigor_Cancellation;//被弾時の硬直時間
    float SA;//スピードアタックの時間
    float MoveTimeLow;
    float MoveTimeHigh;

    bool ReceivedDamage;//ダメージをうけたときtrue
    bool PlayerTracking;//プレイヤーに追従してるときにtrue
    bool MoveSwitch;//前に移動する
    bool wait;//待機状態が解けたか

    Vector3 targetPos;

    // Start is called before the first frame update

    /// <summary>
    /// 数値初期化
    /// </summary>
    void Start()
    {
        wait = false;
        MoveTimeLow = 1.0f;
        MoveTimeHigh = 3.0f;
        DerectionLow = -180;
        DerectionHigh = 180;
        RandomOn = Random.Range(MoveTimeLow,MoveTimeHigh);
        Latency = 1;
        NearObj = searchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得  
        OnPlayerTracking = 15;
        Rigor_Cancellation = 1;
        
    }

    // Update is called once per frame

        /// <summary>
        /// 撃破判定
        /// ダメージを受けた時に硬直時間を発生させる
        /// </summary>
    void Update()
    {
        EnemyTime += Time.deltaTime;

        PlayerRangeDifference = Vector3.Distance(NearObj.transform.position, this.transform.position);

        if(EnemyHp==0)
        {
            GameObject item = Instantiate(Star) as GameObject;
            item.transform.position = transform.position;
            Destroy(this.gameObject);
        }

        if(ReceivedDamage==true)//硬直時間の解除
        {
            if(EnemyTime>=Rigor_Cancellation)
            {
                ReceivedDamage = false;
                EnemyTime = 0;
            }
        }

        if (ReceivedDamage == false)//ダメージを受けたら動かない
        {
            Move();
            DrectionChange();
        }
    }

    /// <summary>
    /// 移動の制御
    /// </summary>
    void Move()
    {
        if (PlayerTracking == false)
        {
            //移動方向
            if (MoveSwitch)//右に進む
                transform.Translate(0, 0, ZMove * Time.deltaTime);
        }


        targetPos = NearObj.transform.position;
        //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
        targetPos.y = this.transform.position.y;


        //敵の索敵範囲に入ったらプレイヤーに追従開始
        if (PlayerRangeDifference <= OnPlayerTracking)
        {
            MoveSwitch = false;
            PlayerTracking = true;
            transform.LookAt(targetPos);//対象の位置方向を向く 
            transform.Translate(0, 0, ZMove * Time.deltaTime);

            Speedatack();
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
        RandomOn = Random.Range(MoveTimeLow,MoveTimeHigh);
        //一定間隔で移動方向を変更
        if (EnemyTime >= RandomOn && PlayerTracking == false)
        {
            MoveSwitch = false;
            Ran2 = Ran1;
            Ran1 = DrectionNumber;

            if (EnemyTime >= RandomOn + Latency)
            {

                for (; DrectionNumber == Ran1 || DrectionNumber == Ran2;)
                {
                    RandomNumber = Random.Range(DerectionLow, DerectionHigh);
                    if (RandomNumber >= 0 && RandomNumber <= 90) { DrectionNumber = 1; }
                    if (RandomNumber >= 91 && RandomNumber <= 180) { DrectionNumber = 2; }
                    if (RandomNumber >= -90 && RandomNumber <= 1) { DrectionNumber = 3; }
                    if (RandomNumber >= -180 && RandomNumber <= -91) { DrectionNumber = 4; }
                }

                this.transform.localRotation = Quaternion.Euler(0, RandomNumber, 0);
                MoveSwitch = true;
                EnemyTime = 0;
            }
        }
    }
        
    /// <summary>
    /// 一旦止まって高速で突撃する攻撃をするときに使う
    /// </summary>
    void Speedatack()
    {
            if (SpeedAtack)
            {
                float taiki = 0.5f;
                if (PlayerRangeDifference <= taiki && wait == false)
                {
                    ZMove = 0;
                    int speedTime = 1;
                    SA += Time.deltaTime;
                    if (SA >= speedTime)
                    {
                        wait = true;
                        SA = 0;
                    }
                }

                if (wait == true)
                {
                    ZMove = 0.7f;
                    SA += Time.deltaTime;
                    float speedTime2 = 5;
                    int speedAtackMove = 2;
                    transform.LookAt(targetPos);//対象の位置方向を向く 
                    transform.Translate(0, 0, ZMove * speedAtackMove * Time.deltaTime);
                    if (SA >= speedTime2)
                    {
                        wait = false;
                        SpeedAtack = false;
                    }
                }
            }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
         EnemyHp -= 10;//HPを減らす
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

         
    
    
        
    

