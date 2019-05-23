using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiEnemy : MonoBehaviour
{
    Enemy GeminiAttackEnemy = null;
    Enemy GeminiShootEnemy = null;
    [SerializeField, Header("合体後の敵を入れる")]
    GameObject Gemini = null;
    [SerializeField, Header("近距離の敵を入れる")]
    GameObject GeminiAttack = null;
    [SerializeField, Header("遠距離の敵を入れる")]
    GameObject GeminiShoot = null;

    bool First = false;//バグ防止
    // Start is called before the first frame update
    void Start()
    {
        GeminiAttackEnemy = GeminiAttack.GetComponent<Enemy>();
        GeminiShootEnemy = GeminiShoot.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }

        if (GeminiAttackEnemy.EnemyStatus.CurrentHp <= GeminiAttackEnemy.EnemyHp / 4
            && GeminiShootEnemy.EnemyStatus.CurrentHp <= GeminiShootEnemy.EnemyHp / 4
            && !First)
        {
            Destroy(GeminiAttack);
            Destroy(GeminiShoot);
            Gemini.SetActive(true);
            First = true;
        }
    }
}
