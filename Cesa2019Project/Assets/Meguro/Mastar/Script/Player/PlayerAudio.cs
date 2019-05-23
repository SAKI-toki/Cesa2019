using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioSource PlayerAudioSource;
    [Header("ジャンプ")]
    public AudioClip JumpAudio;
    [Header("ダメージ")]
    public AudioClip DamegAudio;
    [Header("アタック１")]
    public AudioClip Attack1Audio;
    [Header("アタック２")]
    public AudioClip Attack2Audio;
    [Header("アタック３")]
    public AudioClip Attack3Audio;
    [Header("ダッシュ")]
    public AudioClip DashAudio;

    void Awake()
    {
        PlayerAudioSource = GetComponent<AudioSource>();
    }

    public void AudioPlay(AudioClip audioClip)
    {
        PlayerAudioSource.clip = audioClip;
        PlayerAudioSource.Play();
    }

    public void AudioPauseStart()
    {
        PlayerAudioSource.Play();
    }
    public void AudioPause()
    {
        PlayerAudioSource.Pause();
    }

    public void AudioLoop(bool flg)
    {
        PlayerAudioSource.loop = flg;
    }
}
