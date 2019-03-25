﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySe : MonoBehaviour
{
    public AudioClip AttackSE = null;
    public AudioClip DamageSE = null;
    public AudioClip DestroySE = null;

    AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackSES()
    {
        AudioSource.PlayOneShot(AttackSE);
    }

    public void DamageSES()
    {
        AudioSource.PlayOneShot(DamageSE);
    }

    public void DestroySES()
    {
        AudioSource.PlayOneShot(DestroySE);
    }
}
