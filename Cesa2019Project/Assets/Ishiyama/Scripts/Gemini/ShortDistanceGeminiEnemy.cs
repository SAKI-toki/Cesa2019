using UnityEngine;

/// <summary>
/// 近距離双子
/// </summary>
public class ShortDistanceGeminiEnemy : MonoBehaviour
{
    [SerializeField]
    Enemy ThisEnemy = null;
    [SerializeField, Header("攻撃するときの突進距離の上限")]
    float AttackLength = 30.0f;
    Vector3 AttackInitPosition = new Vector3();
    [SerializeField, Header("遠距離")]
    Enemy LongEnemy = null;
    [SerializeField, Header("バリア")]
    GameObject BarrierObject = null;
    [SerializeField, Header("バリアの弾")]
    GameObject BarrierBullet = null;

    //移動速度
    const float MoveSpeed = 10.0f;
    //ステートのデリゲート
    delegate void StateType();
    //ステート
    StateType State;
    //時間の計測
    float IdleTimeCount = 0.0f;
    float AttackTimeCount = 0.0f;
    int AttackCount = 0;
    float DecelerationTimeCount = 0.0f;

    const float DefaultIdleTime = 0.3f;
    const float LongIdleTime = 10.0f;
    float IdleTimeLimit = 3.0f;
    bool ShotBarrierFlg = false;

    void Start()
    {
        State = new StateType(this.IdleState);
        BarrierObject.SetActive(false);
    }

    void Update()
    {
        if (ThisEnemy.EnemyStatus.CurrentHp <= ThisEnemy.EnemyStatus.Hp / 4)
        {
            State = null;
            ThisEnemy.NoDamage = true;
        }
        if (!ShotBarrierFlg && State != null && LongEnemy.NoDamage)
        {
            State = ShotBarrierState;
        }
        State?.Invoke();
    }

    /// <summary>
    /// アイドルステート
    /// </summary>
    void IdleState()
    {
        IdleTimeCount += Time.deltaTime;
        if (IdleTimeCount > IdleTimeLimit)
        {
            //攻撃ステートに移行
            State = AttackState;
            AttackInitPosition = transform.position;
            IdleTimeCount = 0.0f;
            //プレイヤーのほうを向く
            var lookAtPos = ThisEnemy.Player.transform.position;
            lookAtPos.y = transform.position.y;
            transform.LookAt(lookAtPos);
        }
    }

    /// <summary>
    /// アタックステート
    /// </summary>
    void AttackState()
    {
        AttackTimeCount += Time.deltaTime;
        transform.position = transform.position + transform.forward * MoveSpeed * Time.deltaTime;

        if (Vector3.Distance(AttackInitPosition, transform.position) > AttackLength ||
            AttackTimeCount > 3.0f)
        {
            //減速ステートに移行
            State = Deceleration;
            ++AttackCount;
            AttackTimeCount = 0.0f;
        }
    }

    /// <summary>
    /// 減速ステート
    /// </summary>
    void Deceleration()
    {
        const float DecelerationTimeLimit = 0.3f;

        DecelerationTimeCount += Time.deltaTime;
        transform.position = transform.position + (transform.forward * MoveSpeed * Time.deltaTime *
            (Mathf.Min(DecelerationTimeLimit - DecelerationTimeCount, DecelerationTimeLimit) / DecelerationTimeLimit));
        if (DecelerationTimeCount > DecelerationTimeLimit)
        {
            DecelerationTimeCount = 0.0f;
            //アタックが10回未満ならデフォルトの時間待機
            if (AttackCount < 10)
            {
                IdleTimeLimit = DefaultIdleTime;
            }
            //アタックが10回目なら長い時間待機
            else
            {
                IdleTimeLimit = LongIdleTime;
                AttackCount = 0;
            }
            //アイドルステートに移行
            State = IdleState;
        }
    }

    void ShotBarrierState()
    {
        ShotBarrierFlg = true;
        Instantiate(BarrierBullet, transform.position + new Vector3(0, 2, 0), transform.rotation).
            GetComponent<GeminiBarrierBullet>().GeminiBarrierBulletInit(
            LongEnemy.transform.position - transform.position);
        State = IdleState;
    }

    public float GetHpPercent()
    {
        return ThisEnemy.EnemyStatus.CurrentHp / ThisEnemy.EnemyStatus.Hp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<GeminiBarrierBullet>() && ThisEnemy.NoDamage)
        {
            BarrierObject.SetActive(true);
            Destroy(other.gameObject);
        }
    }
}