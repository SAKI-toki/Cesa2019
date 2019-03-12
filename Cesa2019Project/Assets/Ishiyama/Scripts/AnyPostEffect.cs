using UnityEngine;

/// <summary>
/// ポストエフェクト
/// </summary>
[ExecuteInEditMode]//実行していなくても適応される
public class AnyPostEffect : MonoBehaviour
{
    [SerializeField, Header("マテリアル")]
    Material InvertMaterial = null;

    /// <summary>
    /// 全てのレンダリングが完了した時に呼ばれる関数
    /// </summary>
    /// <param name="src">コピー元の画像</param>
    /// <param name="dest">コピー先のRenderTextureオブジェクト</param>
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, InvertMaterial);
    }
}
