using UnityEngine;
using UnityEditor;

/// <summary>
/// MeshFilterの三角ポリゴンの数を出力
/// </summary>
[CustomEditor(typeof(MeshFilter))]
public class PolygonCounter : Editor
{
    /// <summary>
    /// Inspectorに表示
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshFilter filter = target as MeshFilter;
        string polygons = "Triangles: "+ filter.sharedMesh.triangles.Length / 3;
        EditorGUILayout.LabelField(polygons);
    }
}

/// <summary>
/// SkinnedMeshRendererの三角ポリゴンの数を出力
/// </summary>
[CustomEditor(typeof(SkinnedMeshRenderer))]
public class SkinPolygonCounter : Editor
{
    /// <summary>
    /// Inspectorに表示
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SkinnedMeshRenderer skin = target as SkinnedMeshRenderer;
        string polygons = "Triangles: " + skin.sharedMesh.triangles.Length / 3;
        EditorGUILayout.LabelField(polygons);
    }
}
