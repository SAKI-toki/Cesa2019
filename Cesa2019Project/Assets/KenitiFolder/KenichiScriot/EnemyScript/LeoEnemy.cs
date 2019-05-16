using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeoEnemy : MonoBehaviour
{
    float MoveSave = 0;
    float ReoTime = 0;
    float Yforword = 30;
    bool AssaultFlag = false;
    float AttackTime = 0;//攻撃の時間
    bool AttackOn = false;//攻撃中か
    bool AttackFirst = false;//攻撃を一度だけ実行
    bool AttackMotionFirst = false;//攻撃モーションを一度だけ実行
    GameObject AttackObject = null;

    Rigidbody Rigidbody = null;


    [SerializeField]
    GameObject EffectRush = null;
    //[SerializeField]
    //float KnockBackDecision = 50;
    [SerializeField]
    Enemy Enemy = null;
    [SerializeField]
    LeoGroundWave LeoGroundWave = null;
    [SerializeField]
    public NavMeshAgent GetMeshAgent = null;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = this.GetComponent<Rigidbody>();
        MoveSave = Enemy.ZMove;
        Enemy.MoveSwitch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.ReceivedDamage == false
            && Enemy.AttackEnemy == false
            && LeoGroundWave.Ground == false
            )//ダメージを受けたら動かない,攻撃中も動かない,ジャンプ中も動かない
        {
            ReoMove();
        }

        if (Enemy.PlayerRangeDifference <= Enemy.AttackDecision
          && AttackOn == false
          && Enemy.ReceivedDamage == false)
        { AttackOn = true; }//攻撃中

        if (AttackOn == true) { Attack(); }

        if (Enemy.JampFlag)
        {
            FalterMove();
            Enemy.JampFlag = false;
        }
    }

    /// <summary>
    /// 獅子座の動き
    /// </summary>
    void ReoMove()
    {
        if (Enemy.PlayerRangeDifference <= Enemy.OnPlayerTracking && AssaultFlag == false)
        {
            ReoTime += Time.deltaTime;
        }

        if (ReoTime <= 3) { Enemy.MoveSwitch = false; }
        else if (!AssaultFlag)
        {

            Enemy.TargetPos = Enemy.NearObj.transform.position;
            //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
            Enemy.TargetPos.y = this.transform.position.y;
            Enemy.ZMove = MoveSave * 6;
            transform.LookAt(Enemy.TargetPos);//対象の位置方向を向く
            Enemy.MoveSwitch = true;
            Enemy.BossTime = 0;
            AssaultFlag = true;
        }

        if (AssaultFlag)
        {
            Move();
            if (Enemy.BossTime >= 4)
            {
                Enemy.MoveSwitch = false;
                ReoTime = 0;
               
                Enemy.BossTime = 0;
            }
            else
            {
               AssaultFlag = false;
            }
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
            transform.Translate(0, 0, Enemy.ZMove * Time.deltaTime);
            EffectRush.SetActive(true);
        }
        else { EffectRush.SetActive(false); }
    }

    /// <summary>
    /// 攻撃判定をだす
    /// </summary>
    void Attack()
    {
        AttackTime += Time.deltaTime;
        Enemy.AttackEnemy = true;
        //Enemy.Animator.SetBool("EnemyWalk", false);
        if (AttackMotionFirst == false)//攻撃モーションを一度だけ実行
        {
            EffectRush.SetActive(false);
            Debug.Log("ji");
            Enemy.Animator.SetTrigger("EnemyAttack");
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

    void FalterMove()
    {
        GetMeshAgent.enabled = false;
        Vector3 force = new Vector3(0, Yforword, 0);
        Rigidbody.AddForce(force, ForceMode.Impulse);
    }
}
