using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour
{
    GameObject NearObj;//プレイヤーの位置取得
    new Rigidbody rigidbody = null;
    //
    [SerializeField, Header("アイテムの追尾範囲")]
    float ItemOn = 10;
    [SerializeField, Header("アイテム出現時のZ方向の移動")]
    float Zforword = 0;
    [SerializeField, Header("アイテム出現時のY方向の移動")]
    float Yforword = 0;
    [SerializeField, Header("出現したアイテムが移動する時間")]
    float LimitTime = 2;

    float ItemTime;
    float ZMove;//
    float PlayerRange;
    bool First = true;

    Collider Collider = null;
    // Start is called before the first frame update
    void Start()
    {
        ZMove = 7;
        NearObj = SearchTag(gameObject, "Player");//プレイヤーのオブジェクトを取得 
        Collider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        ItemTime += Time.deltaTime;
        PlayerRange = Vector3.Distance(NearObj.transform.position, this.transform.position);

        Vector3 targetPos = NearObj.transform.position;
        //プレイヤーのYの位置とアイテムのYの位置を同じにしてX軸が回転しないようにします。
        //targetPos.y = this.transform.position.y;

        if (First)//前に飛び出させる
        {
            First = false;
            rigidbody = this.GetComponent<Rigidbody>();
            Vector3 force = this.transform.forward * Zforword;
            Vector3 force2 = new Vector3(0, Yforword, 0);
            rigidbody.AddForce(force, ForceMode.Impulse);
            rigidbody.AddForce(force2, ForceMode.Impulse);
        }

        if (ItemTime >= LimitTime)
        {
            rigidbody.velocity = Vector3.zero;
            Collider.isTrigger = true;
            if (PlayerRange <= ItemOn)
            {
                transform.LookAt(targetPos);//対象の位置方向を向く 
                transform.Translate(0, 0, ZMove * Time.deltaTime);
            }
        }


    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 指定したtagのオブジェクトを拾得
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
