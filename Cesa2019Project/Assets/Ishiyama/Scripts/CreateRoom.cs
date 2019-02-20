using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CreateRoom : MonoBehaviour
{
    [SerializeField, Header("部屋のサイズ")]
    Vector3 RoomSize = new Vector3();
    MeshFilter RoomMeshFilter;

    void Start()
    {
        RoomMeshFilter = GetComponent<MeshFilter>();
        RoomSize /= 2;
        CreateCube();
    }

    void CreateCube()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(RoomSize.x,RoomSize.y,RoomSize.z),
            new Vector3(-RoomSize.x,RoomSize.y,RoomSize.z),
            new Vector3(-RoomSize.x,-RoomSize.y,RoomSize.z),
            new Vector3(RoomSize.x,-RoomSize.y,RoomSize.z),
            new Vector3(-RoomSize.x,RoomSize.y,-RoomSize.z),
            new Vector3(RoomSize.x,-RoomSize.y,-RoomSize.z),
            new Vector3(RoomSize.x,RoomSize.y,-RoomSize.z),
            new Vector3(-RoomSize.x,-RoomSize.y,-RoomSize.z)
        };
        mesh.triangles = new int[]
        {
            1,3,
        };
        mesh.RecalculateNormals();
        RoomMeshFilter.sharedMesh = mesh;
    }
}
