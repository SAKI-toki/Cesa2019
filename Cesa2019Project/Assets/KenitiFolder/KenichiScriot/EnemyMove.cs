﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    bool First = false;//一度だけ実行させる

    int RandomNumber = 0;//ランダムな数値を入れる
    int DerectionLow = 1;//ランダムの低値
    int DerectionHigh = 36;//ランダムの高値
    int Ran1 = 0;//RandomNumberの値を保存
    int Ran2 = 0;//Ran1の値を保存
    int DrectionNumber = 0;//方向に応じて数値を保存
    int RotationCount = 0;//回転の回数

    float YPlus = 0;//rotationを動かす角度
    float RotationTime = 0;//敵のrotation変更の時に使う時間
    float RandomOn = 0;//移動方向変更の時
    float MoveTimeLow = 1.0f;//移動している時間の低値
    float MoveTimeHigh = 3.0f;//移動している時間高値
    [SerializeField]
    Enemy Enemy=null;
    // Start is called before the first frame update
    void Start()
    {

        YPlus = Enemy.RotationPlus;
        RandomOn = Random.Range(MoveTimeLow, MoveTimeHigh);
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.ReceivedDamage == false && Enemy.AttackEnemy == false)//ダメージを受けたら動かない,攻撃中も動かない
        {
            Move();
            Following();
            DrectionChange();
        }
    }

    /// <summary>
    /// 移動の制御
    /// </summary>
    void Move()
    {
        //前に進む
        if (Enemy.MoveSwitch)
        {
            Enemy.Animator.SetBool("EnemyWalk", true);
            transform.Translate(0, 0, Enemy.ZMove * Time.deltaTime);
        }
        else
        {
            Enemy.Animator.SetBool("EnemyWalk", false);
        }
    }

    /// <summary>
    /// 敵の索敵範囲に入ったらプレイヤーに向く
    /// </summary>
    void Following()
    {
        Enemy.TargetPos = Enemy.NearObj.transform.position;
        //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
        Enemy.TargetPos.y = this.transform.position.y;

        //敵の索敵範囲に入ったらプレイヤーに追従開始
        if (Enemy.PlayerRangeDifference <= Enemy.OnPlayerTracking)
        {
            Enemy.MoveSwitch = true;
            Enemy.PlayerTracking = true;
            transform.LookAt(Enemy.TargetPos);//対象の位置方向を向く 

        }
        else
        {
            Enemy.PlayerTracking = false;
        }
    }

    /// <summary>
    /// 一定間隔で方向を変えてます
    /// </summary>
    void DrectionChange()
    {
        RandomOn = Random.Range(MoveTimeLow, MoveTimeHigh);
        //一定間隔で移動方向を変更
        if (Enemy.EnemyTime >= RandomOn && Enemy.PlayerTracking == false)
        {
            Enemy.MoveSwitch = false;
            Ran2 = Ran1;
            Ran1 = DrectionNumber;
            if (Enemy.EnemyTime >= RandomOn + Enemy.Latency)
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
                    if (RandomNumber >= 0) { if (YPlus <= 0) { YPlus = Enemy.RotationPlus; } }

                    if (RandomNumber <= 0)
                    {
                        RandomNumber = RandomNumber * -1;
                        YPlus = YPlus * -1;
                    }
                    First = true;
                }
                //決められた方向にむくまで移動しない
                if (RandomNumber != RotationCount & RotationTime >= Enemy.RotateHours)
                {
                    transform.Rotate(0, YPlus, 0);
                    RotationCount++;
                    if (RandomNumber == RotationCount)
                    {
                        First = false;
                        Enemy.EnemyTime = 0;
                        Enemy.MoveSwitch = true;
                        RotationCount = 0;
                    }
                }
            }
        }
    }
}
