using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionEnemy : MonoBehaviour
{
    int PoisonCount = 0;
    float MoveSave = 0;
    float ScorpionTime = 0;
    float PoisonHp = 0;
    float SecondPoisonHp = 0;
    bool PoisonFirst = false;
    bool AssaultFlag = false;

    float AttackTime = 0;//攻撃の時間
    bool AttackOn = false;//攻撃中か
    bool AttackFirst = false;//攻撃を一度だけ実行
    bool AttackMotionFirst = false;//攻撃モーションを一度だけ実行
    GameObject AttackObject = null;


    Vector3 HighPlus = new Vector3(0, -0.5f, 4);

    [SerializeField]
    GameObject PoisonObject = null;
    [SerializeField]
    GameObject SecondPoison = null;
    [SerializeField]
    Enemy Enemy = null;
    // Start is called before the first frame update
    void Start()
    {
        PoisonHp = Enemy.EnemyHp * 0.6f;
        SecondPoisonHp = Enemy.EnemyHp * 0.3f;
        MoveSave = Enemy.ZMove;
        Enemy.MoveSwitch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.ReceivedDamage == false
            && Enemy.AttackEnemy == false
            )//ダメージを受けたら動かない,攻撃中も動かない,ジャンプ中も動かない
        {
            ScorpionMove();
        }

        if (Enemy.PlayerRangeDifference <= Enemy.AttackDecision
           && AttackOn == false
           && !Enemy.NonDirectAttack
           && Enemy.ReceivedDamage == false)
        { AttackOn = true; }//攻撃中

        if (AttackOn == true) { Attack(); }
    }

    /// <summary>
    /// 獅子座の動き
    /// </summary>
    void ScorpionMove()
    {
        if (Enemy.PlayerRangeDifference <= Enemy.OnPlayerTracking && AssaultFlag == false)
        {
            ScorpionTime += Time.deltaTime;
        }

        if (ScorpionTime <= 3) { Enemy.MoveSwitch = false; }
        else
        {
            Enemy.TargetPos = Enemy.NearObj.transform.position;
            //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
            Enemy.TargetPos.y = this.transform.position.y;
            Enemy.ZMove = MoveSave;
            transform.LookAt(Enemy.TargetPos);//対象の位置方向を向く
            Enemy.MoveSwitch = true;
            Enemy.BossTime = 0;
            AssaultFlag = true;
        }

        if (AssaultFlag)
        {
            Move();
            if (Enemy.BossTime >= 6)
            {
                Enemy.MoveSwitch = false;
                ScorpionTime = 0;
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

    /// <summary>
    /// 攻撃判定をだす
    /// </summary>
    void Attack()
    {
        AttackTime += Time.deltaTime;
        Enemy.AttackEnemy = true;
        if (AttackMotionFirst == false)//攻撃モーションを一度だけ実行
        {
            Enemy.EnemySe.AttackSES();
            if (Enemy.EnemyStatus.CurrentHp < SecondPoisonHp && PoisonCount <= 3)
            {
                SecondPoisonSwamp();
                PoisonFirst = true;
                PoisonCount++;
            }
            if (Enemy.EnemyStatus.CurrentHp < PoisonHp && PoisonFirst == false)
            {
                PoisonSwamp();
            }
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

    /// <summary>
    /// 毒沼の生成
    /// </summary>
    void PoisonSwamp()
    {
        Vector3 position = transform.position + transform.up * HighPlus.y +
           transform.right * HighPlus.x +
           transform.forward * HighPlus.z;
        GameObject poison = (GameObject)Instantiate(PoisonObject, position, transform.rotation);
    }

    void SecondPoisonSwamp()
    {
        Vector3 position = transform.position + transform.up * HighPlus.y +
            transform.right * HighPlus.z;
        GameObject secondpoison = (GameObject)Instantiate(SecondPoison, position, transform.rotation);
    }
}
