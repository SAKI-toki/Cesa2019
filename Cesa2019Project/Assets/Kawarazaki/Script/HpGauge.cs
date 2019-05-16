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

    //HPゲージ初期化
    void Start()
    {
        HorizontalHp.fillAmount = 1.0f;
    }


    void Update()
    {
        HorizontalHp.fillAmount = Player.PlayerStatus.CurrentHp / Player.PlayerStatus.Hp;
    }
}
