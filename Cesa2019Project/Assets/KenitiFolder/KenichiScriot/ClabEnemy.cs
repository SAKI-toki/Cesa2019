using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClabEnemy : MonoBehaviour
{
    [SerializeField, Header("蟹が方向を変える時間")]
    float CrabMoveChange = 5;

    bool CrabFirst = true;//移動速度を一度だけ上げる
    int MoveDouble = 3;
    float MoveChange = 1;
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

        if (MoveChange == 1) { transform.position += transform.right * Enemy.ZMove * Time.deltaTime; }
        else { transform.position -= transform.right * Enemy.ZMove * Time.deltaTime; }

        transform.Translate(0, 0, Enemy.ZMove * Time.deltaTime / 10);
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
}
