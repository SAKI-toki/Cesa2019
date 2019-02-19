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

    float PlayerRangeDifference;//プレイヤーと敵の距離差
    float OnPlayerTracking;//プレイヤーとの差が数値以下になったら追従開始
    float EnemyTime;//敵の時間
    float RandomOn;//移動方向変更の時間
    float Rigor_Cancellation;//被弾時の硬直時間
    float SA;//スピードアタックの時間

    bool ReceivedDamage;//ダメージをうけたときtrue
    bool PlayerTracking;//プレイヤーに追従してるときにtrue
    bool MoveSwitchR;//右に移動する
    bool MoveSwitchL;//左に移動する
    bool MoveSwitchU;//上に移動する
    bool MoveSwitchD;//下に移動する
    bool wait;//待機状態が解けたか

    // Start is called before the first frame update

        /// <summary>
        /// 数値初期化
        /// </summary>
    void Start()
    {
        MoveSwitchD = false;
        MoveSwitchU = false;
        MoveSwitchL = false;
        MoveSwitchR = false;

        wait = false;

        DerectionLow = 1;
        DerectionHigh = 8;
        RandomOn = 2;
        Latency = 1;
        NearObj = searchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得  
        OnPlayerTracking = 1;
        Rigor_Cancellation = 1;
    }

    // Update is called once per frame

        /// <summary>
        /// 各MoveSwitchがtrueになった方向に進む
        /// プレイヤーが索敵範囲の中に入ったらプレイヤーの方向に
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

            if (PlayerTracking == false)
            {
                //移動方向
                if (MoveSwitchR)//右に進む
                    transform.position += new Vector3(XMove * Time.deltaTime, 0, 0);
                if (MoveSwitchL)//左にすすむ
                    transform.position += new Vector3(-XMove * Time.deltaTime, 0, 0);
                if (MoveSwitchU)//上にすすむ
                    transform.position += new Vector3(0, 0, ZMove * Time.deltaTime);
                if (MoveSwitchD)//下に進む
                    transform.position += new Vector3(0, 0, -ZMove * Time.deltaTime);
            }


            Vector3 targetPos = NearObj.transform.position;
            targetPos.y = this.transform.position.y;


            //敵の索敵範囲に入ったらプレイヤーに追従開始
            if (PlayerRangeDifference <= OnPlayerTracking)
            {
                MoveSwitchD = false;
                MoveSwitchU = false;
                MoveSwitchL = false;
                MoveSwitchR = false;
                PlayerTracking = true;
                transform.LookAt(targetPos);//対象の位置方向を向く 
                transform.Translate(0, 0, ZMove * Time.deltaTime);

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
            else
            {
                PlayerTracking = false;
            }

            //一定間隔で移動方向を変更
            if (EnemyTime >= RandomOn && PlayerTracking == false)
            {
                MoveSwitchD = false;
                MoveSwitchU = false;
                MoveSwitchL = false;
                MoveSwitchR = false;

                if (EnemyTime >= RandomOn + Latency)
                {
                    RandomNumber = Random.Range(DerectionLow, DerectionHigh);


                    CMove();
                    EnemyTime = 0;
                }
            }
        }
    }

    /// <summary>
    /// RandomNumberで決められた数値の方向に行くようにMoveSwichを変更し、
    /// 向きを変更する
    /// </summary>
    void CMove()
    {

            if(RandomNumber==1)
            {
               MoveSwitchR = true;
                this.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
               
            if (RandomNumber == 2)
            {
               MoveSwitchL = true;
                this.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
                
            if (RandomNumber == 3)
            {
                MoveSwitchU = true;
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
                
            if (RandomNumber == 4)
            {
                MoveSwitchD = true;
                this.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
               
            if (RandomNumber == 5)
            {
                this.transform.localRotation = Quaternion.Euler(0, 45, 0);
                MoveSwitchR = true;
                MoveSwitchU = true;
            }
               
            if (RandomNumber == 6)
            {
                this.transform.localRotation = Quaternion.Euler(0, -135, 0);
                MoveSwitchL = true;
                MoveSwitchD = true;
            }
               
            if (RandomNumber == 7)
            {
                this.transform.localRotation = Quaternion.Euler(0, 45, 0);
                MoveSwitchU = true;
              MoveSwitchL = true;
            }
               
            if (RandomNumber == 8)
            {
                this.transform.localRotation = Quaternion.Euler(0, 135, 0);
                MoveSwitchD = true;
                MoveSwitchR = true;
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

         
    
    
        
    

