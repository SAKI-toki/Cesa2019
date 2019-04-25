using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimationController : MonoBehaviour
{
    public enum FaceTypes
    {
        Defalut,
        Angry,
        Happy,
        No
    }

    [SerializeField]
    Material FaceMat;
    [SerializeField]
    Vector2 FaceAtlasSize;

    struct AnimationInfo
    {
        public Vector2 TextureSize;
        public Rect Atlas;
        public int Type;
        public Material Mat;
        public int VNum;
        public int HNum;
    }
    AnimationInfo FaceInfo;

    void Awake()
    {
        FaceInfo.Mat = FaceMat;
        Texture texture = FaceInfo.Mat.mainTexture;
        FaceInfo.TextureSize.x = texture.width;
        FaceInfo.TextureSize.y = texture.height;
        FaceInfo.Atlas.width = FaceAtlasSize.x;
        FaceInfo.Atlas.height = FaceAtlasSize.y;
        FaceInfo.VNum = (int)(FaceInfo.TextureSize.y / FaceInfo.Atlas.height);
        FaceInfo.HNum = (int)(FaceInfo.TextureSize.x / FaceInfo.Atlas.width);
    }

    public void FaceChange(FaceTypes type)
    {
        FaceInfo.Type = (int)type;

        FaceInfo.Atlas.x = ((int)type / FaceInfo.VNum);
        FaceInfo.Atlas.y = ((int)type - (FaceInfo.Atlas.x * FaceInfo.VNum));
        FaceInfo.Atlas.x *= FaceInfo.Atlas.width;
        FaceInfo.Atlas.y *= FaceInfo.Atlas.height;

        // UV座標計算
        Vector2 offset;
        offset = new Vector2(FaceInfo.Atlas.x / FaceInfo.TextureSize.x, 1.0f - (FaceInfo.Atlas.y / FaceInfo.TextureSize.y));

        // 適用
        FaceInfo.Mat.SetTextureOffset("_MainTex", offset);
    }

    void Update()
    {
        /*debug用*/
        if (Input.GetKeyDown(KeyCode.Alpha7)) { FaceChange(FaceTypes.Defalut); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { FaceChange(FaceTypes.Angry); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { FaceChange(FaceTypes.Happy); }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { FaceChange(FaceTypes.No); }
    }
}
