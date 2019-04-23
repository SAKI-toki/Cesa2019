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
    [SerializeField]
    ShortDistanceGeminiEnemy ShortGeminiEnemy = null;

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

    void Start()
    {
        State = BulletState;
    }

    void Update()
    {
        if (GetHpPercent() < 0.25f &&
            ShortGeminiEnemy.GetHpPercent() < 0.25f) 
        {
            State = UnionState;
        }
        State?.Invoke();
    }

    /// <summary>
    /// 弾のステート
    /// </summary>
    void BulletState()
    {
        BulletTimeCount += Time.deltaTime;
        transform.LookAt(ThisEnemy.PlayerController.transform);
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
        transform.LookAt(ThisEnemy.PlayerController.transform);
        if (LaserTimeCount > LaserTimeLimit)
        {
            ShotLaser();
            State = IdleState;
            AfterIdleState = BulletState;
            IdleTimeLimit = AfterLaserIdleTimeLimit;
        }
    }

    /// <summary>
    /// 弾の発射
    /// </summary>
    void ShotBullet()
    {
        Instantiate(BulletObject, transform.position, transform.rotation);
    }

    /// <summary>
    /// レーザーの発射
    /// </summary>
    void ShotLaser()
    {
        GameObject obj=Instantiate(LaserObject, transform.position, transform.rotation);
        obj.GetComponent<GeminiLaser>().LaserInit();
        //obj.transform.Rotate
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, 20, 0);
        obj.GetComponent<GeminiLaser>().LaserInit();
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, -20, 0);
        obj.GetComponent<GeminiLaser>().LaserInit();
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, 40, 0);
        obj.GetComponent<GeminiLaser>().LaserInit();
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, -40, 0);
        obj.GetComponent<GeminiLaser>().LaserInit();
    }

    void UnionState()
    {
        if (Vector3.Distance(transform.position, ShortGeminiEnemy.gameObject.transform.position) < 4.0f)
        {
            State = null;
            return;
        }
        transform.LookAt(ShortGeminiEnemy.gameObject.transform);
        transform.Translate(0, MoveSpeed * Time.deltaTime, 0);
    }

    public float GetHpPercent()
    {
        return ThisEnemy.EnemyStatus.CurrentHp / ThisEnemy.EnemyStatus.Hp;
    }
}
