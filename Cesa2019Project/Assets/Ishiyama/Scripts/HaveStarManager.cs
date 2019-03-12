/// <summary>
/// 所持している星の管理
/// </summary>
static public class HaveStarManager
{
    //色の列挙型
    public enum StarColorEnum { Red, Blue, Green, None };
    //小さい星と大きい星を保持する構造体
    struct LittleAndBigStar
    {
        public int Little;
        public int Big;
    }
    //それぞれの色がいくつ星をもっているか保持している
    static LittleAndBigStar[] StarNum = new LittleAndBigStar[(int)(StarColorEnum.None)];

    /// <summary>
    /// 全てゼロにリセットする
    /// </summary>
    static public void AllZeroReset()
    {
        for (int i = 0; i < (int)(StarColorEnum.None); ++i) 
        {
            StarNum[i].Little = 0;
            StarNum[i].Big = 0;
        }
    }

    /// <summary>
    /// 小さい星を1増やす
    /// </summary>
    /// <param name="starColor">増やす星の色</param>
    static public void AddLittleStar(StarColorEnum starColor)
    {
        if (++StarNum[(int)(starColor)].Little >= Constant.ConstNumber.StarConversion)
        {
            StarNum[(int)(starColor)].Little -= Constant.ConstNumber.StarConversion;
            AddBigStar(starColor);
        }
    }
    /// <summary>
    /// 大きい星を1増やす
    /// </summary>
    /// <param name="starColor">増やす星の色</param>
    static public void AddBigStar(StarColorEnum starColor)
    {
        ++StarNum[(int)(starColor)].Big;
        Star.AddBigStarUI(starColor);
    }

    /// <summary>
    /// 小さい星を1減らす
    /// </summary>
    /// <param name="starColor">減らす星の色</param>
    static public void SubLittleStar(StarColorEnum starColor)
    {
        if (StarNum[(int)(starColor)].Little >= 1)
        {
            --StarNum[(int)(starColor)].Little;
        }
    }
    /// <summary>
    /// 大きい星を1減らす
    /// </summary>
    /// <param name="starColor">減らす星の色</param>
    static public void SubBigStar(StarColorEnum starColor)
    {
        if (StarNum[(int)(starColor)].Big >= 1)
        {
            --StarNum[(int)(starColor)].Big;
            Star.SubBigStarUI(starColor, StarNum[(int)(starColor)].Big);
        }
    }

    /// <summary>
    /// 小さい星のゲッタ
    /// </summary>
    /// <param name="starColor">取得する星の色</param>
    /// <returns>指定した色の小さい星の数</returns>
    static public int GetLittleStar(StarColorEnum starColor)
    {
        return StarNum[(int)(starColor)].Little;
    }
    /// <summary>
    /// 大きい星のゲッタ
    /// </summary>
    /// <param name="starColor">取得する星の色</param>
    /// <returns>指定した色の大きい星の数</returns>
    static public int GetBigStar(StarColorEnum starColor)
    {
        return StarNum[(int)(starColor)].Big;
    }
}
