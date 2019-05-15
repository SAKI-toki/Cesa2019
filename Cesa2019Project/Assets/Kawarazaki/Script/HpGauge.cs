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
    public Sprite Green;
    [SerializeField]
    public Sprite Yellow;
    [SerializeField]
    public Sprite Red;


    //HPゲージ初期化
    void Start()
    {
        HorizontalHp.fillAmount = 1.0f;
        HorizontalHp.sprite = Green;
    }


    void Update()
    {
        HorizontalHp.fillAmount = PlayerController.PlayerStatus.CurrentHp / PlayerController.PlayerStatus.Hp;

        if (HorizontalHp.fillAmount <= 0.3f)
        {
            HorizontalHp.sprite = Red;
        }
        else if (HorizontalHp.fillAmount <= 0.6f)
        {
            HorizontalHp.sprite = Yellow;
        }
        else
        {
            HorizontalHp.sprite = Green;
        }
    }
}
