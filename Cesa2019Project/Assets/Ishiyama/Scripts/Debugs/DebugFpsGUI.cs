using UnityEngine;

/// <summary>
/// FPSを表示する
/// </summary>
public class DebugFpsGUI : MonoBehaviour
{
    [SerializeField, Header("表示する位置")]
    Rect RenderRect = new Rect(20, 20, 200, 200);
    [SerializeField, Header("文字サイズ")]
    int CharSize = 20;
    [SerializeField, Header("文字色")]
    Color CharColor = Color.white;

    /// <summary>
    /// レンダリングとGUIイベントのハンドリング
    /// </summary>
    void OnGUI()
    {
        GUI.skin.label.fontSize = CharSize;
        GUI.color = CharColor;
        GUI.Label(RenderRect, "FPS:" + (1.0f / Time.deltaTime).ToString("#.##"));
    }
}
