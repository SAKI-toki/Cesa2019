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
        HorizontalHp.fillAmount = PlayerController.PlayerStatus.CurrentHp / PlayerController.PlayerStatus.Hp;
        ////ダメージ
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Hp -= Damage;
        //    if (Hp < 0)
        //        Hp = 0;
        //    Debug.Log(0);
        //}
        ////回復
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Hp += Damage;
        //    if (Hp > MaxHp)
        //        Hp = MaxHp;
        //    Debug.Log(1);
        //}
        //HPダメージ
        //if(PlayerController.PlayerStatus.Hp > 0 ) 
        //{
        //    if(horizonflg)
        //    {
        //        HorizontalHp.fillAmount = (PlayerController.PlayerStatus.CurrentHp - (PlayerController.PlayerStatus.Hp * 3 / 7)) / (PlayerController.PlayerStatus.Hp * 4 / 7);
        //        if(PlayerController.PlayerStatus.CurrentHp <= PlayerController.PlayerStatus.Hp * 3 / 7 )
        //        {
        //            horizonflg = false;
        //            circleflg = true;
        //        }
        //    }
        //    else if(circleflg)
        //    {
        //        if(PlayerController.PlayerStatus.CurrentHp <= PlayerController.PlayerStatus.Hp * 3 / 7)
        //        {
        //            horizonflg = true;
        //            circleflg = false;
        //        }
        //        CircleHp.fillAmount = (PlayerController.PlayerStatus.CurrentHp + (PlayerController.PlayerStatus.Hp * 1 / 7)) / (PlayerController.PlayerStatus.Hp * 4 / 7);
        //        if(PlayerController.PlayerStatus.CurrentHp <= 0)
        //        {
        //            horizonflg = false;
        //            circleflg = false;
        //        }
        //    }
        //}
    }
}
