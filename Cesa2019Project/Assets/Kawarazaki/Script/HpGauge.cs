using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// HPゲージ処理
/// </summary>
public class HpGauge : MonoBehaviour
{
    //HPバー(円)
    [SerializeField]
    public Image CircleHp = null;

    //HPバー(横)
    [SerializeField]
    public Image HorizontalHp = null;

    //プレイヤーHP
    //[SerializeField]
    //float Hp = 200;
    //float MaxHp = 250;

    //プレイヤーが受けるダメージ
    //[SerializeField]
    //float Damage = 10;

    bool circleflg = false;
    bool horizonflg = true;

    //HPゲージ初期化
    void Start()
    {
        //MaxHp = Hp;
        CircleHp.fillAmount = 1.0f;
        HorizontalHp.fillAmount= 1.0f;
    }


    void Update()
    {
        HorizontalHp.fillAmount = Player.PlayerStatus.CurrentHp / Player.PlayerStatus.Hp;
    }
}
