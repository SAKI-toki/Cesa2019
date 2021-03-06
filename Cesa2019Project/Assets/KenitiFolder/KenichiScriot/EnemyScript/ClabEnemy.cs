﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClabEnemy : MonoBehaviour
{
    [SerializeField, Header("蟹が方向を変える時間")]
    float CrabMoveChange = 5;

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
        if (Enemy.EnemyStatus.CurrentHp <= 0) { return; }

        ClabTime += Time.deltaTime;
        Following();

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

        if (First && !Enemy.DestroyFlag)
        {
            transform.Translate(0, 0, -Enemy.ZMove * Time.deltaTime / 5);
            if (ClabTime >= 5) { First = false; ClabTime = 0; }
        }

        ///攻撃用
        if (Enemy.PlayerRangeDifference <= Enemy.AttackDecision &&
            AttackOn == false &&
            !Enemy.NonDirectAttack &&
            Enemy.ReceivedDamage == false)
        { AttackOn = true; }//攻撃中

        if (AttackOn == true) { Attack(); }

        if (MoveChange == 1 && !Enemy.ReceivedDamage && !Enemy.DestroyFlag)
        { transform.position += transform.right * Enemy.ZMove * Time.deltaTime; }
        else if (MoveChange == 2 && !Enemy.ReceivedDamage && !Enemy.DestroyFlag)
        { transform.position -= transform.right * Enemy.ZMove * Time.deltaTime; }
        else { }

        if (Enemy.ReceivedDamage == false && Enemy.AttackEnemy == false && !Enemy.DestroyFlag)//ダメージを受けたら動かない,攻撃中も動かない
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

            AttackMotionFirst = true;
        }

        if (AttackFirst == false && AttackTime >= Enemy.OutPutAttackDecision && Enemy.DamageFlag == false)
        {//敵の前にオブジェクト生成
            Enemy.EnemySe.AttackSES();
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
