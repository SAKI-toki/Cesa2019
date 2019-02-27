﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    GameObject StarPiece = null;        // 星の欠片

    [SerializeField]
    float Hp = 0;                         // 体力
    [SerializeField]
    int StarPieceNum = 0;               // 星の欠片所持数

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            Hp -= PlayerController.PlayerStatus.CurrentAttack;
            if (Hp <= 0)
            {
                for (int i = 0; i < StarPieceNum; ++i)
                {
                    Instantiate(StarPiece, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }
}
