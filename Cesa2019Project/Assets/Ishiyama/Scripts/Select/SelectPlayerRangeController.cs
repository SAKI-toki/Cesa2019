using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerRangeController : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerObject = null;
    [SerializeField, Header("プレイヤーが動ける半径")]
    float PlayerMoveRange = 10.0f;

    void LateUpdate()
    {
        var playerPosition = PlayerObject.transform.position;
        //高さを考慮しない
        playerPosition.y = 0.0f;
        //プレイヤーの中心からの距離を取得
        var distane = Vector3.Distance(Vector3.zero, playerPosition);
        if (distane > PlayerMoveRange)
        {
            PlayerObject.transform.position += -playerPosition.normalized * (distane - PlayerMoveRange);
        }
    }
}
