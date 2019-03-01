using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjct : MonoBehaviour
{
    [SerializeField, Header("攻撃判定を出している時間")]
    float DestoroyTime = 1;

    float AttackObjectTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// 時間が来たら削除
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        AttackObjectTime += Time.deltaTime;

        if (AttackObjectTime >= DestoroyTime) { Destroy(gameObject); }

    }
}
