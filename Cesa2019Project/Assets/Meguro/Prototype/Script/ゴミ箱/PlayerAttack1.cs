using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack1 : MonoBehaviour,IPlayerMotion
{
    public void Start()
    {
        Debug.Log("Attack1Start");
    }

    public void Update()
    {
        Debug.Log("Attack1Update");
    }

    public void End()
    {
        Debug.Log("Attack1End");
    }

    public void Next()
    {
        Debug.Log("Attack1Next");
    }
}
