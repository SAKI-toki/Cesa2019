﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 星の選択
/// </summary>
public class StarSlect : MonoBehaviour
{
    [SerializeField, Header("星選択UI")]
    GameObject SelectColor = null;
    [SerializeField, Header("赤")]
    GameObject SelectRed = null;
    [SerializeField, Header("青")]
    GameObject SelectBlue = null;
    [SerializeField, Header("緑")]
    GameObject SelectGreen = null;
    [SerializeField]
    StarPlaceManager StarPlaceController = null;
    [SerializeField]
    SelectSE SE = null;

    private int Select;
    const int SelectMax = 2;
    const int SelectMin = 0;

    //星の大きさ
    float StarScale = 1.0f;
    float v = 0.1f;
    float OriginalScale = 1.0f;

    float LStick;
    bool StickFlg = false;

    bool SelectFlg = false;


    void Start()
    {
        Select = SelectMin;
    }

    void Update()
    {
        if (!SelectFlg) return;

        switch (Select)
        {
            //「赤星」
            case 0:
                StarSize(SelectRed);
                OriginalSize(SelectBlue, SelectGreen);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Red) >= 1)
                    {
                        //星設置音
                        SE.Star();
                        //選択した星を1個減らす
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Red);
                        //選択した星を設置する
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Red);
                        //星選択UIを消す
                        DeleteSelect();
                    }
                }
                //設置した星から線を描く
                StarPlaceController.LineCheck();
                break;
            //「青星」
            case 1:
                StarSize(SelectBlue);
                OriginalSize(SelectRed, SelectGreen);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Blue) >= 1)
                    {
                        SE.Star();
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Blue);
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Blue);
                        DeleteSelect();
                    }
                    StarPlaceController.LineCheck();
                }
                break;
            //「緑星」
            case 2:
                StarSize(SelectGreen);
                OriginalSize(SelectRed, SelectBlue);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Green) >= 1)
                    {
                        SE.Star();
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Green);
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Green);
                        DeleteSelect();
                    }
                    StarPlaceController.LineCheck();
                }
                break;
        }

        //入力処理
        SelectStick();
        SelectKeyInput();
    }

    /// <summary>
    /// スティック選択
    /// </summary>
    void SelectStick()
    {
        LStick = Input.GetAxis("L_Stick_H");
        if (LStick == 0)
        {
            StickFlg = false;
            return;
        }
        if (StickFlg)
            return;
        StickFlg = true;
        if (LStick > 0)
        {
            AddSelect();
        }
        if (LStick < 0)
        {
            DecSelect();
        }
    }

    /// <summary>
    /// キーボード選択
    /// </summary>
    void SelectKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddSelect();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DecSelect();
        }
    }
    /// <summary>
    /// Select変数を加算
    /// </summary>
    void AddSelect()
    {
        SE.Sel();
        ++Select;
        if (Select > SelectMax)
            Select = SelectMin;
    }

    /// <summary>
    /// Select変数を減算
    /// </summary>
    void DecSelect()
    {
        SE.Sel();
        --Select;
        if (Select < SelectMin)
            Select = SelectMax;
    }

    /// <summary>
    /// 選択画面を消す処理
    /// </summary>
    public void DeleteSelect()
    {
        SelectFlg = false;
        Time.timeScale = 1;
        StarPlaceController.StarSelectCancel();
        SelectColor.SetActive(false);
    }

    /// <summary>
    /// 選択画面を出す処理 
    /// </summary>
    public void StartSelect()
    {
        SelectColor.SetActive(true);
        Time.timeScale = 0;
        SelectFlg = true;
    }

    /// <summary>
    /// 選択中の星を動かす処理
    /// </summary>
    /// <param name="obj"></param>
    void StarSize(GameObject obj)
    {
        StarScale = 1.25f + Mathf.Sin(v) * 0.25f;
        v += 0.1f;
        if (v > Mathf.PI) v -= Mathf.PI;
        obj.GetComponent<RectTransform>().localScale = new Vector3(StarScale, StarScale, 1.0f);
    }

    /// <summary>
    /// 選択していない星の位置を元に戻す処理
    /// </summary>
    /// <param name="obj2"></param>
    /// <param name="obj3"></param>
    void OriginalSize(GameObject obj2, GameObject obj3)
    {
        obj2.GetComponent<RectTransform>().localScale = new Vector3(OriginalScale, OriginalScale, 1.0f);
        obj3.GetComponent<RectTransform>().localScale = new Vector3(OriginalScale, OriginalScale, 1.0f);
    }

    public bool GetSelectFlg()
    {
        return SelectFlg;
    }
}
