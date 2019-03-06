using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 円形にブラーをかけるPostEffect
/// </summary>
public class RadialBlurPostEffect : MonoBehaviour
{
    [SerializeField, Header("マテリアル")]
    Material RadialBlurMaterial = null;

    /// <summary>
    /// 全てのレンダリングが完了した時に呼ばれる関数
    /// 円形にブラーをかけるポストエフェクト
    /// </summary>
    /// <param name="src">コピー元の画像</param>
    /// <param name="dest">コピー先のRenderTextureオブジェクト</param>
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, RadialBlurMaterial);
    }
}
