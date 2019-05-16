using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    float AddAttackTime = 0;
    float AddAttackCurrentTime = 0;

    private void Update()
    {
        AddAttackCurrentTime += Time.deltaTime;
        if (AddAttackTime < AddAttackCurrentTime)
        {
            Destroy(this.gameObject);
        }
    }
}
