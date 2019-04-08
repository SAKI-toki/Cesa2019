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
    PlayerController GetPlayerController = null;
    Rigidbody Rigidbody = null;
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
        GetPlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
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
        else
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
        }
    }

    void FalterMove()
    {
        GetMeshAgent.enabled = false;
        Vector3 force = new Vector3(0, Yforword, 0);
        Rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void KnockBackOn()
    {
        GetPlayerController.KnockBack();
    }
}
