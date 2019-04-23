using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrate : MonoBehaviour
{
    [SerializeField]
    Enemy GetEnemy = null;
    [SerializeField]
    EnemyMove GetEnemyMove = null;

    float StopTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetEnemy.EnemyTime <= StopTime)
        {
            Move();
        }
        else
        {
            GetEnemyMove.enabled =true;
            this.enabled = false;
        }

    }

    /// <summary>
    /// 移動の制御
    /// </summary>
    void Move()
    {
            GetEnemy.Animator.SetBool("EnemyWalk", true);
            transform.Translate(0, 0, GetEnemy.ZMove * Time.deltaTime);
    }
}
