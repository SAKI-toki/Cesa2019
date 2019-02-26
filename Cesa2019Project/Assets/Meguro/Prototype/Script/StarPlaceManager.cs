using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 星を置く処理を管理する
/// </summary>
public class StarPlaceManager : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    GameObject Player = null;               // プレイヤー
    [SerializeField, Header("星")]
    GameObject Star = null;                 // 星
    [SerializeField, Header("星選択UI")]
    GameObject StarSelectUI = null;         // 星の色を選択するUI
    //[SerializeField, Header("最初に選択されるボタン")]
    //GameObject StartButton = null;
    Vector3 PlayerPos = Vector3.zero;       // プレイヤーの位置
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
                            if (PlayerController.StarHave > 0)
                            {
                                StarPlaceList[i].isActive = true;
                            }
                            // 星を持っていない
                            else if (PlayerController.StarHave <= 0)
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
                            if (Input.GetKeyDown("joystick button 2"))
                            {
                                StarSelectPlaceNum = i;
                                StarSelectActive();
                            }
                        }
                    }
                }
            }
            // 星の色選択
            else if(StarSelect)
            {
                // 色決定
                if(Input.GetKeyDown("joystick button 1"))
                {
                    StarSet();
                }
                // キャンセル
                if(Input.GetKeyDown("joystick button 2"))
                {
                    StarSelectCancel();
                }
            }
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

    void StarSet()
    {
        StarSelect = false;
        Time.timeScale = 1.0f;
        StarSelectUI.SetActive(false);
        StarPlaceList[StarSelectPlaceNum].isSet = true;
        --PlayerController.StarHave;
        GameObject star = Instantiate(Star, PlayerPos + new Vector3(0, 2, 0), Quaternion.identity);

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
}
