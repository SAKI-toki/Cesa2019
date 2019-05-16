using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [System.NonSerialized]
    public Animator PlayerAnimator;
    [System.NonSerialized]
    public AnimatorStateInfo PlayerAnimatorStateInfo;
    [System.NonSerialized]
    public float AnimationTime;

    private void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
        AnimationTime = 0;
    }

    void Update()
    {
        PlayerAnimatorStateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
    }
}
