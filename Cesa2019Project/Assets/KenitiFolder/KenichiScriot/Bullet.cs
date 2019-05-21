using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    int BulletCategory = 0;
    [SerializeField, Header("弾の移動力")]
    float BulletMove = 2;
    [SerializeField, Header("弾の破壊時間")]
    float DestroyTime = 0;
    [SerializeField]
    bool NonDestroy = false;
    [SerializeField]
    AudioClip ShotSe = null;

    GameObject PlayerObj = null;
    float BulletTime = 0;
    bool First = false;
    public Vector3 TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = SearchTag(gameObject, "Player");
    }

    /// <summary>
    /// 弾の移動
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {

            Destroy(gameObject, DestroyTime);
            BulletTime += Time.deltaTime;

            switch (BulletCategory)
            {
                case 1:
                    transform.Translate(0, 0, BulletMove * Time.deltaTime);
                    break;
                case 2:
                    Vector3 startPos, endPos;
                    startPos = new Vector3();
                    endPos = new Vector3(10, 0, 30);
                    Vector3 dist = endPos - startPos;
                    transform.Translate(dist.normalized * BulletMove * Time.deltaTime);

                    transform.Translate(Vector3.Cross(dist.normalized, transform.up) * BulletMove);
                    break;
                case 3:
                    transform.Translate(0, 0, BulletMove * Time.deltaTime);
                    if (BulletTime >= 1 && First == false)
                    {
                        TargetPos = PlayerObj.transform.position;
                        //プレイヤーのYの位置と敵のYの位置を同じにしてX軸が回転しないようにします。
                        TargetPos.y = this.transform.position.y;
                        transform.LookAt(TargetPos);//対象の位置方向を向く
                        First = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// プレイヤーに当たったら弾を削除する
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !NonDestroy)
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.tag == "Player" && NonDestroy)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
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
        GameObject targetObj = null;//オブジェクト
        //tag指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);//自身と取得したオブジェクトの距離を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }
        }
        return targetObj;
    }
}
