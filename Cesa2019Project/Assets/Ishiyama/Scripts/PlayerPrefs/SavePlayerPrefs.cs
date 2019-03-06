using UnityEngine;

/// <summary>
/// PlayerPrefsにセーブするクラス
/// </summary>
static public class SavePlayerPrefs
{
    /// <summary>
    /// int型の保存
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="n">値</param>
    static public void SaveInt(string key,int n)
    {
        PlayerPrefs.SetInt(key, n);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// float型の保存
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="n">値</param>
    static public void SaveFloat(string key,float n)
    {
        PlayerPrefs.SetFloat(key, n);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// string型の保存
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="n">値</param>
    static public void SaveString(string key,string n)
    {
        PlayerPrefs.SetString(key, n);
        PlayerPrefs.Save();
    }
}
