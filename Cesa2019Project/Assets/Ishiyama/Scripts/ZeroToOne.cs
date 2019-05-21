using UnityEngine;

/// <summary>
/// 0から1の間をsinカーブのように周期的に回る
/// sinカーブのような滑らかなカーブではなく、直線的
/// </summary>
public class ZeroToOne
{
    //タイムのカウント
    float TimeCount = 0.0f;
    //周期
    float Period = 0.0f;
    
    float StartNum = 0.0f;
    float EndNum = 10.0f;
    float ReturnNum = 0.0f;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="period">周期の初期化</param>
    public void Init(float period)
    {
        Period = period;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="incrementalNum">増分</param>
    public void ZeroToOneUpdate(float incrementalNum)
    {
        TimeCount += incrementalNum;
        //補間
        ReturnNum = Mathf.Lerp(StartNum, EndNum, TimeCount / Period);
        //最後まで行ったらスタートと終わりをスワップしタイムをゼロにする
        if (ReturnNum == EndNum)
        {
            var temp = StartNum;
            StartNum = EndNum;
            EndNum = temp;
            TimeCount = 0.0f;
        }
    }

    /// <summary>
    /// 0から1の間の値を取得
    /// </summary>
    /// <returns>0から1の間の値</returns>
    public float GetZeroToOne()
    {
        return ReturnNum;
    }
}
