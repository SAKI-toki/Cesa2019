using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [System.NonSerialized]
    public AudioSource PlayerAudioSource;
    [SerializeField]
    public AudioClip JumpAudio;
    [SerializeField]
    public AudioClip DamegAudio;
    [SerializeField]
    public AudioClip Attack1Audio;
    [SerializeField]
    public AudioClip Attack2Audio;
    [SerializeField]
    public AudioClip Attack3Audio;
    [SerializeField]
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
}
