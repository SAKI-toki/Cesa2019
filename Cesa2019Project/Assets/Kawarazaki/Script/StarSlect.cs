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
    [SerializeField]
    GameObject SelectColor = null;
    [SerializeField, Header("赤")]
    GameObject SelectRed = null;
    [SerializeField, Header("緑")]
    GameObject SelectGreen = null;
    [SerializeField, Header("青")]
    GameObject SelectBlue = null;
    [SerializeField]
    StarPlaceManager StarPlaceController = null;
    private int Select;

    float StarScale = 1.0f;
    float v = 0.1f;
    float OriginalScale = 1.0f;
    bool SelectFlg = false;
    
    void Start()
    {
        Select = 0;
    }
    
    void Update()
    {
        if (!SelectFlg) return;
        //0:赤 1:青 2:緑
        switch (Select)
        {
            case 0:
                StarSize(SelectRed);
                OriginalSize(SelectBlue, SelectGreen);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Red) >= 1)
                    {
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Red);
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Red);
                        DeleteSelect();
                    }
                }
                    StarPlaceController.LineCheck();
                break;
            case 1:
                StarSize(SelectBlue);
                OriginalSize(SelectRed, SelectGreen);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Blue) >= 1)
                    {
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Blue);
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Blue);
                        DeleteSelect();
                    }
                    StarPlaceController.LineCheck();
                }
                break;
            case 2:
                StarSize(SelectGreen);
                OriginalSize(SelectRed, SelectBlue);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    if (HaveStarManager.GetBigStar(HaveStarManager.StarColorEnum.Green) >= 1)
                    {
                        HaveStarManager.SubBigStar(HaveStarManager.StarColorEnum.Green);
                        StarPlaceController.StarSet(HaveStarManager.StarColorEnum.Green);
                        DeleteSelect();
                    }
                    StarPlaceController.LineCheck();
                }
                break;
        }


        //色の選択
        if (Input.GetKeyDown("joystick button 5") || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Select++;
            if (Select > 2)
                Select = 0;
        }
        if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Select--;
            if (Select < 0)
                Select = 2;
        }
    }

    /// <summary>
    /// 選択画面を消す処理
    /// </summary>
    public void DeleteSelect()
    {
        Time.timeScale = 1;
        SelectColor.SetActive(false);
        StarPlaceController.StarSelectCancel();
        SelectFlg = false;
    }

    /// <summary>
    /// 選択画面を出す処理 
    /// </summary>
    public void StartSelect()
    {
        Select = 0;
        Time.timeScale = 0;
        SelectColor.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(SelectColor);
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
    /// 選択外の星の位置を元に戻す処理
    /// </summary>
    /// <param name="obj2"></param>
    /// <param name="obj3"></param>
    void OriginalSize(GameObject obj2,GameObject obj3)
    {
        obj2.GetComponent<RectTransform>().localScale = new Vector3(OriginalScale, OriginalScale, 1.0f);
        obj3.GetComponent<RectTransform>().localScale = new Vector3(OriginalScale, OriginalScale, 1.0f);
    }

    public bool GetSelectFlg()
    {
         return SelectFlg; 
    }
}
