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


    void FlockGenerate()
    {
        Vector3 position = transform.position + transform.up * GetEnemy.Offset.y +
                transform.right * GetEnemy.Offset.x +
                transform.forward * GetEnemy.Offset.z;
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
            transform.Translate(0, 0, GetEnemy.ZMove * Time.deltaTime);
        }
    }
}
