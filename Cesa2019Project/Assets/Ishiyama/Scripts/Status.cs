/// <summary>
/// ステータス
/// </summary>
[System.Serializable]
public class Status
{
    //体力
    public float Hp;
    //攻撃力
    public float Attack;
    //防御力
    public float Defense;
    //速さ
    public float Speed { get; set; }
    //スタミナ
    public float Stamina { get; set; }
    //現在の体力
    public float CurrentHp;
    //現在の攻撃力
    public float CurrentAttack;
    //現在の防御力
    public float CurrentDefense;
    //現在の速さ
    public float CurrentSpeed { get; set; }
    //現在のスタミナ
    public float CurrentStamina { get; set; }

    /// <summary>
    /// ステータスの初期化
    /// </summary>
    /// <param name="hp">体力</param>
    /// <param name="attack">攻撃力</param>
    /// <param name="defense">防御力</param>
    /// <param name="speed">速さ</param>
    public void InitStatus(float hp, float attack, float defense, float speed, float stamina)
    {
        Hp = hp;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Stamina = stamina;
        ResetStatus();
    }

    /// <summary>
    /// ステータスをリセットする
    /// </summary>
    public void ResetStatus()
    {
        CurrentHp = Hp;
        CurrentAttack = Attack;
        CurrentDefense = Defense;
        CurrentSpeed = Speed;
        CurrentStamina = Stamina;
    }

    public void HpUp(float num)
    {
        Hp += num;
        CurrentHp = Hp;
    }
    public void HpDown(float num)
    {
        Hp -= num;
        CurrentHp = Hp;
    }

    public void AttackUp(float num)
    {
        Attack += num;
        CurrentAttack = Attack;
    }
    public void AttackDown(float num)
    {
        Attack -= num;
        CurrentAttack = Attack;
    }

    public void DefenseUp(float num)
    {
        Defense += num;
        CurrentDefense = Defense;
    }
    public void DefenseDown(float num)
    {
        Defense -= num;
        CurrentDefense = Defense;
    }

    public void SpeedUp(float num)
    {
        Speed += num;
        CurrentSpeed = Speed;
    }
    public void SpeedDown(float num)
    {
        Speed -= num;
        CurrentSpeed = Speed;
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    /// <param name="attack">与える攻撃力</param>
    /// <param name="defense">受ける防御力</param>
    /// <returns></returns>
    static public float Damage(float attack, float defense)
    {
        //TODO:ダメージ計算
        float damage = attack - defense;
        return (damage > 0) ? damage : 1;
    }
}
