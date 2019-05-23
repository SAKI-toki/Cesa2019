using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSceneObjectManager : MonoBehaviour
{
    [SerializeField, Header("黒いマテリアル")]
    Material BlackMat = null;
    [SerializeField, Header("季節オブジェクト")]
    GameObject[] SeasonObject = new GameObject[(int)SelectSeasonInfo.Season.None];
    [SerializeField]
    Player PlayerObj = null;
    [SerializeField]
    Text[] ConstellationText = new Text[3];
    [SerializeField]
    GameObject Cursor = null;
    int CursorNum = 0;
    public static bool[] SeasonUnlock = new bool[(int)SelectSeasonInfo.Season.None];
    static bool First = true;
    public static bool Select = false;
    static public SelectSeasonInfo.Season CurrentSeason;
    float Stick = 0.0f;
    bool Upflg = false;
    bool Downflg = false;
    [SerializeField]
    Vector3 Offset;
    bool LocalFlg = false;
    bool Decide = false;
    void Start()
    {
        Select = false;

        Cursor.SetActive(false);
        for (int i = 0; i < 3; ++i)
        {
            ConstellationText[i].enabled = false;
        }
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

    private void Update()
    {
        if (Decide) return;
        if (TitleController.Title)
            return;
        if (Select)
        {
            Time.timeScale = 0.0f;
            for (int i = 0; i < 3; ++i)
            {
                ConstellationText[i].enabled = true;
            }
            Cursor.SetActive(true);
            PlayerObj.enabled = false;
            switch (CurrentSeason)
            {
                case SelectSeasonInfo.Season.Spring:
                    ConstellationText[0].text = "かに座";
                    ConstellationText[1].text = "しし座";
                    ConstellationText[2].text = "おとめ座";
                    break;
                case SelectSeasonInfo.Season.Summer:
                    ConstellationText[0].text = "てんびん座";
                    ConstellationText[1].text = "さそり座";
                    ConstellationText[2].text = "いて座";
                    break;
                case SelectSeasonInfo.Season.Autumn:
                    ConstellationText[0].text = "やぎ座";
                    ConstellationText[1].text = "みずがめ座";
                    ConstellationText[2].text = "うお座";
                    break;
                case SelectSeasonInfo.Season.Winter:
                    ConstellationText[0].text = "おひつじ座";
                    ConstellationText[1].text = "おうし座";
                    ConstellationText[2].text = "ふたご座";
                    break;
            }
            Stick = Input.GetAxis("L_Stick_V");
            if (Stick > 0)
            {
                Downflg = false;
                if (!Upflg)
                {
                    Upflg = true;
                    --CursorNum;
                }
            }
            else if (Stick < 0)
            {
                Upflg = false;
                if (!Downflg)
                {
                    Downflg = true;
                    ++CursorNum;
                }
            }
            else
            {
                Upflg = false;
                Downflg = false;
            }
            CursorNum = Mathf.Clamp(CursorNum, 0, 2);
            Cursor.transform.position = ConstellationText[CursorNum].transform.position + Offset;
            if (LocalFlg && Input.GetKeyDown("joystick button 1"))
            {
                FadeController.FadeOut("GameScene" +
                ((int)CurrentSeason + 1).ToString()
                + "-" +
                (CursorNum + 1).ToString());
                Time.timeScale = 1.0f;
                Decide = true;
            }
            else if (Input.GetKeyDown("joystick button 0"))
            {
                Select = false;
                Time.timeScale = 1.0f;
            }
            LocalFlg = true;
        }
        else
        {
            LocalFlg = false;
            Cursor.SetActive(false);
            for (int i = 0; i < 3; ++i)
            {
                ConstellationText[i].enabled = false;
            }
            PlayerObj.enabled = true;
            CursorNum = 0;
            Upflg = Downflg = false;
        }
    }
}
