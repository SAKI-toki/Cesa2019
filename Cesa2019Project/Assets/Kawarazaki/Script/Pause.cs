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
    SelectSE SE = null;

    [SerializeField, Header("ポーズUI")]
    private GameObject PauseUi = null;
    //カーソル
    [SerializeField, Header("カーソル(赤)")]
    private GameObject CarsorRed = null;
    [SerializeField, Header("カーソル(青)")]
    private GameObject CarsorBlue = null;

    //プレイヤーステータステキスト
    [SerializeField, Header("HPテキスト")]
    TextMeshProUGUI HpText = null;
    [SerializeField, Header("ATTACKテキスト")]
    TextMeshProUGUI AttackText = null;
    [SerializeField, Header("DEFENSEテキスト")]
    TextMeshProUGUI DefenseText = null;
    [SerializeField, Header("SPEEDテキスト")]
    TextMeshProUGUI SpeedText = null;

    const int Back = 0;
    const int StageSelect = 1;

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
                        SE.Dec();
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
                        SE.Dec();
                        PauseUi.SetActive(false);
                        FadeController.FadeOut("SelectScene");
                    }
                    break;
            }

            //入力処理
            Clear.SelectStick(Back, StageSelect);
            Clear.SelectKeyInput(Back, StageSelect);
        }

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
