using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : MonoBehaviour,IPlayerMotion
{
    int count = 0;

    public void Start()
    {
        Debug.Log("IdleStart");
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ++count;
            Debug.Log("Attack");
        }
        if(count > 2)
        {
            End();
        }
    }

    public void End()
    {
        Next();
    }

    public void Next()
    {

    }
}
