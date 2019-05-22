using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text ErrorMessageText = null;
    float TimeCount = 0.0f;
    const float TimeLimit = 7.0f;
    void Awake()
    {
        if (!ErrorMessageText)
        {
            Debug.LogError("ErrorMessageTextがnullです");
        }
    }

    void Update()
    {
        TimeCount += Time.unscaledDeltaTime;
        ErrorMessageText.text = "<color=#ff0000>Error!</color>\n<color=#ffffff>" +
        ((int)(TimeLimit - TimeCount)).ToString() +
        "秒後にアプリを強制終了します</color>";
        if (TimeCount > TimeLimit)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
        }
    }
}
