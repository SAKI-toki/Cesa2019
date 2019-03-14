using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAddAttack
{
    GameObject TimingCanvas { get; set; }
    Image TimingCircle = null;
    Transform Target = null;
    Vector3 TargetPos = Vector3.zero;
    float StartSize = 0.0f;
    float CurrentSize = 0.0f;
    float CurrentTime = 0.0f;
    public bool TimingFlg { get; private set; }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="circle"></param>
    /// <param name="target"></param>
    public void InitPlayerAddAttack(GameObject canvas, Image circle, Transform target)
    {
        TimingCanvas = canvas;
        TimingCircle = circle;
        Target = target;
        TargetPos = Target.position;
        StartSize = TimingCircle.transform.localScale.x;
        CurrentSize = StartSize;
    }

    /// <summary>
    /// UIをターゲットと一緒に動かす
    /// </summary>
    public void TargetTracking()
    {
        TimingCanvas.transform.position += Target.position - TargetPos;
        TargetPos = Target.position;
        TimingCanvas.transform.LookAt(Camera.main.transform.position);
    }

    /// <summary>
    /// タイミングUIの表示
    /// </summary>
    public void TimingUIAwake()
    {
        TimingFlg = true;
        TimingCanvas.SetActive(true);
        CurrentTime = 0;
        CurrentSize = StartSize;
    }

    /// <summary>
    /// タイミング良く押せたか
    /// </summary>
    /// <returns></returns>
    public bool TimingAttack()
    {
        if (TimingFlg)
        {
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.R))
            {
                if (CurrentTime > 0.35f && CurrentTime < 0.50f)
                {
                    return true;
                }
                else
                {
                    Debug.Log(CurrentTime);
                    return false;
                }
            }
            CurrentTime += Time.deltaTime;
            CurrentSize -= Time.deltaTime * 0.7f;
            TimingCircle.transform.localScale = new Vector3(CurrentSize, CurrentSize, 1);
            if (CurrentSize < 0.1f)
            {
                TimingFlg = false;
                TimingCanvas.SetActive(false);
                return false;
            }
        }
        return false;
    }
}
