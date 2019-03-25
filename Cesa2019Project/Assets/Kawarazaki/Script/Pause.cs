using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ポーズ画面
/// </summary>
public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseUi = null;

    [SerializeField]
    private StarSlect Slect = null;

    bool PauseFlg = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !Slect.GetSelectFlg()) 
        {
            //ポーズUIのアクティブ、非アクティブを切り替え
            PauseUi.SetActive(!PauseUi.activeSelf);
            //ポーズUIが表示されている時は停止
            if (PauseUi.activeSelf)
            {
                Time.timeScale = 0f;
                PauseFlg = true;
            }
            //表示されていなければ通常進行
            else
            {
                Time.timeScale = 1f;
                PauseFlg = false;
            }
        }
    }

    public bool GetPauseFlg()
    {
        return PauseFlg;
    }
}
