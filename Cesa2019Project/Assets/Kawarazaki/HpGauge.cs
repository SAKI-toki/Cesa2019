using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// HPゲージ処理
/// </summary>
public class HpGauge : MonoBehaviour
{
    //緑ゲージのHPバー
    [SerializeField]
    public Image FrontHp;
    //赤ゲージのHPバー
    [SerializeField]
    public Image BackHp;

    //プレイヤーHP
    [SerializeField]
    float Hp = 0;
    float MaxHp;

    //プレイヤーが受けるダメージ
    [SerializeField]
    float Damege = 0;

    //HPゲージ初期化
    void Start()
    {
        MaxHp = Hp;
        FrontHp = GameObject.Find("FrontHp").GetComponent<Image>();
        FrontHp.fillAmount = 1.0f;
        BackHp = GameObject.Find("BackHp").GetComponent<Image>();
        BackHp.fillAmount = 1.0f;
    }
    

    void Update()
    {
        //HP減少スピード変数
        float reducespeed = 1.5f;
        float waitspeed = 10.0f;

        //ダメージ
        if (Input.GetMouseButtonDown(0))
        {
            Hp -= Damege;
            if (Hp < 0)
                Hp = 0;
        }
        //回復
        if (Input.GetMouseButtonDown(1))
        {
            Hp += Damege;
            if (Hp > MaxHp)
                Hp = MaxHp;
        }
        //HPダメージ
        FrontHp.fillAmount = Hp / MaxHp;

        //FrontHpを追いかける
        if (FrontHp.fillAmount <= BackHp.fillAmount)
            BackHp.fillAmount -= reducespeed / waitspeed * Time.deltaTime;

        //HP回復
        if (FrontHp.fillAmount >= BackHp.fillAmount)
        {
            BackHp.fillAmount = FrontHp.fillAmount;
        }


        Debug.Log(Hp);
    }
}
