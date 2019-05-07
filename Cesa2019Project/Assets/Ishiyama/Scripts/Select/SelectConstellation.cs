﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星座を選択するステート
/// </summary>
public class SelectConstellation : MonoBehaviour, ISelectSceneState
{
    enum Season { Spring, Summer, Autumn, Winter, None };
    [SerializeField, Header("季節")]
    Season ThisSeason = Season.None;
    [SerializeField, Header("ステージオブジェクトの親オブジェクト")]
    GameObject SelectSceneObject = null;
    [SerializeField, Header("枠")]
    GameObject FlameObject = null;
    [SerializeField, Header("ステージ")]
    List<GameObject> StageList = new List<GameObject>();
    [SerializeField, Header("季節選択ステート")]
    GameObject SelectSeasonStateObject = null;
    //押している間はtrue
    bool InputStickFlg = false;
    //現在の選択中のステージ
    int CurrentStageNum = 0;
    //シーン遷移中かどうか
    bool IsSceneTransition = false;

    void ISelectSceneState.SelectSceneInit()
    {
        ObjectSetActive(true);
    }

    void ISelectSceneState.SelectSceneUpdate(Stack<ISelectSceneState> stateStack)
    {
        if (IsSceneTransition) return;
        InputMoveFlame();
        MoveFlame();
        InputDecisionOrReturn(stateStack);
    }

    void ISelectSceneState.SelectSceneDestroy()
    {
        ObjectSetActive(false);
        CurrentStageNum = 0;
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
        if (leftStickH > 0 && CurrentStageNum < StageList.Count - 1)
        {
            ++CurrentStageNum;
        }
        if (leftStickH < 0 && CurrentStageNum > 0)
        {
            --CurrentStageNum;
        }
    }

    /// <summary>
    /// 決定か戻るの入力
    /// </summary>
    /// <param name="stateStack">ステートのスタック</param>
    void InputDecisionOrReturn(Stack<ISelectSceneState> stateStack)
    {
        //戻る
        if (Input.GetKeyDown("joystick button 0"))
        {
            stateStack.Push(SelectSeasonStateObject.GetComponent(typeof(ISelectSceneState)) as ISelectSceneState);
        }
        else if (Input.GetKeyDown("joystick button 1"))
        {
            string nextSceneName = Constant.SceneName.GameSceneName +
                ((int)ThisSeason + 1).ToString() +
                "-" +
                (CurrentStageNum + 1).ToString();
            if (!FadeController.IsFadeOut)
            {
                FadeController.FadeOut(nextSceneName);
            }
        }
    }

    /// <summary>
    /// 枠を移動する
    /// </summary>
    void MoveFlame()
    {
        FlameObject.transform.position = StageList[CurrentStageNum].transform.position;
    }

    /// <summary>
    /// オブジェクトをアクティブ、非アクティブにする
    /// </summary>
    /// <param name="is_active">アクティブにするかどうか</param>
    void ObjectSetActive(bool is_active)
    {
        SelectSceneObject.SetActive(is_active);
    }
}
