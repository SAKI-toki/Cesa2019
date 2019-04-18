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
    [SerializeField]
    AudioSource AudioSource = null;


    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayOneShot(ShotSe);
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

            switch (BulletCategory)
            {
                case 1:
                    transform.Translate(0, 0, BulletMove * Time.deltaTime);
                    Destroy(gameObject, DestroyTime);
                    break;
                case 2:
                    Vector3 startPos, endPos;
                    startPos = new Vector3();
                    endPos = new Vector3(10, 0, 30);
                    Vector3 dist = endPos - startPos;
                    transform.Translate(dist.normalized * BulletMove * Time.deltaTime);
                    
                    transform.Translate(Vector3.Cross(dist.normalized, transform.up) * BulletMove);
                    break;
                default:
                    break;
            }
        }
    }

    void Halo()
    {

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
}
