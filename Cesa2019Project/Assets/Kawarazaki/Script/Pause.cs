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
    private GameObject PauseUi = null;
    //カーソル
    [SerializeField]
    private GameObject CarsorRed = null;
    [SerializeField]
    private GameObject CarsorBlue = null;
    [SerializeField]
    private GameObject CarsorGreen = null;

    [SerializeField]
    private StarSlect Slect = null;
    [SerializeField]
    Player PlayerControll = null;
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
    int Select, SelectMax, SelectMin;
    //スティック変数
    float LStick;
    //スティックフラグ
    bool StickFlg = false;

    bool PauseFlg = false;


    void Start()
    {
        Select = 0;
        SelectMin = Select;
        SelectMax = 2;
    }

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

        switch (Select)
        {
            //「戻る」
            case 0:
                CarsorRed.SetActive(true);
                CarsorFalse(CarsorBlue, CarsorGreen);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    PauseUi.SetActive(false);
                    //通常に戻す
                    ActiveChange();
                }
                break;
            //「ステージ」
            case 1:
                CarsorBlue.SetActive(true);
                CarsorFalse(CarsorRed, CarsorGreen);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    PauseUi.SetActive(false);
                    FadeController.FadeOut("SelectScene");
                }
                break;
            //「タイトル」
            case 2:
                CarsorGreen.SetActive(true);
                CarsorFalse(CarsorBlue, CarsorRed);
                if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                {
                    PauseUi.SetActive(false);
                    FadeController.FadeOut("SelectScene");
                }
                break;
        }
        SelectStick();
        //カーソルのポジション設定
        //SelectCarsor.GetComponent<RectTransform>().localPosition = new Vector3(CarsorPosX, CarsorPosY, 1.0f);

        //ステータステキスト
        HpText.text = "HP:" + Player.PlayerStatus.CurrentHp;
        AttackText.text = "ATTACK:" + Player.PlayerStatus.CurrentAttack;
        DefenseText.text = "DEFENCE:" + Player.PlayerStatus.CurrentDefense;
        SpeedText.text = "SPEED:" + Player.PlayerStatus.CurrentSpeed;
    }
    /// <summary>
    /// スティック選択
    /// </summary>
    void SelectStick()
    {
        LStick = Input.GetAxis("L_Stick_V");
        if (LStick == 0)
        {
            StickFlg = false;
            return;
        }
        if (StickFlg)
            return;
        StickFlg = true;
        //スティックを上に倒す処理
        if (LStick > 0)
        {
            DecSelect();
        }
        //スティックを下に倒す処理
        if (LStick < 0)
        {
            AddSelect();
        }
    }

    /// <summary>
    /// Select変数を加算
    /// </summary>
    void AddSelect()
    {
        if (PauseFlg) //ポーズを開いたときにSelect変数が加算される
        {
            ++Select;
            if (Select > SelectMax)
                Select = SelectMin;
        }
    }

    /// <summary>
    /// Select変数を減算
    /// </summary>
    void DecSelect()
    {
        if (PauseFlg)//ポーズを開いたときにSelect変数が減算される
        {
            {
                --Select;
                if (Select < SelectMin)
                    Select = SelectMax;
            }
        }
    }

    void CarsorFalse(GameObject Carsor1, GameObject Carsor2)
    {
        Carsor1.SetActive(false);
        Carsor2.SetActive(false);
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
