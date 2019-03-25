using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] Audios;

    // Start is called before the first frame update
    void Start()
    {
        Audios[0].Play();
    }

    private void Update()
    {

    }

    public void Play()
    {
        Audios[0].Stop();//停止
        Audios[1].Play();//再生
    }
}
