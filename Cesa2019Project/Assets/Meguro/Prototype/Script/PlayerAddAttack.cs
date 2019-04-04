using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAddAttack
{
    GameObject TimingCanvas { get; set; }
    Image TimingCircle = null;
    Image TimingIcon = null;
    Color TimingIconStartColor = new Color();
    Transform Target = null;
    Vector3 TargetPos = Vector3.zero;
    float StartSize = 0.0f;
    float CurrentSize = 0.0f;
    float CurrentTime = 0.0f;
    bool TimingPush = false;
    public bool TimingFlg { get; private set; }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="circle"></param>
    /// <param name="icon"></param>
    /// <param name="target"></param>
    public void InitPlayerAddAttack(GameObject canvas, Image circle, Image icon, Transform target)
    {
        TimingCanvas = canvas;
        TimingCircle = circle;
        TimingIcon = icon;
        TimingIconStartColor = TimingIcon.color;
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
        TimingIcon.color = TimingIconStartColor;
        TimingFlg = true;
        TimingCanvas.SetActive(true);
        CurrentTime = 0;
        CurrentSize = StartSize;
    }

    /// <summary>
    /// タイミングUIの非表示
    /// </summary>
    public void TimingUIHidden()
    {
        TimingCanvas.SetActive(false);
    }
    /// <summary>
    /// タイミング良く押せたか
    /// </summary>
    /// <returns></returns>
    public bool TimingAttack()
    {
        if (TimingFlg)
        {
            if ((Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Return)) && !TimingPush)
            {
                TimingPush = true;
                if (CurrentTime > 0.35f && CurrentTime < 0.50f)
                {
                    TimingIcon.color = new Color(0, 100, 100, 1);
                    return true;
                }
                else
                {
                    TimingIcon.color = new Color(100, 0, 0, 1);
                    return false;
                }
            }
            CurrentTime += Time.deltaTime;
            CurrentSize -= Time.deltaTime * 0.7f;
            TimingCircle.transform.localScale = new Vector3(CurrentSize, CurrentSize, 1);
            if (CurrentSize < 0.1f)
            {
                TimingFlg = false;
                TimingPush = false;
                TimingCanvas.SetActive(false);
                return false;
            }
        }
        return false;
    }
}
