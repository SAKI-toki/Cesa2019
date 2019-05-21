using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// HPゲージ処理
/// </summary>
public class HpGauge : MonoBehaviour
{

    //HPバー(横)
    [SerializeField]
    public Image HorizontalHp = null;

    [SerializeField]
    Sprite GreenHpBar = null;
    [SerializeField]
    Sprite YellowHpBar = null;
    [SerializeField]
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
        HorizontalHp.fillAmount = Player.PlayerStatus.CurrentHp / Player.PlayerStatus.Hp;
        //Hpゲージの画像変更
        if (HorizontalHp.fillAmount <= RedZone)
            HorizontalHp.sprite = RedHpBar;
        else if (HorizontalHp.fillAmount <= YellowZone)
            HorizontalHp.sprite = YellowHpBar;
    }
}
