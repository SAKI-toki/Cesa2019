using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBlurPostEffect : MonoBehaviour
{
    [SerializeField, Header("マテリアル")]
    Material RadialBlurMaterial = null;

    /// <summary>
    /// 全てのレンダリングが完了した時に呼ばれる関数
    /// フェードインアウトを行うポストエフェクト
    /// </summary>
    /// <param name="src">コピー元の画像</param>
    /// <param name="dest">コピー先のRenderTextureオブジェクト</param>
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, RadialBlurMaterial);
    }
}
