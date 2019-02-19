using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour
{
    GameObject NearObj;//プレイヤーの位置取得

    float ItemTime;
    float LimitTime;
    float YMove;
    float XMove;
    float ZMove;
    float PlayerRange;
    float ItemOn;

    // Start is called before the first frame update
    void Start()
    {
        LimitTime = 0.5f;
        YMove = 0.5f;
        XMove = 0.2f;
        ZMove = 1;
        ItemOn = 0.6f;
        NearObj = searchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得 
    }

    // Update is called once per frame
    void Update()
    {
        ItemTime += Time.deltaTime;

        PlayerRange = Vector3.Distance(NearObj.transform.position, this.transform.position);

        if (ItemTime>=LimitTime)
        {
            XMove = 0;
            YMove = 0;
        }

        transform.position += new Vector3(XMove*Time.deltaTime, YMove*Time.deltaTime, 0);

        Vector3 targetPos = NearObj.transform.position;
        targetPos.y = this.transform.position.y;

        if(PlayerRange<=ItemOn)
        {
            transform.LookAt(targetPos);//対象の位置方向を向く 
            transform.Translate(0, 0, ZMove * Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name=="Capsule")
        {
            Destroy(gameObject);
        }
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
