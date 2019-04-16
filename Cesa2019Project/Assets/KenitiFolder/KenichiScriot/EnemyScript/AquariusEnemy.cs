﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariusEnemy : MonoBehaviour
{
    [SerializeField]
    Enemy GetEnemy = null;
    [SerializeField, Header("移動時間")]
    float BackTime = 10;
    [SerializeField]
    GameObject Wave = null;

    float AquariusTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetEnemy.ReceivedDamage == false && GetEnemy.AttackEnemy == false)//ダメージを受けたら動かない,攻撃中も動かない
        {
            Following();

            if (GetEnemy.EnemyTime <= 60) { EnemyGenerate(); }
        }
    }

    void AquariusMove()
    {
        if (!GetEnemy.JampFlag) { AquariusTime = 0; return; }

        AquariusTime += Time.deltaTime;

        if (AquariusTime >= BackTime)
        {
            transform.Translate(1, 0, -GetEnemy.ZMove * Time.deltaTime);
            GetEnemy.JampFlag = false;
        }
    }

   void EnemyGenerate()
    {
        GameObject wave = Instantiate(Wave) as GameObject;//弾を生成
        wave.transform.position = this.transform.position;//指定した位置に移動
    }

    /// <summary>
    /// 敵の索敵範囲に入ったらプレイヤーに向く
    /// </summary>
    void Following()
    {
        GetEnemy.TargetPos = GetEnemy.NearObj.transform.position;
        //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
        GetEnemy.TargetPos.y = this.transform.position.y;

        //敵の索敵範囲に入ったらプレイヤーに追従開始
        if (GetEnemy.PlayerRangeDifference <= GetEnemy.OnPlayerTracking)
        {
            GetEnemy.MoveSwitch = true;
            GetEnemy.PlayerTracking = true;
            transform.LookAt(GetEnemy.TargetPos);//対象の位置方向を向く 

        }
        else
        {
            GetEnemy.PlayerTracking = false;
        }
    }
}
