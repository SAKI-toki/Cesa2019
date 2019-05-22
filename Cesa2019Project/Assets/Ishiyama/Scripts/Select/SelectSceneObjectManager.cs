using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSceneObjectManager : MonoBehaviour
{
    [SerializeField, Header("黒いマテリアル")]
    Material BlackMat = null;
    [SerializeField, Header("季節オブジェクト")]
    GameObject[] SeasonObject = new GameObject[(int)SelectSeasonInfo.Season.None];
    public static bool[] SeasonUnlock = new bool[(int)SelectSeasonInfo.Season.None];
    static bool First = true;
    void Start()
    {
        if (First)
        {
            SeasonUnlock[0] = true;
            for (int i = 1; i < (int)SelectSeasonInfo.Season.None; ++i)
            {
                SeasonUnlock[i] = (LoadPlayerPref.LoadInt(i.ToString()) == 1);
            }
            First = false;
        }
        else
        {
            for (int i = 0; i < (int)SelectSeasonInfo.Season.None; ++i)
            {
                SavePlayerPrefs.SaveInt(i.ToString(), SeasonUnlock[i] ? 1 : 0);
            }
        }
        for (int i = 0; i < (int)SelectSeasonInfo.Season.None; ++i)
        {
            if (!SeasonUnlock[i])
            {
                SeasonObject[i].GetComponent<MeshRenderer>().material = BlackMat;
            }
        }
    }
}
