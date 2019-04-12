using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeoGroundWave : MonoBehaviour
{
    [HideInInspector]
    public bool Ground = false;
    [HideInInspector]
    public bool Check = false;
    [SerializeField]
    LeoEnemy GetLeoEnemy = null;

    private void OnTriggerEnter(Collider collision)
    {
        Ground = false;
        GetLeoEnemy.KnockBackOn();
        GetLeoEnemy.GetMeshAgent.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Ground = true;
       
    }
}
