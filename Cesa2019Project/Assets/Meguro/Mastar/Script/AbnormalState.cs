using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalState
{
    public float ParalysisTime = 0;         // 麻痺時間
    float CurrentParalysisTime = 0;         // 麻痺経過時間
    public bool ParalysisFlg = false;       // 麻痺フラグ
    public float PoisonDamage = 0;          // 毒ダメージ
    public float PoisonTime = 0;            // 毒時間
    float CurrentPoisonTime = 0;            // 毒経過時間
    public float PoisonDamageTime = 0;      // 毒のダメージを受ける間隔
    float CurrentPoisonDamageTime = 0;      // 毒ダメージ間隔の経過時間
    public bool PoisonFlg = false;          // 毒フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="parlysisTime"></param>
    /// <param name="poisonTime"></param>
    /// <param name="poisonDamageTime"></param>
    /// <param name="poisonDamage"></param>
    public void Init(float parlysisTime, float poisonTime, float poisonDamageTime, float poisonDamage)
    {
        ParalysisTime = parlysisTime;
        PoisonTime = poisonTime;
        PoisonDamageTime = poisonDamageTime;
        PoisonDamage = poisonDamage;
    }

    public void ParalysisStart()
    {
        ParalysisFlg = true;
        CurrentParalysisTime = 0;
    }

    public void PoisonStart()
    {
        PoisonFlg = true;
        CurrentPoisonTime = 0;
    }

    /// <summary>
    /// 異常状態処理
    /// </summary>
    /// <param name="hp"></param>
    public void Abnormal(ref float hp)
    {
        if (ParalysisFlg)
        {
            Paralysis();
        }
        if (PoisonFlg)
        {
            Poison(ref hp);
        }
    }

    /// <summary>
    /// 麻痺
    /// </summary>
    public void Paralysis()
    {

        CurrentParalysisTime += Time.deltaTime;
        // 麻痺終了判定
        if (CurrentParalysisTime > ParalysisTime)
        {
            ParalysisFlg = false;
            CurrentParalysisTime = 0;
        }
    }

    /// <summary>
    /// 毒
    /// </summary>
    /// <param name="hp"></param>
    public void Poison(ref float hp)
    {

        CurrentPoisonTime += Time.deltaTime;
        CurrentPoisonDamageTime += Time.deltaTime;

        // 毒ダメージ
        if (CurrentPoisonDamageTime > PoisonDamageTime)
        {
            hp -= PoisonDamage;
            CurrentPoisonDamageTime = 0;
        }
        // 毒終了判定
        if (CurrentPoisonTime > PoisonTime)
        {
            PoisonFlg = false;
            CurrentPoisonTime = 0;
            CurrentPoisonDamageTime = 0;
        }
    }
}
