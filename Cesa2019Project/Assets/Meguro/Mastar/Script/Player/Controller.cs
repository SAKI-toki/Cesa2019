using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    public float LeftStickH;
    public float LeftStickV;
    public float Trigger;
    bool RTriggerActive;

    public void Init()
    {
        RTriggerActive = false;
    }

    public void Update()
    {
        LeftStickH = Input.GetAxis("L_Stick_H");
        LeftStickV = Input.GetAxis("L_Stick_V");
        Trigger = Input.GetAxis("L_R_Trigger");
    }

    /// <summary>
    /// トリガーが押されたときに1回の判定処理
    /// </summary>
    /// <returns></returns>
    public bool TriggerButtonDown()
    {
        if (!RTriggerActive && Trigger > 0)
        {
            RTriggerActive = true;
            return true;
        }
        else if (RTriggerActive && Trigger == 0)
        {
            RTriggerActive = false;
        }
        return false;
    }
}
