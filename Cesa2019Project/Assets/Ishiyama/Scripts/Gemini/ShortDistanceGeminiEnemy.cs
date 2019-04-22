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
    [SerializeField]
    LongDistanceGeminiEnemy LongGeminiEnemy = null;
    Vector3 AttackInitPosition = new Vector3();
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

    void Start()
    {
        State = new StateType(this.IdleState);
    }

    void Update()
    {
        if (GetHpPercent() < 0.25f &&
            LongGeminiEnemy.GetHpPercent() < 0.25f)
        {
            State = UnionState;
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
            transform.LookAt(ThisEnemy.PlayerController.transform);
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

    void UnionState()
    {
        if (Vector3.Distance(transform.position, LongGeminiEnemy.gameObject.transform.position) < 4.0f)
        {
            State = null;
            return;
        }
        transform.LookAt(LongGeminiEnemy.gameObject.transform);
        transform.Translate(0, MoveSpeed * Time.deltaTime, 0);
    }

    public float GetHpPercent()
    {
        return ThisEnemy.EnemyStatus.CurrentHp / ThisEnemy.EnemyStatus.Hp;
    }
}