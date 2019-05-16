using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLastAttack : MonoBehaviour
{
    [SerializeField, Header("タイミングUIのキャンバス")]
    GameObject LastAttackCanvas = null;
    [SerializeField, Header("タイミングのサークル")]
    Image LastAttackCircle = null;
    [SerializeField, Header("タイミングの中心")]
    Image LastAttackIcon = null;
    Color LastAttackIconStartColor = new Color();
    [SerializeField, Header("追加攻撃のエフェクト")]
    ParticleSystem LastAttackEffect = null;
    float CircleStartSize;
    float CircleSize;
    float AttackTime;
    bool AttackEnd;

    private void Start()
    {
        CircleStartSize = LastAttackCircle.transform.localScale.x;
        LastAttackIconStartColor = LastAttackIcon.color;
    }

    public void Init()
    {
        CircleSize = CircleStartSize;
        LastAttackCircle.transform.localScale = new Vector3(CircleSize, CircleSize, 1);
        LastAttackIcon.color = LastAttackIconStartColor;
        AttackTime = 0;
        AttackEnd = false;
        LastAttackCanvas.transform.LookAt(Camera.main.transform.position);
        LastAttackCanvas.SetActive(true);
    }

    public void LastAttackUpdate()
    {
        LastAttackCanvas.transform.LookAt(Camera.main.transform.position);

        if (!AttackEnd)
        {
            AttackTime += Time.deltaTime;
            CircleSize -= Time.deltaTime * 0.7f;
            LastAttackCircle.transform.localScale = new Vector3(CircleSize, CircleSize, 1);

            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return))
            {
                AttackEnd = true;
                //Debug.Log(AttackTime);
                // 成功
                if (AttackTime > 0.40f && AttackTime < 0.56f)
                {
                    Instantiate(LastAttackEffect, transform.position, transform.rotation);
                    LastAttackIcon.color = new Color(0, 100, 100, 1);
                }
                // 失敗
                else
                {
                    LastAttackIcon.color = new Color(100, 0, 0, 1);
                }
            }

            if (CircleSize < 0.35f)
            {
                AttackEnd = true;
                LastAttackIcon.color = new Color(100, 0, 0, 1);
            }
        }
    }

    public void UIHidden()
    {
        AttackEnd = false;
        LastAttackCanvas.SetActive(false);
    }
}
