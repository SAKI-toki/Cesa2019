using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebug : MonoBehaviour
{
    [SerializeField, Header("プレイヤーステータスデバッグUI")]
    GameObject PlayerStatusDebugUI = null;
    List<Text> PlayerStatusDebugText = new List<Text>();

    private void Awake()
    {
        foreach (Transform child in PlayerStatusDebugUI.transform)
        {
            if (null != child.GetComponent<Text>())
            {
                PlayerStatusDebugText.Add(child.GetComponent<Text>());
            }
        }
    }

    private void Update()
    {
        // プレイヤーデバッグUI 表示/非表示
        if (Input.GetKeyDown(KeyCode.P)) { DebugUISwitch(); }
        // Text更新
        DebugPlayerStatusUpdate();
    }

    /// <summary>
    /// デバッグUIの表示/非表示
    /// </summary>
    void DebugUISwitch()
    {
        // 表示
        if (PlayerStatusDebugUI.activeInHierarchy == false) { PlayerStatusDebugUI.SetActive(true); }
        // 非表示
        else if (PlayerStatusDebugUI.activeInHierarchy == true) { PlayerStatusDebugUI.SetActive(false); }
    }

    /// <summary>
    /// デバッグ用Textの更新
    /// </summary>
    void DebugPlayerStatusUpdate()
    {
        PlayerStatusDebugText[0].text = "Hp:      " + Player.PlayerStatus.CurrentHp.ToString();
        PlayerStatusDebugText[1].text = "Attack:  " + Player.PlayerStatus.CurrentAttack.ToString();
        PlayerStatusDebugText[2].text = "Defence: " + Player.PlayerStatus.CurrentDefense.ToString();
        PlayerStatusDebugText[3].text = "Speed:   " + Player.PlayerStatus.CurrentSpeed.ToString();
        PlayerStatusDebugText[4].text = "Stamina: " + Player.PlayerStatus.CurrentStamina.ToString();
    }
}
