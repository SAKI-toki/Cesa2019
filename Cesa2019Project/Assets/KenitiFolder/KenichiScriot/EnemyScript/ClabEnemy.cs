﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClabEnemy : MonoBehaviour
{
    [SerializeField, Header("蟹が方向を変える時間")]
    float CrabMoveChange = 5;

    bool CrabFirst = true;//移動速度を一度だけ上げる
    int MoveDouble = 3;
    float MoveChange = 1;
    float ClabTime = 0;
    float CoolTime = 3;//離脱用

    float AttackTime = 0;//攻撃の時間
    bool AttackOn = false;//攻撃中か
    bool AttackFirst = false;//攻撃を一度だけ実行
    bool AttackMotionFirst = false;//攻撃モーションを一度だけ実行
    GameObject AttackObject = null;

    bool First = false;

    [SerializeField]
    Enemy Enemy = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// 蟹座の敵の動き
    /// </summary>
    void Update()
    {
        ClabTime += Time.deltaTime;
        Following();

        //蟹座のボスの時HPが１/３になったら移動速度三倍
        if (Enemy.EnemyStatus.Hp / 3 >= Enemy.EnemyStatus.CurrentHp && CrabFirst)
        {
            Enemy.ZMove = MoveDouble * Enemy.ZMove;
            CrabFirst = false;
        }

        if (Enemy.BossTime >= CrabMoveChange)
        {
            MoveChange = Random.Range(1, 3);
            Enemy.BossTime = 0;
        }
        ///離脱用
        if (Enemy.PlayerRangeDifference <= Enemy.AttackDecision &&
            ClabTime >= CoolTime &&
            !First)
        {
            First = true; ClabTime = 0;
        }

        if (First)
        {
            transform.Translate(0, 0, -Enemy.ZMove * Time.deltaTime);
            if (ClabTime >= 5) { First = false; ClabTime = 0; }
        }

        ///攻撃用
        if (Enemy.PlayerRangeDifference <= Enemy.AttackDecision &&
            AttackOn == false &&
            !Enemy.NonDirectAttack &&
            Enemy.ReceivedDamage == false)
        { AttackOn = true; }//攻撃中

        if (AttackOn == true) { Attack(); }

        if (MoveChange == 1) { transform.position += transform.right * Enemy.ZMove * Time.deltaTime; }
        else { transform.position -= transform.right * Enemy.ZMove * Time.deltaTime; }

        if (Enemy.ReceivedDamage == false && Enemy.AttackEnemy == false)//ダメージを受けたら動かない,攻撃中も動かない
        {
            Enemy.Animator.SetBool("EnemyWalk", true);
            transform.Translate(0, 0, Enemy.ZMove * Time.deltaTime / 10);
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
    /// 攻撃判定をだす
    /// </summary>
    void Attack()
    {
        AttackTime += Time.deltaTime;
        Enemy.AttackEnemy = true;
        Enemy.Animator.SetBool("EnemyWalk", false);
        if (AttackMotionFirst == false)//攻撃モーションを一度だけ実行
        {
            Enemy.Animator.SetTrigger("EnemyAttack2");
            Enemy.EnemySe.AttackSES();
            AttackMotionFirst = true;
        }

        if (AttackFirst == false && AttackTime >= Enemy.OutPutAttackDecision && Enemy.DamageFlag == false)
        {//敵の前にオブジェクト生成
            Vector3 position = transform.position + transform.up * Enemy.Offset.y +
            transform.right * Enemy.Offset.x +
            transform.forward * Enemy.Offset.z;
            AttackObject = (GameObject)Instantiate(Enemy.AttackPrefab, position, transform.rotation);
            AttackObject.transform.parent = this.transform;
            Destroy(AttackObject, 0.1f);
            AttackFirst = true;
        }

        if (AttackTime >= Enemy.AttackWait || Enemy.DamageFlag == true)
        {
            Enemy.AttackEnemy = false;
            AttackTime = 0;
            Enemy.EnemyTime = 0;
            AttackOn = false;
            AttackFirst = false;
            AttackMotionFirst = false;
        }
    }
}
