using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [SerializeField]
    PlayerController PlayerControll = null;
    [SerializeField]
    Transform CameraObject = null;
    [SerializeField]
    CameraController CameraControll = null;
    bool CameraMoveFlg = false;
    bool CameraRotationFlg = false;

    void Update()
    {
        if (PlayerControll.DeathFlg)
        {
            if (!CameraMoveFlg)
            {
                if (CameraObject.localEulerAngles.x < 60) { CameraControll.DeathMoveInit(); CameraControll.DeathMove(); }
                else { CameraMoveFlg = true; }
            }

            if (CameraMoveFlg && !CameraRotationFlg)
            {
                CameraControll.DeathRotation();
            }
            if (!FadeController.IsFadeOut)
            {
                FadeController.FadeOut("SelectScene");
            }
        }
    }
}
