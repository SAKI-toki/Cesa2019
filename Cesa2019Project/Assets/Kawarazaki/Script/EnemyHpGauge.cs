using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
/// <summary>
/// 敵HpUI
/// </summary>
public class EnemyHpGauge : MonoBehaviour
{
    //敵のHPバー
    [SerializeField]
    public Image EnemyHp = null;
    
    // Start is called before the first frame update
    void Start()
    {
        EnemyHp.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //カメラと同じ向きに設定
        transform.rotation = Camera.main.transform.rotation;
        //HPバー減少
        EnemyHp.fillAmount = Mathf.Clamp01(Enemy.EnemyStatus.CurrentHp/ Enemy.EnemyStatus.Hp);
    }
}
