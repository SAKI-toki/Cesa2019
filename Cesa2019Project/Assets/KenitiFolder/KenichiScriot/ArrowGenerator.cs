using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    float BulletTime = 0;
    int BulletCount = 0;
    [SerializeField]
    GameObject Bullet = null;
    [SerializeField]
    AudioClip AttackSE = null;
    [HideInInspector]
    public AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BulletCount >= 3) { Destroy(gameObject); }
        BulletTime += Time.deltaTime;
        if (BulletTime >= 1)
        {
            AudioSource.PlayOneShot(AttackSE);
            GameObject bullet = Instantiate(Bullet) as GameObject;//弾を生成
            bullet.transform.position = transform.position;//指定した位置に移動
            bullet.transform.Rotate(90, 0, 0);//弾の向きを発射方向に
            BulletCount++;
            BulletTime = 0;
        }

    }
}
