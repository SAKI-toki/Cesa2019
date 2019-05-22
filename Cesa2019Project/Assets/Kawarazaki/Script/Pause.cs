using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// ポーズ画面
/// </summary>
public class Pause : MonoBehaviour
{
    [SerializeField]
    ClearManager Clear = null;
    [SerializeField]
    private StarSlect Slect = null;
    [SerializeField]
    Player PlayerControll = null;

    [SerializeField]
    private GameObject PauseUi = null;
    //カーソル
    [SerializeField]
    private GameObject CarsorRed = null;
    [SerializeField]
    private GameObject CarsorBlue = null;

    //プレイヤーステータステキスト
    [SerializeField]
    TextMeshProUGUI HpText = null;
    [SerializeField]
    TextMeshProUGUI AttackText = null;
    [SerializeField]
    TextMeshProUGUI DefenseText = null;
    [SerializeField]
    TextMeshProUGUI SpeedText = null;

    //変動するSelect変数　最大値、最小値
    //int Select;
    const int Back = 0;
    const int StageSelect = 1;
    //スティック変数
    float LStick;

    bool PauseFlg = false;

    void Update()
    {
        //ポーズボタン入力
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 7"))
            && !Slect.GetSelectFlg() && !StarPlaceManager.AllPlaceSet && !PlayerControll.DeathFlg)
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
                ActiveChange();
            }
        }
        if (GetPauseFlg())
        {
            switch (Clear.GetCarsor())
            {
                //「戻る」
                case Back:
                    CarsorRed.SetActive(true);
                    CarsorBlue.SetActive(false);
                    if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                    {
                        //通常に戻す
                        ActiveChange();
                        PauseUi.SetActive(false);
                    }
                    break;
                //「ステージ」
                case StageSelect:
                    CarsorBlue.SetActive(true);
                    CarsorRed.SetActive(false);
                    if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                    {
                        PauseUi.SetActive(false);
                        FadeController.FadeOut("SelectScene");
                    }
                    break;
            }
        }

        Clear.SelectStick(Back, StageSelect);
        Clear.SelectKeyInput(Back, StageSelect);

        //ステータステキスト
        HpText.text = "HP:" + Player.PlayerStatus.CurrentHp;
        AttackText.text = "ATTACK:" + Player.PlayerStatus.CurrentAttack;
        DefenseText.text = "DEFENCE:" + Player.PlayerStatus.CurrentDefense;
        SpeedText.text = "SPEED:" + Player.PlayerStatus.CurrentSpeed;
    }

    /// <summary>
    /// 通常進行に戻す処理
    /// </summary>
    void ActiveChange()
    {
        if (!Slect.GetSelectFlg())
        {
            Time.timeScale = 1f;
            PauseFlg = false;
        }
    }


    public bool GetPauseFlg()
    {
        return PauseFlg;
    }

}
