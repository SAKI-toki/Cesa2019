using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistanceGeminiEnemy : MonoBehaviour
{
    [SerializeField]
    public Enemy ThisEnemy = null;
    [SerializeField]
    GameObject BulletObject = null;
    [SerializeField]
    GameObject LaserObject = null;
    [SerializeField, Header("近距離")]
    Enemy ShortEnemy = null;
    [SerializeField, Header("バリア")]
    GameObject BarrierObject = null;
    [SerializeField, Header("バリアの弾")]
    GameObject BarrierBullet = null;

    delegate void StateType();

    StateType State;

    float BulletTimeCount = 0.0f;
    int BulletGroupNum = 0;
    float IdleTimeCount = 0.0f;
    float IdleTimeLimit = 0.5f;
    float LaserTimeCount = 0.0f;
    StateType AfterIdleState;

    const float BulletRate = 0.25f;
    const float AfterBulletIdleTimeLimit = 0.5f;
    const float AfterLaserIdleTimeLimit = 5.0f;
    const float LaserTimeLimit = 5.0f;
    const float MoveSpeed = 10.0f;
    bool ShotBarrierFlg = false;

    void Start()
    {
        State = BulletState;
        BarrierObject.SetActive(false);
    }

    void Update()
    {
        if (ThisEnemy.EnemyStatus.CurrentHp <= ThisEnemy.EnemyStatus.Hp / 4)
        {
            State = null;
            ThisEnemy.NoDamage = true;
        }
        if (!ShotBarrierFlg && State != null && ShortEnemy.NoDamage)
        {
            State = ShotBarrierState;
        }
        State?.Invoke();
    }

    /// <summary>
    /// 弾のステート
    /// </summary>
    void BulletState()
    {
        BulletTimeCount += Time.deltaTime;
        var lookAtPos = ThisEnemy.PlayerController.transform.position;
        lookAtPos.y = transform.position.y;
        transform.LookAt(lookAtPos);
        if (BulletTimeCount > BulletRate)
        {
            ShotBullet();
            BulletTimeCount = 0.0f;
            //10回発射したら待機ステートに移行
            if (++BulletGroupNum >= 10)
            {
                State = IdleState;
                AfterIdleState = LaserState;
                IdleTimeLimit = AfterBulletIdleTimeLimit;
                BulletGroupNum = 0;
            }
        }
    }

    /// <summary>
    /// 待機ステート
    /// </summary>
    void IdleState()
    {
        IdleTimeCount += Time.deltaTime;
        if (IdleTimeCount > IdleTimeLimit)
        {
            State = AfterIdleState;
            IdleTimeCount = 0.0f;
        }
    }

    /// <summary>
    /// レーザーステート
    /// </summary>
    void LaserState()
    {
        LaserTimeCount += Time.deltaTime;
        var lookAtPos = ThisEnemy.PlayerController.transform.position;
        lookAtPos.y = transform.position.y;
        transform.LookAt(lookAtPos);
        if (LaserTimeCount > LaserTimeLimit)
        {
            ShotLaser();
            State = IdleState;
            AfterIdleState = BulletState;
            IdleTimeLimit = AfterLaserIdleTimeLimit;
            LaserTimeCount = 0.0f;
        }
    }

    void ShotBarrierState()
    {
        Debug.Log("shot");
        ShotBarrierFlg = true;
        Instantiate(BarrierBullet, transform.position + new Vector3(0, 2, 0), transform.rotation).
            GetComponent<GeminiBarrierBullet>().GeminiBarrierBulletInit(
            ShortEnemy.transform.position - transform.position);
        State = IdleState;
    }

    /// <summary>
    /// 弾の発射
    /// </summary>
    void ShotBullet()
    {
        Instantiate(BulletObject, transform.position + new Vector3(0, 2, 0), transform.rotation);
    }

    /// <summary>
    /// レーザーの発射
    /// </summary>
    void ShotLaser()
    {
        GameObject obj=Instantiate(LaserObject, transform.position, transform.rotation);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, 20, 0);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, -20, 0);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, 40, 0);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, -40, 0);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
    }

    public float GetHpPercent()
    {
        return ThisEnemy.EnemyStatus.CurrentHp / ThisEnemy.EnemyStatus.Hp;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<GeminiBarrierBullet>() && ThisEnemy.NoDamage)
        {
            BarrierObject.SetActive(true);
            Destroy(other.gameObject);
        }
    }
}
