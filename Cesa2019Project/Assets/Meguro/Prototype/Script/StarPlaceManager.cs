﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 星を置く処理を管理する
/// </summary>
public class StarPlaceManager : MonoBehaviour
{
    [System.Serializable]
    class Line
    {
        [SerializeField]
        public GameObject StarPlace1 = null;
        [SerializeField]
        public GameObject StarPlace2 = null;
        [System.NonSerialized]
        public bool DorwEnd = false;
    }
    [SerializeField, Header("プレイヤー")]
    GameObject Player = null;               // プレイヤー
    [SerializeField, Header("星")]
    GameObject Star = null;                 // 星
    [SerializeField, Header("星選択UI")]
    GameObject StarSelectUI = null;         // 星の色を選択するUI
    //[SerializeField, Header("最初に選択されるボタン")]
    //GameObject StartButton = null;
    Vector3 PlayerPos = Vector3.zero;       // プレイヤーの位置
    [SerializeField]
    List<Line> LineList = new List<Line>();
    List<StarPlace> StarPlaceList = new List<StarPlace>();      // 星を置く場所のリスト
    [SerializeField, Header("星が置けるようになる距離")]
    float ActiveDistance = 0;               // 星を置けるようになる距離
    int StarSelectPlaceNum = 0;
    public static bool StarSelect = false;  // 星の色を選択中か
    bool AllPlaceSet = false;               // 星が全てセットされているかのフラグ

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (null != child.GetComponent<StarPlace>())
            {
                StarPlaceList.Add(child.GetComponent<StarPlace>());
                //最初からセットしているかどうか
                if(child.GetComponent<StarPlace>().IsAwakeSet)
                {
                    Instantiate(Star, child.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                }
            }
        }
    }

    void Update()
    {
        // 全て星がセットされているか
        if (!AllPlaceSet)
        {
            if (!StarSelect)
            {
                for (int i = 0; i < StarPlaceList.Count; ++i)
                {
                    // 星がセットされているか
                    if (!StarPlaceList[i].isSet)
                    {
                        PlayerPos = Player.transform.position;

                        float distance = Vector3.Distance(StarPlaceList[i].Pos, PlayerPos);
                        // 距離が範囲内か
                        if (distance < ActiveDistance)
                        {
                            // 星を持っていたら
                            if (PlayerController.StarPieceHave >= Constant.ConstNumber.StarConversion)
                            {
                                StarPlaceList[i].isActive = true;
                            }
                            // 星を持っていない
                            else
                            {
                                Debug.Log("====星が無いよ====");
                            }
                        }
                        else if (distance > ActiveDistance)
                        {
                            StarPlaceList[i].isActive = false;
                        }

                        // 範囲内にいるとき
                        if (StarPlaceList[i].isActive)
                        {
                            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.F))
                            {
                                StarSelectPlaceNum = i;
                                StarSelectActive();
                            }
                        }
                    }
                }
            }
            // 星の色選択
            else if (StarSelect)
            {
                // 色決定
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    // 星を置く
                    StarSet();
                }
                // キャンセル
                if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.F))
                {
                    StarSelectCancel();
                }
            }
            LineCheck();
        }
        // 全ての星がセットされている
        else if (AllPlaceSet)
        {

        }
    }

    void StarSelectActive()
    {
        StarSelect = true;
        Time.timeScale = 0;
        StarSelectUI.SetActive(true);
    }

    void StarSelectCancel()
    {
        StarSelect = false;
        Time.timeScale = 1.0f;
        StarSelectUI.SetActive(false);
    }

    /// <summary>
    /// 星の配置
    /// </summary>
    void StarSet()
    {
        StarSelect = false;
        Time.timeScale = 1.0f;
        StarSelectUI.SetActive(false);
        StarPlaceList[StarSelectPlaceNum].isSet = true;
        PlayerController.StarPieceHave -= Constant.ConstNumber.StarConversion;
        GenerateStar(StarSelectPlaceNum);
        //StarPlaceList[StarSelectPlaceNum].Star = Instantiate(Star, StarPlaceList[StarSelectPlaceNum].Pos + new Vector3(0, 1, 0), Quaternion.identity);
        //AllPlaceSet = AllPlaceSetCheck();
    }

    void GenerateStar(int n)
    {
        Instantiate(Star, StarPlaceList[n].gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        AllPlaceSet = AllPlaceSetCheck();
    }

    /// <summary>
    /// 星が全て配置されたかをチェックする
    /// </summary>
    bool AllPlaceSetCheck()
    {
        for (int i = 0; i < StarPlaceList.Count; ++i)
        {
            if (StarPlaceList[i].isSet == false)
            {
                return false;
            }
        }
        Debug.Log("====星のセット完了====");
        return true;
    }

    /// <summary>
    /// 星が配置されて線を描く
    /// </summary>
    void LineCheck()
    {
        for (int i = 0; i < LineList.Count; ++i)
        {
            if (!LineList[i].DorwEnd)
            {
                if (LineList[i].StarPlace1.GetComponent<StarPlace>().isSet && LineList[i].StarPlace2.GetComponent<StarPlace>().isSet)
                {
                    LineRenderer lineRendererStarPlace1 = LineList[i].StarPlace1.GetComponent<LineRenderer>();
                    lineRendererStarPlace1.positionCount = lineRendererStarPlace1.positionCount + 2;
                    lineRendererStarPlace1.SetPosition(lineRendererStarPlace1.positionCount - 2, LineList[i].StarPlace1.GetComponent<StarPlace>().Star.transform.position);
                    lineRendererStarPlace1.SetPosition(lineRendererStarPlace1.positionCount - 1, LineList[i].StarPlace2.GetComponent<StarPlace>().Star.transform.position);
                    LineList[i].DorwEnd = true;
                }
            }
        }
    }
}
