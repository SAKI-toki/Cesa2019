using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour
{
    public void SelectStick()
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
            if (Carsor == NextStage)
                Carsor = StageSelect;
            else
                Carsor = NextStage;
        }
        //スティックを下に倒す処理
        if (LStick < 0)
        {
            if (Carsor == StageSelect)
                Carsor = NextStage;
            else
                Carsor = StageSelect;
        }
    }
    float StartHp = 0;
    float StartAttack = 0;
    float StartDefense = 0;
    float StartSpeed = 0;
    public static int EnemyDownNum = 0;
    [SerializeField, Header("チュートリアルならチェック")]
    bool TutorialFlg = false;
    //「クリア」のテキスト
    [SerializeField]
    TextMeshProUGUI ClearText = null;
    //背景のUI
    [SerializeField]
    Image ResultPanel = null;
    //ステータステキスト
    [SerializeField]
    TextMeshProUGUI ResultText = null;
    [SerializeField]
    TextMeshProUGUI HpText = null;
    [SerializeField]
    TextMeshProUGUI HpNumText = null;
    [SerializeField]
    TextMeshProUGUI AttackText = null;
    [SerializeField]
    TextMeshProUGUI AttackNumText = null;
    [SerializeField]
    TextMeshProUGUI DefenseText = null;
    [SerializeField]
    TextMeshProUGUI DefenseNumText = null;
    [SerializeField]
    TextMeshProUGUI SpeedText = null;
    [SerializeField]
    TextMeshProUGUI SpeedNumText = null;
    [SerializeField]
    TextMeshProUGUI EnemyText = null;
    [SerializeField]
    TextMeshProUGUI EnemyNumText = null;
    [SerializeField]
    TextMeshProUGUI EvaluationText = null;
    [SerializeField]
    TextMeshProUGUI NextStageText = null;
    [SerializeField]
    TextMeshProUGUI StageSelectText = null;
    //次ステージUI
    [SerializeField]
    Image NextStageImage = null;
    //ステージセレクトUI
    [SerializeField]
    Image StageSelectImage = null;
    //カーソル
    [SerializeField]
    GameObject CarsorRed = null;
    [SerializeField]
    GameObject CarsorBlue = null;

    //プレイヤー
    [SerializeField]
    GameObject PlayerObj = null;
    //UICanvas
    [SerializeField]
    GameObject UI = null;
    //MiniMap
    [SerializeField]
    GameObject MiniMap = null;
    [SerializeField]
    CameraController CameraScript = null;
    [SerializeField]
    Transform ClearPos = null;

    bool ClearInitFlg = false;
    bool ClearMoveFlg = false;
    bool ResultMoveFlg = false;
    bool ClearTextFlg = false;
    bool ResultTextFlg = false;
    bool TextFadeOutFlg = false;
    bool TransitionFlg = false;

    bool StickFlg;
    float LStick;
    int Carsor;

    const int NextStage = 0;
    const int StageSelect = 1;

    int SceneNumber;
    string SceneName;

    //UIポジション
    float PosX;
    float PosY1, PosY2;
    void Awake()
    {
        EnemyDownNum = 0;
        TextAlphaZero(ClearText);
        ImageAlphaZero(ResultPanel);
        ImageAlphaZero(NextStageImage);
        ImageAlphaZero(StageSelectImage);
        TextAlphaZero(NextStageText);
        TextAlphaZero(StageSelectText);
        TextAlphaZero(ResultText);
        TextAlphaZero(HpText);
        TextAlphaZero(HpNumText);
        TextAlphaZero(AttackText);
        TextAlphaZero(AttackNumText);
        TextAlphaZero(DefenseText);
        TextAlphaZero(DefenseNumText);
        TextAlphaZero(SpeedText);
        TextAlphaZero(SpeedNumText);
        TextAlphaZero(EnemyText);
        TextAlphaZero(EnemyNumText);
        TextAlphaZero(EvaluationText);
        CarsorRed.SetActive(false);
        CarsorBlue.SetActive(false);
        UI.SetActive(true);
        MiniMap.SetActive(true);
    }

    private void Start()
    {
        StartHp = Player.PlayerStatus.Hp;
        StartAttack = Player.PlayerStatus.Attack;
        StartDefense = Player.PlayerStatus.Defense;
        StartSpeed = Player.PlayerStatus.Speed;
        Carsor = NextStage;
        SceneNumber = SceneManager.GetActiveScene().buildIndex;
        SceneName = SceneManager.GetActiveScene().name;
        PosX = 120.0f;
        PosY1 = 0.0f;
        PosY2 = -70.0f;
    }
    void Update()
    {
        if (!ClearInitFlg)
        {
            ClearInitFlg = true;
        }

        if (StarPlaceManager.AllPlaceSet)
        {
            //クリア画面の時ゲーム画面のActiveをfalse
            UI.SetActive(false);
            MiniMap.SetActive(false);

            //カメラとプレイヤーの移動
            if (!ClearMoveFlg)
            {
                CameraScript.ClearMoveInit();
                CameraScript.ClearMove();
                Vector3 dir = ClearPos.position - PlayerObj.transform.position;
                //PlayerObj.GetComponent<Player>().Move(dir, 50);
                float dis = Vector3.Distance(PlayerObj.transform.position, ClearPos.position);
                if (dis < 1.0f) { ClearMoveFlg = true; }
            }

            //クリアテキストのフェードインと移動
            if (ClearMoveFlg && !ClearTextFlg)
            {
                if (CameraScript.Distance > 8)
                {
                    CameraScript.ZoomIn(0.1f);
                }
                else
                {
                    Debug.Log(true);
                    if (ClearText.color.a < 1)
                    {
                        TextFadeIn(ClearText, 0.01f);
                    }
                    if (ClearText.rectTransform.localPosition.y > 0)
                    {
                        ClearText.rectTransform.localPosition += new Vector3(0, -5, 0);
                    }

                    if (ClearText.color.a >= 1 && ClearText.rectTransform.localPosition.y <= 0)
                    {
                        ClearTextFlg = true;
                    }
                }
            }
            if (ClearTextFlg && !ResultMoveFlg)
            {
                if (ClearTextFlg)
                {
                    TextFadeOut(ClearText, 0.01f);
                }
                float dis = Vector3.Distance(PlayerObj.transform.position, ClearPos.position);
                if (dis < 3.5f)
                {
                    Vector3 dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
                    dir = dir * -0.4f + Camera.main.transform.right * -1.0f;
                    //Player.GetComponent<Player>().Move(dir, 30);
                }
                else
                {
                    Vector3 dir = Camera.main.transform.position - PlayerObj.transform.position;
                    //Player.GetComponent<Player>().Look(dir);
                    if (Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized == PlayerObj.transform.forward)
                    {
                        ResultMoveFlg = true;
                        HpNumText.text = StartHp + " > " + Player.PlayerStatus.Hp;
                        AttackNumText.text = StartAttack + " > " + Player.PlayerStatus.Attack;
                        DefenseNumText.text = StartDefense + " > " + Player.PlayerStatus.Defense;
                        SpeedNumText.text = StartSpeed + " > " + Player.PlayerStatus.Speed;
                    }
                }
            }
            //リザルト画面のフェードイン
            if (ResultMoveFlg && !ResultTextFlg)
            {
                if (ResultPanel.color.a < 1)
                {
                    ImageFadeIn(ResultPanel, 0.01f);
                }
                else
                {
                    if (ResultText.color.a < 1)
                    {
                        TextFadeIn(ResultText, 0.01f);
                        TextFadeIn(HpText, 0.01f);
                        TextFadeIn(AttackText, 0.01f);
                        TextFadeIn(DefenseText, 0.01f);
                        TextFadeIn(SpeedText, 0.01f);
                        TextFadeIn(EnemyText, 0.01f);
                    }
                    else
                    {
                        TextFadeIn(HpNumText, 0.01f);
                        TextFadeIn(AttackNumText, 0.01f);
                        TextFadeIn(DefenseNumText, 0.01f);
                        TextFadeIn(SpeedNumText, 0.01f);
                        TextFadeIn(EnemyNumText, 0.01f);
                        if (HpNumText.color.a >= 1)
                        {
                            ResultTextFlg = true;
                            EnemyNumText.text = EnemyDownNum.ToString("00");
                            if (!TutorialFlg)
                            {
                                if (EnemyDownNum > 40)
                                {
                                    EvaluationText.text = "A";
                                }
                                else if (EnemyDownNum > 30)
                                {
                                    EvaluationText.text = "B";
                                }
                                else
                                {
                                    EvaluationText.text = "C";
                                }
                            }
                            else
                            {
                                EvaluationText.text = "A";
                            }
                        }
                    }
                }
                //フェードインのスキップ
                if (Input.GetKeyDown("joystick button 1"))
                {
                    ImageFadeIn(ResultPanel, 1.0f);
                    TextFadeIn(ResultText, 1.0f);
                    TextFadeIn(HpText, 1.0f);
                    TextFadeIn(AttackText, 1.0f);
                    TextFadeIn(DefenseText, 1.0f);
                    TextFadeIn(SpeedText, 1.0f);
                    TextFadeIn(HpNumText, 1.0f);
                    TextFadeIn(AttackNumText, 1.0f);
                    TextFadeIn(DefenseNumText, 1.0f);
                    TextFadeIn(SpeedNumText, 1.0f);
                    TextFadeIn(EnemyText, 1.0f);
                    TextFadeIn(EnemyNumText, 1.0f);
                    return;
                }
            }
            if (ResultTextFlg && !TextFadeOutFlg)
            {
                if (EvaluationText.color.a < 1)
                {
                    TextFadeIn(EvaluationText, 0.01f);
                }
                else
                {
                    if (Input.GetKeyDown("joystick button 1"))
                    {
                        TextFadeOutFlg = true;
                        return;
                    }
                }
            }
            //リザルト画面のフェードアウト
            if (TextFadeOutFlg && !TransitionFlg)
            {
                if (ResultText.color.a > 0)
                {
                    TextFadeOut(ResultText, 0.01f);
                    TextFadeOut(HpText, 0.01f);
                    TextFadeOut(HpNumText, 0.01f);
                    TextFadeOut(AttackText, 0.01f);
                    TextFadeOut(AttackNumText, 0.01f);
                    TextFadeOut(DefenseText, 0.01f);
                    TextFadeOut(DefenseNumText, 0.01f);
                    TextFadeOut(SpeedText, 0.01f);
                    TextFadeOut(SpeedNumText, 0.01f);
                    TextFadeOut(EnemyText, 0.01f);
                    TextFadeOut(EnemyNumText, 0.01f);
                    TextFadeOut(EvaluationText, 0.01f);
                    //フェードアウトのスキップ
                    if (Input.GetKeyDown("joystick button 1"))
                    {
                        TextFadeOut(ResultText, 1.0f);
                        TextFadeOut(HpText, 1.0f);
                        TextFadeOut(HpNumText, 1.0f);
                        TextFadeOut(AttackText, 1.0f);
                        TextFadeOut(AttackNumText, 1.0f);
                        TextFadeOut(DefenseText, 1.0f);
                        TextFadeOut(DefenseNumText, 1.0f);
                        TextFadeOut(SpeedText, 1.0f);
                        TextFadeOut(SpeedNumText, 1.0f);
                        TextFadeOut(EnemyText, 1.0f);
                        TextFadeOut(EnemyNumText, 1.0f);
                        TextFadeOut(EvaluationText, 1.0f);
                    }
                }
                else
                {
                    TransitionFlg = true;
                }
            }
            if (TransitionFlg)
            {
                switch (SceneName)
                {
                    //各ステージの最終ステージの場合ステージセレクトシーンのみの遷移
                    case "GameScene1-3":
                    case "GameScene2-3":
                    case "GameScene3-3":
                    case "GameScene4-3":
                    case "ExtraScene":
                        ImageFadeIn(StageSelectImage, 0.01f);
                        TextFadeIn(StageSelectText, 0.01f);
                        CarsorBlue.SetActive(true);
                        StageSelectImage.GetComponent<RectTransform>().localPosition = new Vector3(PosX, PosY1, 0);
                        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                        {
                            /*=================================================*/
                            //遷移
                            /*=================================================*/
                            if (!FadeController.IsFadeOut)
                            {
                                FadeController.FadeOut("SelectScene");
                            }
                        }
                        break;
                    //最終ステージ以外だったら次のステージかステージセレクトに遷移
                    default:
                        SelectStick(NextStage, StageSelect);
                        SelectKeyInput(NextStage, StageSelect);
                        ImageFadeIn(NextStageImage, 0.05f);
                        ImageFadeIn(StageSelectImage, 0.05f);
                        TextFadeIn(NextStageText, 0.05f);
                        TextFadeIn(StageSelectText, 0.05f);
                        StageSelectImage.GetComponent<RectTransform>().localPosition = new Vector3(PosX, PosY2, 0);
                        if (Carsor == NextStage)
                        {
                            CarsorRed.SetActive(true);
                            CarsorBlue.SetActive(false);
                            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                            {
                                /*=================================================*/
                                //遷移
                                /*=================================================*/
                                if (!FadeController.IsFadeOut)
                                {
                                    NextStageText.text = "次へ";
                                    FadeController.FadeOut(++SceneNumber);
                                }
                            }

                        }
                        if (Carsor == StageSelect)
                        {
                            CarsorRed.SetActive(false);
                            CarsorBlue.SetActive(true);
                            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
                            {
                                /*=================================================*/
                                //遷移
                                /*=================================================*/
                                if (!FadeController.IsFadeOut)
                                {
                                    FadeController.FadeOut("SelectScene");
                                }
                            }
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// スティック選択
    /// </summary>
    public void SelectStick(int Top, int Bottom)
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
            if (Carsor == Top)
                Carsor = Bottom;
            else
                Carsor = Top;
        }
        //スティックを下に倒す処理
        if (LStick < 0)
        {
            if (Carsor == Bottom)
                Carsor = Top;
            else
                Carsor = Bottom;
        }
    }

    //キーボード入力
    public void SelectKeyInput(int Top, int Bottom)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Carsor == Top)
                Carsor = Bottom;
            else
                Carsor = Top;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Carsor == Bottom)
                Carsor = Top;
            else
                Carsor = Bottom;
        }
    }

    public void TextAlphaZero(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    public void ImageAlphaZero(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    public void TextFadeIn(TextMeshProUGUI text, float speed)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + speed);
    }

    public void TextFadeOut(TextMeshProUGUI text, float speed)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - speed);
    }

    public void ImageFadeIn(Image image, float speed)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + speed);
    }

    public void ImageFadeOut(Image image, float speed)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - speed);
    }

    public int GetCarsor()
    {
        return Carsor;
    }
}