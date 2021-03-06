using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour
{
    [SerializeField, Header("チュートリアルならチェック")]
    bool TutorialFlg = false;

    //背景のUI
    [SerializeField]
    Image ResultPanel = null;

    //「クリア」のテキスト
    [SerializeField]
    TextMeshProUGUI ClearText = null;

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
    [SerializeField, Header("次ステージテキストの背景")]
    Image NextStageImage = null;

    //ステージセレクトUI
    [SerializeField, Header("ステージセレクトテキストの背景")]
    Image StageSelectImage = null;

    //カーソル
    [SerializeField, Header("カーソル(赤)")]
    GameObject CarsorRed = null;
    [SerializeField, Header("カーソル(青)")]
    GameObject CarsorBlue = null;

    //プレイヤー
    [SerializeField]
    GameObject PlayerObj = null;
    //UICanvas
    [SerializeField, Header("UIのCanvas")]
    GameObject UI = null;

    //MiniMap
    [SerializeField, Header("ミニマップ")]
    GameObject MiniMap = null;

    [SerializeField]
    CameraController CameraScript = null;
    [SerializeField]
    Transform ClearPos = null;
    [SerializeField]
    SelectSE SE = null;

    float StartHp = 0;
    float StartAttack = 0;
    float StartDefense = 0;
    float StartSpeed = 0;

    public static int EnemyDownNum = 0;
    bool ClearInitFlg = false;
    bool ClearEffectFlg = false;
    bool ClearAnimationFlg = false;
    bool ClearMoveFlg = false;
    bool ResultMoveFlg = false;
    bool ClearTextFlg = false;
    bool ResultTextFlg = false;
    bool TextFadeOutFlg = false;
    bool TransitionFlg = false;

    bool StickFlg;
    float LStick;
    int NextStage;
    int StageSelect;
    int Carsor;

    float ClearEffectTime = 8;
    float CurrentClearEffectTime = 0;
    float CurrentTime;
    float AnimationTime = 0;

    [SerializeField]
    GameObject ClearEffect = null;

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
        UI.SetActive(true);
        MiniMap.SetActive(true);
    }

    private void Start()
    {
        StartHp = Player.PlayerStatus.Hp;
        StartAttack = Player.PlayerStatus.Attack;
        StartDefense = Player.PlayerStatus.Defense;
        StartSpeed = Player.PlayerStatus.Speed;
        NextStage = 0;
        StageSelect = 1;
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

            // クリア演出の場所まで移動、移動したら星座のエフェクトが出る
            if (!ClearMoveFlg)
            {
                CameraScript.ClearMoveInit();
                CameraScript.ClearMove();
                Vector3 dir = ClearPos.position - PlayerObj.transform.position;
                PlayerObj.GetComponent<Player>().Move(dir, 40);
                float dis = Vector3.Distance(PlayerObj.transform.position, ClearPos.position);
                if (dis < 1.0f)
                {
                    PlayerObj.GetComponent<Player>().PlayerAnimator.SetBool("DashFlg", false);
                    ClearMoveFlg = true;
                    Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward.normalized, new Vector3(1, 0, 1));

                    Instantiate(ClearEffect, Camera.main.transform.position + cameraForward * 35 + new Vector3(0, 9, 0), ClearEffect.transform.rotation);
                }
            }
            // 星座のエフェクトが終わったらクリアポーズ
            if (ClearMoveFlg && !ClearEffectFlg)
            {
                CurrentClearEffectTime += Time.deltaTime;
                if (CurrentClearEffectTime > ClearEffectTime)
                {
                    PlayerObj.GetComponent<Player>().PlayerAnimator.SetBool("WinFlg", true);
                    ClearEffectFlg = true;
                }
            }

            if (ClearEffectFlg && !ClearAnimationFlg)
            {
                AnimationTime += Time.deltaTime;
                // 2:13 2 + 13/24 = 2.5416...sec
                if (AnimationTime > 2.541)
                {
                    PlayerObj.GetComponent<Player>().PlayerAnimator.SetBool("WinFlg", false);
                }
                if (ClearText.color.a < 1)
                {
                    TextFadeIn(ClearText, 0.01f);
                }
                if (ClearText.rectTransform.localPosition.y > 0)
                {
                    ClearText.rectTransform.localPosition += new Vector3(0, -5, 0);
                }

                if (AnimationTime > 2.541 && ClearText.color.a >= 1 && ClearText.rectTransform.localPosition.y <= 0)
                {
                    ClearAnimationFlg = true;
                }
            }
            if (ClearAnimationFlg && !ClearTextFlg)
            {
                if (CameraScript.Distance > 9)
                {
                    CameraScript.ZoomIn(0.1f);
                }
                if (ClearText.color.a > 0)
                {
                    TextFadeOut(ClearText, 0.02f);
                }
                if (CameraScript.Distance <= 9 && ClearText.color.a <= 0)
                {
                    ClearTextFlg = true;
                }
            }
            if (ClearTextFlg && !ResultMoveFlg)
            {

                float dis = Vector3.Distance(PlayerObj.GetComponent<Player>().transform.position, ClearPos.position);
                if (dis < 3.5f)
                {
                    Vector3 dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
                    dir = dir * -0.4f + Camera.main.transform.right * -1.0f;
                    PlayerObj.GetComponent<Player>().Move(dir, 25);
                }
                else
                {
                    PlayerObj.GetComponent<Player>().PlayerAnimator.SetBool("DashFlg", false);
                    Vector3 dir = Camera.main.transform.position - PlayerObj.GetComponent<Player>().transform.position;
                    PlayerObj.GetComponent<Player>().Look(dir);
                    CurrentTime += Time.deltaTime;
                    if (CurrentTime > 2)
                    {
                        ResultMoveFlg = true;
                        HpNumText.text = StartHp + " > " + Player.PlayerStatus.Hp;
                        AttackNumText.text = StartAttack + " > " + Player.PlayerStatus.Attack;
                        DefenseNumText.text = StartDefense + " > " + Player.PlayerStatus.Defense;
                        SpeedNumText.text = StartSpeed + " > " + Player.PlayerStatus.Speed;
                    }
                }
            }
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
                    }
                    else
                    {
                        TextFadeIn(HpNumText, 0.01f);
                        TextFadeIn(AttackNumText, 0.01f);
                        TextFadeIn(DefenseNumText, 0.01f);
                        TextFadeIn(SpeedNumText, 0.01f);
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
            }
            if (ResultTextFlg && !TextFadeOutFlg)
            {
                if (EnemyText.color.a < 1)
                {
                    TextFadeIn(EnemyText, 0.01f);
                }
                else
                {
                    if (EnemyNumText.color.a < 1)
                    {
                        TextFadeIn(EnemyNumText, 0.01f);
                    }
                    else
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
                            }
                        }
                    }
                }
            }
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
                    case "GameScene1-3":
                    case "GameScene2-3":
                    case "GameScene3-3":
                    case "GameScene4-3":
                        StageSelectImage.gameObject.SetActive(true);
                        CarsorBlue.SetActive(true);
                        StageSelectImage.GetComponent<RectTransform>().localPosition = new Vector3(PosX, PosY1, 0);
                        if (Input.GetKeyDown("joystick button 1"))
                        {
                            SE.Dec();
                            /*=================================================*/
                            //遷移
                            /*=================================================*/
                            if (!FadeController.IsFadeOut)
                            {
                                FadeController.FadeOut("SelectScene");
                            }
                        }
                        break;
                    default:
                        SelectStick();
                        NextStageImage.gameObject.SetActive(true);
                        StageSelectImage.gameObject.SetActive(true);
                        StageSelectImage.GetComponent<RectTransform>().localPosition = new Vector3(PosX, PosY2, 0);
                        if (Carsor == NextStage)
                        {
                            CarsorRed.SetActive(true);
                            CarsorBlue.SetActive(false);
                            if (Input.GetKeyDown("joystick button 1"))
                            {
                                SE.Dec();
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
                            if (Input.GetKeyDown("joystick button 1"))
                            {
                                SE.Dec();
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
            SE.Sel();
            if (Carsor == NextStage)
                Carsor = StageSelect;
            else
                Carsor = NextStage;
        }
        //スティックを下に倒す処理
        if (LStick < 0)
        {
            SE.Sel();
            if (Carsor == StageSelect)
                Carsor = NextStage;
            else
                Carsor = StageSelect;
        }
    }

    //キーボード入力
    public void SelectKeyInput(int Top, int Bottom)
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SE.Sel();
            if (Carsor == Top)
                Carsor = Bottom;
            else
                Carsor = Top;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SE.Sel();
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
            SE.Sel();
            if (Carsor == Top)
                Carsor = Bottom;
            else
                Carsor = Top;
        }
        //スティックを下に倒す処理
        if (LStick < 0)
        {
            SE.Sel();
            if (Carsor == Bottom)
                Carsor = Top;
            else
                Carsor = Bottom;
        }
    }
    public int GetCarsor()
    {
        return Carsor;
    }
    //キーボード入力

}