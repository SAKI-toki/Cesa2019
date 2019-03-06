using UnityEngine;

static public class SavePlayerPrefs
{
    static public void SaveInt(string key,int n)
    {
        PlayerPrefs.SetInt(key, n);
        PlayerPrefs.Save();
    }
    static public void SaveFloat(string key,float n)
    {
        PlayerPrefs.SetFloat(key, n);
        PlayerPrefs.Save();
    }
    static public void SaveString(string key,string n)
    {
        PlayerPrefs.SetString(key, n);
        PlayerPrefs.Save();
    }
}
