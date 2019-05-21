using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    Animator Animator = null;
    [SerializeField]
    Enemy Enemy=null;
    [SerializeField]
    GameObject Bullet = null;
    [SerializeField, Header("発射間隔")]
    float BulletGenelateTime = 0;
    [SerializeField, Header("弾の高さ")]
    Vector3 HighPlus = new Vector3();
    [SerializeField, Header("弾をだす個数")]
    int WayBullet = 0;
    [SerializeField, Header("弾の間隔")]
    float BulletInterval = 0;

    float BulletTime = 0;
    float PlayerRangeDifference = 0;//プレイヤーと敵の距離差
    float Drection = 0;//プレイヤーの向き
    float BulletDrection = 0;//弾の向き

    GameObject NearObj;//プレイヤーの位置取得
    // Start is called before the first frame update
    void Start()
    {
        NearObj = SearchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得
        Animator = this.GetComponent<Animator>();
    }

    /// <summary>
    /// 弾の発射管理
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        if (Enemy.ReceivedDamage) { BulletTime = 0; }

        if (Time.timeScale >= 0 && Enemy.ReceivedDamage==false)
        {
            PlayerRangeDifference = Vector3.Distance(NearObj.transform.position, this.transform.position);

            if (PlayerRangeDifference <= Enemy.OnPlayerTracking) { BulletTime += Time.deltaTime; }

            if (BulletTime >= BulletGenelateTime)
            {
                Animator.SetTrigger("EnemyAttack");
                Drection = this.GetComponent<Transform>().localEulerAngles.y;
                BulletDrection = this.GetComponent<Transform>().localEulerAngles.y;
                if (BulletTime >= BulletGenelateTime+0.5f) { Way3();}
            }
        }
    }

    /// <summary>
    /// NWay弾を出す
    /// </summary>
    void Way3()
    {
        Shot();

        for (int i = 1; WayBullet >= i; i++)
        {
            BulletDrection = Drection - BulletInterval * i;
            Shot();
            BulletDrection = Drection + BulletInterval * i;
            Shot();
        }
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    void Shot()
    {
        GameObject bullet = Instantiate(Bullet) as GameObject;//弾を生成
        bullet.transform.position = transform.position + HighPlus;//指定した位置に移動
        bullet.transform.Rotate(0, BulletDrection, 0);//弾の向きを発射方向に
        BulletTime = 0;
    }


    /// <summary>
    ///指定したtagのオブジェクトを拾得
    /// </summary>
    /// <param name="nowObj"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    GameObject SearchTag(GameObject nowObj, string tagName)//指定されたtagの中で最も近いものを取得
    {
        float tmpDis = 0;//距離用一時変数
        float nearDis = 0;//最も近いオブジェクトの距離
        //string nearObjName="";//オブジェクト名称
        GameObject targetObj = null;//オブジェクト
        //tag指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);//自身と取得したオブジェクトの距離を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                //nearObjName=obs.name;
                targetObj = obs;
            }
        }
        //最も近かったオブジェクトを返す
        //return GameObject.Find(nearObjName);
        return targetObj;
    }
}
