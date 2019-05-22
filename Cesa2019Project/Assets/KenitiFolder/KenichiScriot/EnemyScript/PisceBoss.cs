using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PisceBoss : MonoBehaviour
{
    [SerializeField]
    Enemy GetEnemy = null;
    [SerializeField]
    GameObject Flock = null;

    GameObject FlockObj = null;
    bool AssaultFlag = false;
    float FishTime = 0;
    bool First = false;

    float AttackTime = 0;//攻撃の時間
    bool AttackOn = false;//攻撃中か
    bool AttackFirst = false;//攻撃を一度だけ実行
    bool AttackMotionFirst = false;//攻撃モーションを一度だけ実行
    GameObject AttackObject = null;

    Vector3 Gene = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        FlockGenerate();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetEnemy.ReceivedDamage == false && GetEnemy.AttackEnemy == false)//ダメージを受けたら動かない,攻撃中も動かない
        {
            PisceMove();
            Move();
        }

        if (GetEnemy.PlayerRangeDifference <= GetEnemy.AttackDecision
           && AttackOn == false
           && GetEnemy.ReceivedDamage == false)
        { AttackOn = true; }//攻撃中

        if (AttackOn == true) { Attack(); }

        if (FlockObj != null)
        {
            GetEnemy.NoDamage = true;
        }
        else
        {
            if (GetEnemy.EnemyStatus.CurrentHp <= GetEnemy.EnemyHp / 2 && !First)
            {
                FlockGenerate();
                First = true;
            }
            GetEnemy.NoDamage = false;
        }
    }

    void PisceMove()
    {
        if (GetEnemy.PlayerRangeDifference <= GetEnemy.OnPlayerTracking && AssaultFlag == false)
        {
            FishTime += Time.deltaTime;
        }

        if (FishTime <= 3) { GetEnemy.MoveSwitch = false; }
        else
        {
            GetEnemy.TargetPos = GetEnemy.NearObj.transform.position;
            //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
            GetEnemy.TargetPos.y = this.transform.position.y;
            transform.LookAt(GetEnemy.TargetPos);//対象の位置方向を向く
            GetEnemy.MoveSwitch = true;
            GetEnemy.BossTime = 0;
            AssaultFlag = true;
        }

        if (AssaultFlag)
        {
            Move();
            if (GetEnemy.BossTime >= 4)
            {
                GetEnemy.MoveSwitch = false;
                FishTime = 0;
                GetEnemy.BossTime = 0;
            }
            else
            {
                AssaultFlag = false;
            }
        }
    }


    /// <summary>
    /// 攻撃判定をだす
    /// </summary>
    void Attack()
    {
        AttackTime += Time.deltaTime;
        GetEnemy.AttackEnemy = true;
        //Enemy.Animator.SetBool("EnemyWalk", false);
        if (AttackMotionFirst == false)//攻撃モーションを一度だけ実行
        {
            GetEnemy.Animator.SetTrigger("EnemyAttack1");
            GetEnemy.EnemySe.AttackSES();
            AttackMotionFirst = true;
        }

        if (AttackFirst == false && AttackTime >= GetEnemy.OutPutAttackDecision && GetEnemy.DamageFlag == false)
        {//敵の前にオブジェクト生成
            Vector3 position = transform.position + transform.up * GetEnemy.Offset.y +
            transform.right * GetEnemy.Offset.x +
            transform.forward * GetEnemy.Offset.z;
            AttackObject = (GameObject)Instantiate(GetEnemy.AttackPrefab, position, transform.rotation);
            AttackObject.transform.parent = this.transform;
            Destroy(AttackObject, 0.1f);
            AttackFirst = true;
        }

        if (AttackTime >= GetEnemy.AttackWait || GetEnemy.DamageFlag == true)
        {
            GetEnemy.AttackEnemy = false;
            AttackTime = 0;
            GetEnemy.EnemyTime = 0;
            AttackOn = false;
            AttackFirst = false;
            AttackMotionFirst = false;
        }
    }

    void FlockGenerate()
    {
        Vector3 position = transform.position + transform.up * Gene.y +
                transform.right * Gene.x +
                transform.forward * Gene.z;
        FlockObj = (GameObject)Instantiate(Flock, position, transform.rotation);
    }

    /// <summary>
    /// 前に移動
    /// </summary>
    void Move()
    {
        //前に進む
        if (GetEnemy.MoveSwitch)
        {
            GetEnemy.Animator.SetBool("EnemyWalk", true);
            transform.Translate(0, 0, GetEnemy.ZMove * Time.deltaTime);
        }
        else
        {
            GetEnemy.Animator.SetBool("EnemyWalk", true);
        }
    }
}
