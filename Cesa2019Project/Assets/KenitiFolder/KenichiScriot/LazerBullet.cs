using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBullet : MonoBehaviour
{
    [SerializeField, Header("発射間隔")]
    float ShotInterval = 0;
    [SerializeField, Header("レーザーが出るまでの時間")]
    float LazerInterval = 2;
    [SerializeField]
    GameObject LazerBulletObj = null;
    [SerializeField]
    GameObject PointObj = null;
    [SerializeField]
    Enemy Enemy = null;

    GameObject Point = null;
    Vector3 PlayerPoint = new Vector3(0, 0, 0);
    float BulletTime = 0;
    bool First = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BulletTime += Time.deltaTime;

        if (Enemy.DestroyFlag) { Destroy(this.gameObject); }

        if (BulletTime >= ShotInterval)
        {
            if (!First)
            {
                PlayerPoint.x = Enemy.TargetPos.x;
                PlayerPoint.z = Enemy.TargetPos.z;
                PlayerPoint.y = this.transform.position.y;
                Point = Instantiate(PointObj) as GameObject;//弾を生成
                Point.transform.position = Enemy.TargetPos;//指定した位置に移動
                Point.transform.Rotate(0, 0, 0);
                
                First = true;
            }

            if (BulletTime >= ShotInterval + LazerInterval)
            {
                GameObject bullet = Instantiate(LazerBulletObj) as GameObject;//弾を生成
                bullet.transform.position = PlayerPoint;//指定した位置に移動
                bullet.transform.Rotate(90, 0, 0);
                First = false;
                Destroy(Point);
                BulletTime = 0;
            }

        }
    }
}
