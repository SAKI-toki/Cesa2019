using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 季節を選択するステート
/// </summary>
public class SelectSeason : MonoBehaviour, ISelectSceneState
{
    //季節の列挙型
    enum Season { Spring, Summer, Autumn, Winter, None };
    //現在の季節
    Season CurrentSeason = Season.Spring;
    [SerializeField, Header("季節選択オブジェクトの親オブジェクト")]
    GameObject SelectSceneObject = null;
    [SerializeField, Header("枠")]
    GameObject FlameObject = null;
    [SerializeField, Header("春")]
    GameObject SpringObject = null;
    [SerializeField, Header("夏")]
    GameObject SummerObject = null;
    [SerializeField, Header("秋")]
    GameObject AutumnObject = null;
    [SerializeField, Header("冬")]
    GameObject WinterObject = null;
    [SerializeField, Header("春ステート")]
    GameObject SpringStateObject = null;
    [SerializeField, Header("夏ステート")]
    GameObject SummerStateObject = null;
    [SerializeField, Header("秋ステート")]
    GameObject AutumnStateObject = null;
    [SerializeField, Header("冬ステート")]
    GameObject WinterStateObject = null;
    //押している間はtrue
    bool InputStickFlg = false;

    void ISelectSceneState.SelectSceneInit()
    {
        ObjectSetActive(true);
        MoveFlame();
    }

    void ISelectSceneState.SelectSceneUpdate(Stack<ISelectSceneState> stateStack)
    {
        InputMoveFlame();
        MoveFlame();
        if (Input.GetKeyDown("joystick button 1"))
        {
            DecisionSeason(stateStack);
        }
    }

    void ISelectSceneState.SelectSceneDestroy()
    {
        ObjectSetActive(false);
    }

    /// <summary>
    /// 枠を移動する入力
    /// </summary>
    void InputMoveFlame()
    {
        float leftStickH = Input.GetAxis("L_Stick_H");
        if (leftStickH == 0)
        {
            InputStickFlg = false;
            return;
        }
        if (InputStickFlg) return;
        InputStickFlg = true;
        if (leftStickH > 0 && CurrentSeason != Season.Winter)
        {
            ++CurrentSeason;
        }
        if (leftStickH < 0 && CurrentSeason != Season.Spring)
        {
            --CurrentSeason;
        }
    }

    /// <summary>
    /// 枠を移動する
    /// </summary>
    void MoveFlame()
    {
        switch (CurrentSeason)
        {
            case Season.Spring:
                FlameObject.transform.position = SpringObject.transform.position;
                break;
            case Season.Summer:
                FlameObject.transform.position = SummerObject.transform.position;
                break;
            case Season.Autumn:
                FlameObject.transform.position = AutumnObject.transform.position;
                break;
            case Season.Winter:
                FlameObject.transform.position = WinterObject.transform.position;
                break;
        }
    }

    /// <summary>
    /// 季節の決定
    /// </summary>
    void DecisionSeason(Stack<ISelectSceneState> stateStack)
    {
        GameObject nextStateObject = null;
        switch (CurrentSeason)
        {
            case Season.Spring:
                nextStateObject = SpringStateObject;
                break;
            case Season.Summer:
                nextStateObject = SummerStateObject;
                break;
            case Season.Autumn:
                nextStateObject = AutumnStateObject;
                break;
            case Season.Winter:
                nextStateObject = WinterStateObject;
                break;
        }
        if (nextStateObject != null)
        {
            stateStack.Push(nextStateObject.GetComponent(typeof(ISelectSceneState)) as ISelectSceneState);
        }
    }

    /// <summary>
    /// オブジェクトをアクティブ、非アクティブにする
    /// </summary>
    /// <param name="is_active">アクティブにするかどうか</param>
    void ObjectSetActive(bool is_active)
    {
        SelectSceneObject.SetActive(is_active);
        //SpringObject.SetActive(is_active);
        //SummerObject.SetActive(is_active);
        //AutumnObject.SetActive(is_active);
        //WinterObject.SetActive(is_active);
        //FlameObject.SetActive(is_active);
    }
}
