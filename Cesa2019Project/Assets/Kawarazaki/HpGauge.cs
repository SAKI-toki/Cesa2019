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
    //[SerializeField]
    //float Hp = 0;
    //float MaxHp;

    //プレイヤーが受けるダメージ
    //[SerializeField]
    //float Damage = 10;

    //HPゲージ初期化
    void Start()
    {
        //MaxHp = Hp;
        FrontHp = GameObject.Find("FrontHp").GetComponent<Image>();
        FrontHp.fillAmount = 1.0f;
        BackHp = GameObject.Find("BackHp").GetComponent<Image>();
        BackHp.fillAmount = 1.0f;
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
        //FrontHp.fillAmount = Mathf.Clamp01(Hp / MaxHp);
        FrontHp.fillAmount = Mathf.Clamp01(PlayerController.PlayerStatus.CurrentHp / PlayerController.PlayerStatus.Hp);
        //FrontHpを追いかける
        if (FrontHp.fillAmount <= BackHp.fillAmount)
            BackHp.fillAmount -= Mathf.Clamp01(reduceSpeed / waitSpeed * Time.deltaTime);

        //HP回復
        if (FrontHp.fillAmount >= BackHp.fillAmount)
        {
            BackHp.fillAmount = FrontHp.fillAmount;
        }
    }
}
