using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剣の軌跡
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class BladeAfterImageMesh : MonoBehaviour
{
    //残像メッシュ
    Mesh AfterImageMesh;
    [SerializeField, Header("剣先のTransform")]
    Transform BladeTipTransform = null;
    [SerializeField, Header("剣元のTransform")]
    Transform BladeEndTransform = null;
    [SerializeField, Header("最大面数")]
    int MaxSize = 10;
    //1フレーム前の位置
    Vector3 PrevTipPos = new Vector3(), PrevEndPos = new Vector3();
    //頂点リスト
    List<Vector3> PositionList = new List<Vector3>();
    //UVリスト
    List<Vector2> UvList = new List<Vector2>();
    //インデックスリスト
    List<int> IndexList = new List<int>();

    /// <summary>
    /// 各値の初期化
    /// </summary>
    void Start()
    {
        AfterImageMesh = GetComponent<MeshFilter>().mesh;
        PrevTipPos = BladeTipTransform.position;
        PrevEndPos = BladeEndTransform.position;
    }
    
    /// <summary>
    /// 全てのUpdateが終わってからメッシュ更新する
    /// </summary>
    void LateUpdate()
    {
        CreateMesh();
        transform.position = new Vector3();
        AfterImageMesh.RecalculateBounds();
        AfterImageMesh.RecalculateNormals();
    }

    /// <summary>
    /// メッシュの作成
    /// </summary>
    void CreateMesh()
    {
        //頂点
        {
            var tipPos = BladeTipTransform.position;
            var endPos = BladeEndTransform.position;
            PositionList.AddRange(new Vector3[]
            {
            PrevTipPos,PrevEndPos,tipPos,
            tipPos,PrevEndPos,endPos
            });
            PrevTipPos = tipPos;
            PrevEndPos = endPos;
            int maxDif = PositionList.Count - MaxSize * 3;
            if (maxDif > 0)
            {
                PositionList.RemoveRange(0, maxDif);
            }
        }
        //UV
        {
            UvList.Clear();
            int size = PositionList.Count / 6;
            Vector2 prevTipUv = new Vector2(0, 0), prevEndUv = new Vector2(0, 1);
            Vector2 currentTipUv, currentEndUv;
            for (int i = 0; i < size; ++i)
            {
                currentTipUv = new Vector2(((float)(i + 1)) / size, 0);
                currentEndUv = new Vector2(((float)(i + 1)) / size, 1);
                UvList.AddRange(new Vector2[]
                {
                prevTipUv,prevEndUv,currentTipUv,
                currentTipUv,prevEndUv,currentEndUv
                });
                prevTipUv = currentTipUv;
                prevEndUv = currentEndUv;
            }
        }
        //三角形
        {
            IndexList.Clear();
            for (int i = 0; i < PositionList.Count; ++i)
            {
                IndexList.Add(i);
            }
        }
        AfterImageMesh.vertices = PositionList.ToArray();
        AfterImageMesh.uv = UvList.ToArray();
        AfterImageMesh.triangles = IndexList.ToArray();
    }
}
