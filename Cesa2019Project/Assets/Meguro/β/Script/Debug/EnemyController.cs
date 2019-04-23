using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//======================
// 目黒
// とりあえずの敵
//======================
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    GameObject StarPiece = null;        // 星の欠片

    [SerializeField]
    float Hp = 0;                         // 体力
    [SerializeField]
    float defense = 0;
    [SerializeField]
    int StarPieceNum = 0;               // 星の欠片所持数

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            ++PlayerController.ComboController.CurrentComboNum;
            Hp -= Status.Damage(PlayerController.PlayerStatus.CurrentAttack, defense);
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

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            ++PlayerController.ComboController.CurrentComboNum;
            Hp -= 2;
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
