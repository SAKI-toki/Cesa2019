using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    GameObject Star = null;

    [SerializeField]
    int Hp = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "PlayerAttack")
        {
            Hp -= PlayerController.AttackVal;
            if (Hp <= 0)
            {
                Instantiate(Star, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

    }
}
