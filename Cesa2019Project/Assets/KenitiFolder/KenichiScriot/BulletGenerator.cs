using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField, Header("発射間隔")]
    float BulletGenelateTime=0;
    [SerializeField, Header("攻撃範囲（敵の索敵範囲と同じに）")]
    float AtackDcetion=10;
    [SerializeField]
    GameObject Bullet=null;
    [SerializeField,Header("弾の高さ")]
    Vector3 HighPlus=new Vector3();
    [SerializeField,Header("弾をだす個数")]
    int WayBullet=0;
    [SerializeField, Header("弾の間隔")]
    float BulletInterval=0;



    float BulletTime=0;
    float PlayerRangeDifference=0;//プレイヤーと敵の距離差
    float Drection=0;//プレイヤーの向き
    float BulletDrection=0;//弾の向き

    GameObject NearObj;//プレイヤーの位置取得
    // Start is called before the first frame update
    void Start()
    {
        NearObj = searchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale!=0)
        {
            
            PlayerRangeDifference = Vector3.Distance(NearObj.transform.position, this.transform.position);

            if (PlayerRangeDifference <= AtackDcetion) { BulletTime += Time.deltaTime;}

            if (BulletTime>=BulletGenelateTime)
            {
                 Drection = this.GetComponent<Transform>().localEulerAngles.y;
                BulletDrection= this.GetComponent<Transform>().localEulerAngles.y;
                
                Way3();    
            }
        }
    }

    void Way3()
    {

        Shot();

        for (int i=0;WayBullet!=i;i++)
        {
            BulletDrection =Drection-BulletInterval * i;
            Shot();
            BulletDrection = Drection+BulletInterval * i;
            Shot();
        }
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    void Shot()
    {
               
                GameObject item = Instantiate(Bullet) as GameObject;
                item.transform.position = transform.position+HighPlus;
                item.transform.Rotate(0, BulletDrection, 0);
                BulletTime = 0;
    }


    /// <summary>
    /// プレイヤーの位置取得
    /// </summary>
    /// <param name="nowObj"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    GameObject searchTag(GameObject nowObj, string tagName)//指定されたtagの中で最も近いものを取得
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
