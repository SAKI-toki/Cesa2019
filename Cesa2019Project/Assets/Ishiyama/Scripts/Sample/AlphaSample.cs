using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マテリアルのアルファ値を変えるサンプル
/// </summary>
public class AlphaSample : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer EnemySkinnedMeshRenderer = null;

    [SerializeField, Range(0.0f, 1.0f)]
    float AlphaNum = 1.0f;
    void Update()
    {
        var col = EnemySkinnedMeshRenderer.material.color;
        col.a = AlphaNum;
        EnemySkinnedMeshRenderer.material.color = col;
    }
}
