using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// セレクトシーンのステートのインターフェース
/// </summary>
interface ISelectSceneState
{
    void SelectSceneInit();
    void SelectSceneUpdate(Stack<ISelectSceneState> stateStack);
    void SelectSceneDestroy();
}

/// <summary>
/// セレクトシーンの制御
/// </summary>
public class SelectSceneController : MonoBehaviour
{
    [SerializeField, Header("最初のステート")]
    GameObject FirstStateObject = null;
    //ステート
    ISelectSceneState SelectSceneState;
    //スタック
    Stack<ISelectSceneState> StateStack = new Stack<ISelectSceneState>();

    void Start()
    {
        SelectSceneState = FirstStateObject.GetComponent(typeof(ISelectSceneState)) as ISelectSceneState;
        SelectSceneState.SelectSceneInit();
        FadeController.FadeIn();
    }

    void Update()
    {
        SelectSceneState.SelectSceneUpdate(StateStack);
        if (StateStack.Count > 0)
        {
            SelectSceneState.SelectSceneDestroy();
            SelectSceneState = StateStack.Pop();
            SelectSceneState.SelectSceneInit();
        }
    }
}
