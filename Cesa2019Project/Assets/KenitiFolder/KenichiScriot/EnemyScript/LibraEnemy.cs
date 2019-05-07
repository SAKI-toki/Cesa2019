using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraEnemy : MonoBehaviour
{
    [SerializeField]
    Enemy GetEnemy = null;
    [SerializeField, Header("移動時間")]
    float BackTime = 10;
    [SerializeField]
    Vector3 Offset = new Vector3(5, 5, 0);
    [SerializeField]
    Vector3 Offset2 = new Vector3(-5, 5, 0);
    [SerializeField]
    GameObject Bullet = null;
    [SerializeField]
    GameObject Bulet2 = null;

    float VirgoTime = 0;
    float BulletTime = 0;
    float BulletTime2 = 0;
    int BulletCount = 0;
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

            LiblaMove();
            BulletTime += Time.deltaTime;
            if(BulletTime>=2)
            {
                BulletTime2 += Time.deltaTime;
                if(BulletTime2>=1.5)
                {
                    BulletGenerate();
                    BulletCount++;
                    if (BulletCount >= 3) { BulletTime = 0;BulletTime2 = 0; };
                    BulletTime2 = 0;
                }
            }
        }
    }

    void LiblaMove()
    {
        if (!GetEnemy.JampFlag) { VirgoTime = 0; return; }

        VirgoTime += Time.deltaTime;

        if (VirgoTime >= BackTime)
        {
            transform.Translate(1, 0, -GetEnemy.ZMove * Time.deltaTime);
            GetEnemy.JampFlag = false;
        }
    }

    void BulletGenerate()
    {
        Vector3 position = transform.position + transform.up * Offset.y +
           transform.right * Offset.x +
           transform.forward * Offset.z;
        GameObject bullet = (GameObject)Instantiate(Bullet, position, transform.rotation);

        Vector3 position2 = transform.position + transform.up * Offset2.y +
           transform.right * Offset2.x +
           transform.forward * Offset2.z;
        GameObject bullet2 = (GameObject)Instantiate(Bullet, position2, transform.rotation);
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
