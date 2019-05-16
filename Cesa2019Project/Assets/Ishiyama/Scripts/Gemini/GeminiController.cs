using UnityEngine;

public class GeminiController : MonoBehaviour
{
    [SerializeField]
    Enemy ThisEnemy = null;
    [SerializeField]
    GameObject BulletObject = null;
    [SerializeField]
    GameObject LaserObject = null;
    [SerializeField, Header("攻撃するときの突進距離の上限")]
    float AttackLength = 30.0f;
    //移動速度
    const float MoveSpeed = 10.0f;
    const float BulletRate = 0.5f;
    float IdleTime;
    const float RushTime = 6.0f;
    const float DecelerationTime = 0.15f;
    const float LaserTime = 3.0f;
    delegate void StateType();
    StateType State;
    StateType AfterIdleState;
    float TimeCount = 0.0f;
    int LoopCount = 0;
    bool IsNextStateLaser = false;
    int BulletCount = 0;
    Vector3 InitRushPosition = new Vector3();
    private void Start()
    {
        IdleTime = 0.1f;
        State = IdleState;
        AfterIdleState = RushState;
    }

    void Update()
    {
        State();
    }

    void IdleState()
    {
        TimeCount += Time.deltaTime;
        if (TimeCount > IdleTime)
        {
            TimeCount = 0.0f;
            State = AfterIdleState;
            if (State == RushState)
            {
                //プレイヤーのほうを向く
                var lookAtPos = ThisEnemy.Player.transform.position;
                lookAtPos.y = transform.position.y;
                transform.LookAt(lookAtPos);
                InitRushPosition = transform.position;
            }
        }
    }

    void RushState()
    {
        TimeCount += Time.deltaTime;
        transform.position = transform.position + transform.forward * MoveSpeed * Time.deltaTime;
        if (Vector3.Distance(InitRushPosition, transform.position) > AttackLength||
            TimeCount > RushTime)
        {
            TimeCount = 0.0f;
            State = DecelerationState;
        }
    }

    void DecelerationState()
    {
        TimeCount += Time.deltaTime;
        transform.position = transform.position + (transform.forward * MoveSpeed * Time.deltaTime *
             (Mathf.Min(DecelerationTime - TimeCount, DecelerationTime) / DecelerationTime));
        if (TimeCount > DecelerationTime)
        {
            TimeCount = 0.0f;
            if (IsNextStateLaser)
            {
                State = IdleState;
                IdleTime = 0.25f;
                AfterIdleState = LaserState;
            }
            else
            {
                State = BulletState;
            }
            IsNextStateLaser = !IsNextStateLaser;
        }
    }

    void BulletState()
    {
        TimeCount += Time.deltaTime;
        var lookAtPos = ThisEnemy.Player.transform.position;
        lookAtPos.y = transform.position.y;
        transform.LookAt(lookAtPos);
        if (TimeCount > BulletRate)
        {
            TimeCount = 0.0f;
            ShotBullet();
            if (++BulletCount >= 10)
            {
                BulletCount = 0;
                State = IdleState;
                AfterIdleState = RushState;
            }
        }
    }

    void LaserState()
    {
        TimeCount += Time.deltaTime;
        if (TimeCount > LaserTime)
        {
            TimeCount = 0.0f;
            ShotLaser();
            if (++LoopCount > 5)
            {
                IdleTime = 3.0f;
            }
            else
            {
                IdleTime = 0.1f;
            }
            State = IdleState;
            AfterIdleState = RushState;
        }
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
        GameObject obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, 15, 0);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, -15, 0);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, 30, 0);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
        obj = Instantiate(LaserObject, transform.position, transform.rotation);
        obj.transform.Rotate(0, -30, 0);
        obj.GetComponent<GeminiLaser>().LaserInit(transform.position);
    }
}
