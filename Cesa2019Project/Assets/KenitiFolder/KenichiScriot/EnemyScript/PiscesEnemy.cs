using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiscesEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject PisceBoss = null;

    PlayerController PlayerController;

    Status EnemyStatus = new Status();

    // Start is called before the first frame update
    void Start()
    {
        PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        PisceBoss = GameObject.Find("PiscesBoss");
    }


    // Update is called once per frame
    void Update()
    {
        PiscesMove();
    }

    void PiscesMove()
    {
        if (PisceBoss == null) { Debug.Log(""); return; }
        this.transform.position = PisceBoss.transform.position;
        this.transform.rotation = PisceBoss.transform.rotation;
    }

    /// <summary>
    /// 当たり判定とダメージ判定
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            ++PlayerController.ComboController.CurrentComboNum;
            EnemyStatus.CurrentHp -= Status.Damage(PlayerController.PlayerStatus.CurrentAttack, EnemyStatus.CurrentDefense);//HPを減らす

            if (EnemyStatus.CurrentHp <= 0)
            {
                EnemyStatus.CurrentHp = 0;
            }
        }
    }
}

