using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// HPゲージ処理
/// </summary>
public class HpGauge : MonoBehaviour
{
    //緑ゲージのHPバー(円)
    [SerializeField]
    public Image FrontCircleHp = null;
    //赤ゲージのHPバー（円）
    [SerializeField]
    public Image BackCircleHp = null;

    //緑ゲージのHPバー(横)
    [SerializeField]
    public Image FrontHorizontalHp = null;
    //赤ゲージのHPバー（横）
    [SerializeField]
    public Image BackHorizontalHp = null;

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
        FrontCircleHp.fillAmount = 1.0f;
        BackCircleHp.fillAmount = 1.0f;
        FrontHorizontalHp.fillAmount= 1.0f;
        BackHorizontalHp.fillAmount = 1.0f;
    }


    void FixedUpdate()
    {
        //HP減少スピード変数
        float reduceSpeed = 1.5f;
        float waitSpeed = 10.0f;

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
        if(PlayerController.PlayerStatus.Hp > 0 ) 
        {
            if(horizonflg)
            {
                FrontHorizontalHp.fillAmount = (PlayerController.PlayerStatus.CurrentHp - (PlayerController.PlayerStatus.Hp * 3 / 7)) / (PlayerController.PlayerStatus.Hp * 4 / 7);
                if(PlayerController.PlayerStatus.CurrentHp <= PlayerController.PlayerStatus.Hp * 3 / 7 )
                {
                    horizonflg = false;
                    circleflg = true;
                }
            }
            else if(circleflg)
            {
                if(PlayerController.PlayerStatus.CurrentHp <= PlayerController.PlayerStatus.Hp * 3 / 7)
                {
                    horizonflg = true;
                    circleflg = false;
                }
                FrontCircleHp.fillAmount = (PlayerController.PlayerStatus.CurrentHp + (PlayerController.PlayerStatus.Hp*1/7))/(PlayerController.PlayerStatus.Hp-(PlayerController.PlayerStatus.Hp*3/7));
                if(PlayerController.PlayerStatus.CurrentHp <= 0)
                {
                    horizonflg = false;
                    circleflg = false;
                }
            }
        }

        //FrontHpを追いかける
        if (FrontCircleHp.fillAmount <= BackCircleHp.fillAmount)
            BackCircleHp.fillAmount -= Mathf.Clamp01(reduceSpeed / waitSpeed * Time.deltaTime);

        if (FrontHorizontalHp.fillAmount <= BackHorizontalHp.fillAmount)
            BackHorizontalHp.fillAmount -= Mathf.Clamp01(reduceSpeed / waitSpeed * Time.deltaTime);

        ////HP回復
        //if (FrontCircleHp.fillAmount >= BackCircleHp.fillAmount)
        //{
        //    BackCircleHp.fillAmount = FrontCircleHp.fillAmount;
        //}
    }
}
