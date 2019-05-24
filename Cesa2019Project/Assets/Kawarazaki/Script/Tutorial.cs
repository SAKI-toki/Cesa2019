using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField, Header("セレクト用のカメラ")]
    GameObject SelectCameraObject = null;

    [SerializeField]
    Player Player = null;

    [SerializeField]
    Image TutorialText_1BackGround = null;
    [SerializeField]
    Image TutorialText_1 = null;
    [SerializeField]
    Image TutorialText_2BackGround = null;
    [SerializeField]
    Image TutorialText_2 = null;
    [SerializeField]
    Image TutorialText_3BackGround = null;
    [SerializeField]
    Image TutorialText_3 = null;

    int Count = 0;

    bool TutorialFlg1 = false;
    bool TutorialFlg2 = false;
    //bool TutorialFlg3 = false;

    void Start()
    {
        AlphaZero(TutorialText_1BackGround);
        AlphaZero(TutorialText_1);
        AlphaZero(TutorialText_2BackGround);
        AlphaZero(TutorialText_2);
        AlphaZero(TutorialText_3BackGround);
        AlphaZero(TutorialText_3);
    }

    void Update()
    {
        if (SelectCameraObject.activeSelf && Player.NotMove)
        {
            if (!TutorialFlg1)
            {
                FadeIn(TutorialText_1BackGround, 0.01f);
                FadeIn(TutorialText_1, 0.01f);
                if (TutorialText_1.color.a >= 1)
                    TutorialFlg1 = true;
            }
            if (TutorialFlg1)
            {
                if (++Count > 60)
                {
                    FadeOut(TutorialText_1BackGround, 0.01f);
                    FadeOut(TutorialText_1, 0.01f);
                }
                if (TutorialText_1.color.a <= 0)
                {
                    TutorialFlg2 = true;
                }
            }
            if (TutorialFlg2)
            {
                Player.NotMove = false;
                FadeIn(TutorialText_2BackGround, 0.01f);
                FadeIn(TutorialText_2, 0.01f);

                if (Player.Controller.LeftStickV >= 1.0f || Player.Controller.LeftStickH >= 1.0f)
                {
                    FadeOut(TutorialText_2BackGround, 0.01f);
                    FadeOut(TutorialText_2, 0.01f);
                    if (TutorialText_2.color.a <= 0)
                    {
                        FadeIn(TutorialText_3BackGround, 0.01f);
                        FadeIn(TutorialText_3, 0.01f);
                    }
                }
            }
        }
    }

    void AlphaZero(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    void AlphaOne(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    void FadeIn(Image image, float speed)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + speed);
    }

    void FadeOut(Image image, float speed)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - speed);
    }
}
