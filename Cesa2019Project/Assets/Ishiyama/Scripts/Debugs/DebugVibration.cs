using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// デバッグ用にバイブレーションさせる
/// </summary>
public class DebugVibration : MonoBehaviour
{
    [SerializeField]
    Text VibrationPower = null;

    void Update()
    {
        float left_power, right_power;
        left_power = XinputVibration.GetTrigger(0, false);
        right_power = XinputVibration.GetTrigger(0, true);
        XinputVibration.Vibration(0, left_power, right_power);
        VibrationPower.text = "LeftPower:" + left_power + "RightPower:" + right_power;
    }
}
