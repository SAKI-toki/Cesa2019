using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フェードや雰囲気を作るカメラ
/// </summary>
public class FadeCamera : MonoBehaviour
{
    [SerializeField, Header("フェードマテリアル")]
    Material FadeMaterial = null;
    [SerializeField, Range(0, 2)]
    float Distance = 0;

    /// <summary>
    /// 全てのレンダリングが完了した時に呼ばれる関数
    /// フェードインアウトを行うポストエフェクト
    /// </summary>
    /// <param name="src">コピー元の画像</param>
    /// <param name="dest">コピー先のRenderTextureオブジェクト</param>
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        FadeMaterial.SetFloat("FadeDistance", Distance);
        Graphics.Blit(src, dest, FadeMaterial);
    }
}
