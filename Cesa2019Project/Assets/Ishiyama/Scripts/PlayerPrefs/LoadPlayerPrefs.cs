using UnityEngine;

/// <summary>
/// PlayerPrefsからロードするクラス
/// </summary>
static public class LoadPlayerPref
{
    /// <summary>
    /// int型の読み込み
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>読み込んだ値</returns>
    static public int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    /// <summary>
    /// float型の読み込み
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>読み込んだ値</returns>
    static public float LoadFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    /// <summary>
    /// string型の読み込み
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>読み込んだ値</returns>
    static public string LoadString(string key)
    {
        return PlayerPrefs.GetString(key);
    }
}
