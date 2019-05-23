using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// HPゲージ処理
/// </summary>
public class HpGauge : MonoBehaviour
{

    //HPバー
    [SerializeField, Header("HPゲージ")]
    public Image HorizontalHp = null;

    [SerializeField, Header("緑ゲージ")]
    Sprite GreenHpBar = null;
    [SerializeField, Header("黄ゲージ")]
    Sprite YellowHpBar = null;
    [SerializeField, Header("赤ゲージ")]
    Sprite RedHpBar = null;

    float YellowZone = 0.6f;
    float RedZone = 0.3f;

    //HPゲージ初期化
    void Start()
    {
        HorizontalHp.fillAmount = 1.0f;
        HorizontalHp.sprite = GreenHpBar;
    }


    void Update()
    {
        //HPゲージ減少処理
        HorizontalHp.fillAmount = Player.PlayerStatus.CurrentHp / Player.PlayerStatus.Hp;
        //Hpゲージの画像変更
        if (HorizontalHp.fillAmount <= RedZone)
            HorizontalHp.sprite = RedHpBar;
        else if (HorizontalHp.fillAmount <= YellowZone)
            HorizontalHp.sprite = YellowHpBar;
    }
}
