using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField]
    AudioSource Audios = null;
    [SerializeField]
    AudioClip[] Audio = null;


    /// <summary>
    /// 戦闘中のBGMを流す
    /// </summary>
    void Start()
    {
        Audios.clip = Audio[0];
        Audios.Play();
    }

    /// <summary>
    /// ボス戦のBGMをならす
    /// </summary>
    public void Play()
    {
        Audios.clip = Audio[1];
        Audios.Play();
    }

    /// <summary>
    /// リザルトのBGMをならす
    /// </summary>
    public void Result()
    {
        Audios.clip = Audio[2];
        Audios.Play();
        Audios.loop = false;
    }
}
