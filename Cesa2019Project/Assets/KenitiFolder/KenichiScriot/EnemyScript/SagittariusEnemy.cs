using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagittariusEnemy : MonoBehaviour
{
    [SerializeField]
    Enemy GetEnemy = null;
    [SerializeField, Header("移動時間")]
    float BackTime = 10;
    [SerializeField, Header("発射間隔")]
    float ShotInterval = 3;
    [SerializeField, Header("レーザーが出るまでの時間")]
    float LazerInterval = 1;
    [SerializeField]
    GameObject LazerBulletObj = null;
    [SerializeField]
    GameObject PointObj = null;

    float SagittariusHp = 0;
    GameObject Point = null;
    Vector3 PlayerPoint = new Vector3(0, 20, 0);
    float BulletTime = 0;
    bool PointFlag = false;
    bool First = false;
    bool Second = false;
    bool Third = false;

    float SagittariusTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        SagittariusHp = GetEnemy.EnemyHp / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetEnemy.ReceivedDamage == false && GetEnemy.AttackEnemy == false)//ダメージを受けたら動かない,攻撃中も動かない
        {
            Following();
            Attack();
            if(GetEnemy.EnemyStatus.CurrentHp>=SagittariusHp)
            {
                Attack2();
                Attack2();
            }
            SagittariusMove();
        }
    }

    void SagittariusMove()
    {
        if (!GetEnemy.JampFlag) { SagittariusTime = 0; return; }

        SagittariusTime += Time.deltaTime;

        if (SagittariusTime >= BackTime)
        {
            transform.Translate(1, 0, -GetEnemy.ZMove * Time.deltaTime);
            GetEnemy.JampFlag = false;
            SagittariusTime = 0;
        }
    }

    void Attack()
    {
        BulletTime += Time.deltaTime;

        if (BulletTime >= ShotInterval)
        {
            if (!PointFlag)
            {
                PlayerPoint.x = GetEnemy.TargetPos.x;
                PlayerPoint.z = GetEnemy.TargetPos.z;
                Point = Instantiate(PointObj) as GameObject;//弾を生成
                Point.transform.position = GetEnemy.TargetPos;//指定した位置に移動
                Point.transform.Rotate(0, 0, 0);
                PointFlag = true;
            }
            if (BulletTime >= ShotInterval + LazerInterval && !First)
            {
                if (BulletTime >= ShotInterval + LazerInterval)
                {
                    GameObject bullet = Instantiate(LazerBulletObj) as GameObject;//弾を生成
                    bullet.transform.position = PlayerPoint;//指定した位置に移動
                    bullet.transform.Rotate(90, 0, 0);
                    PointFlag = false;
                    Destroy(Point);
                    BulletTime = 0;
                }
            }
        }
    }

    void Attack2()
    {
        BulletTime += Time.deltaTime;

        if (BulletTime >= ShotInterval)
        {
            if (!PointFlag)
            {
                float xPlus = Random.Range(-1, 1);
                float zPlus = Random.Range(-1, 1);
                PlayerPoint.x = GetEnemy.TargetPos.x;
                PlayerPoint.z = GetEnemy.TargetPos.z;
                Point = Instantiate(PointObj) as GameObject;//弾を生成
                Point.transform.position = GetEnemy.TargetPos;//指定した位置に移動
                Point.transform.Rotate(0, 0, 0);
                PointFlag = true;
            }
            if (BulletTime >= ShotInterval + LazerInterval && !First)
            {
                if (BulletTime >= ShotInterval + LazerInterval)
                {
                    GameObject bullet = Instantiate(LazerBulletObj) as GameObject;//弾を生成
                    bullet.transform.position = PlayerPoint;//指定した位置に移動
                    bullet.transform.Rotate(90, 0, 0);
                    PointFlag = false;
                    Destroy(Point);
                    BulletTime = 0;
                }
            }
        }
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
