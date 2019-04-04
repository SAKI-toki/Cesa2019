/// <summary>
/// 星座を定義しているクラス
/// </summary>
static public class Constellations
{
    /// <summary>
    /// 星座の列挙型
    /// Cancer(キャンサー)「かに座」
    /// Leo(リオ)「獅子座」
    /// Virgo(ヴァーゴ)「おとめ座」
    /// Libra(リーブラ)「てんびん座」
    /// Scorpio(スコーピオ)「さそり座」
    /// Sagittarius(サジタリアス)「いて座」
    /// Capricorn(カプリコーン)「やぎ座」
    /// Aquarius(アクエリアス)「みずがめ座」
    /// Pisces(パイシーズ)「うお座」
    /// Aries(アリーズ)「牡羊座」
    /// Taurus(トーラス)「おうし座」
    /// Gemini(ジェミニ)「ふたご座」
    /// </summary>
    public enum Constellation
    {
        Cancer, Leo, Virgo,
        Libra, Scorpio, Sagittarius,
        Capricorn, Aquarius, Pisces,
        Aries, Taurus, Gemini, None
    };

    /// <summary>
    /// 入力された日にちの星座を返す
    /// </summary>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <returns>星座</returns>
    static public Constellation GetConstellation(int month, int day)
    {
        //10月5日なら1005
        //1月15日なら0115が入る
        int unionMonthDay = month * 100 + day;

        //範囲チェックのラムダ式
        System.Func<int, int, int, bool> rangeCheck =
        (int n, int min, int max) =>
        {
            return min <= n && n <= max;
        };

        //1月20日～2月18日
        if (rangeCheck(unionMonthDay, 120, 218)) return Constellation.Aquarius;
        //2月19日～3月20日
        else if (rangeCheck(unionMonthDay, 219, 320)) return Constellation.Pisces;
        //3月21日～4月19日
        else if (rangeCheck(unionMonthDay, 321, 419)) return Constellation.Aries;
        //4月20日～5月20日
        else if (rangeCheck(unionMonthDay, 420, 520)) return Constellation.Taurus;
        //5月21日～6月21日
        else if (rangeCheck(unionMonthDay, 521, 621)) return Constellation.Gemini;
        //6月22日～7月22日
        else if (rangeCheck(unionMonthDay, 622, 722)) return Constellation.Cancer;
        //7月23日～8月22日
        else if (rangeCheck(unionMonthDay, 723, 822)) return Constellation.Leo;
        //8月23日～9月22日
        else if (rangeCheck(unionMonthDay, 823, 922)) return Constellation.Virgo;
        //9月23日～10月23日
        else if (rangeCheck(unionMonthDay, 923, 1023)) return Constellation.Libra;
        //10月24日～11月22日
        else if (rangeCheck(unionMonthDay, 1024, 1122)) return Constellation.Scorpio;
        //11月23日～12月21日
        else if (rangeCheck(unionMonthDay, 1123, 1221)) return Constellation.Sagittarius;
        //12月22日～1月19日
        return Constellation.Capricorn;
    }
}
