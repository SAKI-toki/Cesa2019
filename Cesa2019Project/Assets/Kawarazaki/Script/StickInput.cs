using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInput : MonoBehaviour
{
    static float LStick;
    static bool StickFlg = false;

    /// <summary>
    /// スティック選択
    /// </summary>
    //static public void SelectStickH(int Select, int Max, int Min, float LStick)
    //{
    //    //LStick = Input.GetAxis("L_Stick_H");
    //    if (LStick == 0)
    //    {
    //        StickFlg = false;
    //        return;
    //    }
    //    if (StickFlg)
    //        return;
    //    StickFlg = true;
    //    if (LStick > 0)
    //    {
    //        AddSelect(Select, Max, Min);
    //    }
    //    if (LStick < 0)
    //    {
    //        DecSelect(Select, Max, Min);
    //    }
    //}

    /// <summary>
    /// Select変数を加算
    /// </summary>
    static public int AddSelect(int Sel, int Max, int Min)
    {
        ++Sel;
        if (Sel > Max)
            Sel = Min;
        return Sel;
    }

    /// <summary>
    /// Select変数を減算
    /// </summary>
    static public int DecSelect(int Sel, int Max, int Min)
    {
        --Sel;
        if (Sel < Min)
            Sel = Max;
        return Sel;
    }
}
