using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearManager : MonoBehaviour
{
    float StartHp = 0;
    float StartAttack = 0;
    float StartDefense = 0;
    float StartSpeed = 0;
    public static int EnemyDownNum = 0;
    [SerializeField,Header("チュートリアルならチェック")]
    bool TutorialFlg = false;
    [SerializeField]
    Text ClearText = null;
    [SerializeField]
    Image Panel = null;
    [SerializeField]
    Text ResultText = null;
    [SerializeField]
    Text HpText = null;
    [SerializeField]
    Text HpNumText = null;
    [SerializeField]
    Text AttackText = null;
    [SerializeField]
    Text AttackNumText = null;
    [SerializeField]
    Text DefenseText = null;
    [SerializeField]
    Text DefenseNumText = null;
    [SerializeField]
    Text SpeedText = null;
    [SerializeField]
    Text SpeedNumText = null;
    [SerializeField]
    Text EnemyText = null;
    [SerializeField]
    Text EnemyNumText = null;
    [SerializeField]
    Text EvaluationText = null;
    [SerializeField]
    Button NextButton = null;
    [SerializeField]
    Button StageSelectButton = null;
    [SerializeField]
    GameObject Player = null;
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

    void Awake()
    {
        EnemyDownNum = 0;
        TextAlphaZero(ClearText);
        ImageAlphaZero(Panel);
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
    }

    private void Start()
    {
        StartHp = PlayerController.PlayerStatus.Hp;
        StartAttack = PlayerController.PlayerStatus.Attack;
        StartDefense = PlayerController.PlayerStatus.Defense;
        StartSpeed = PlayerController.PlayerStatus.Speed;
    }
    void Update()
    {
        if (!ClearInitFlg)
        {
            ClearInitFlg = true;
        }

        if (StarPlaceManager.AllPlaceSet)
        {
            if (!ClearMoveFlg)
            {
                CameraScript.ClearMoveInit();
                CameraScript.ClearMove();
                Vector3 dir = ClearPos.position - Player.transform.position;
                Player.GetComponent<PlayerController>().Move(dir, 30);
                float dis = Vector3.Distance(Player.transform.position, ClearPos.position);
                if (dis < 1.0f) { ClearMoveFlg = true; }
            }
            if (ClearMoveFlg && !ClearTextFlg)
            {
                if (CameraScript.Distance > 9)
                {
                    CameraScript.ZoomIn(0.1f);
                }
                else
                {
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
                float dis = Vector3.Distance(Player.transform.position, ClearPos.position);
                if (dis < 3.5f)
                {
                    Vector3 dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
                    dir = dir * -0.4f + Camera.main.transform.right * -1.0f;
                    Player.GetComponent<PlayerController>().Move(dir, 25);
                }
                else
                {
                    Vector3 dir = Camera.main.transform.position - Player.transform.position;
                    Player.GetComponent<PlayerController>().Look(dir);
                    if (Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized == Player.transform.forward)
                    {
                        ResultMoveFlg = true;
                        HpNumText.text = StartHp.ToString() + " ➡ " + PlayerController.PlayerStatus.Hp;
                        AttackNumText.text = StartAttack.ToString() + " ➡ " + PlayerController.PlayerStatus.Attack;
                        DefenseNumText.text = StartDefense.ToString() + " ➡ " + PlayerController.PlayerStatus.Defense;
                        SpeedNumText.text = StartSpeed.ToString() + " ➡ " + PlayerController.PlayerStatus.Speed;
                    }
                }
            }
            if (ResultMoveFlg && !ResultTextFlg)
            {
                if (Panel.color.a < 1)
                {
                    ImageFadeIn(Panel, 0.01f);
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
                            EnemyNumText.text = EnemyDownNum.ToString();
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
                    NextButton.gameObject.SetActive(true);
                    StageSelectButton.gameObject.SetActive(true);
                }
            }
            if (TransitionFlg)
            {
                if (Input.GetKeyDown("joystick button 1"))
                {
                    /*=================================================*/
                    //遷移
                    /*=================================================*/
                }
            }
        }
    }

    void TextAlphaZero(Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    void ImageAlphaZero(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    void TextFadeIn(Text text, float speed)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + speed);
    }

    void TextFadeOut(Text text, float speed)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - speed);
    }

    void ImageFadeIn(Image image, float speed)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + speed);
    }

    void ImageFadeOut(Image image, float speed)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - speed);
    }
}
