using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSE : MonoBehaviour
{
    //選択音
    [SerializeField]
    AudioClip Select = null;
    //決定音
    [SerializeField]
    AudioClip Decision = null;
    //星設置音
    [SerializeField]
    AudioClip StarSE = null;
    AudioSource SE;

    private void Start()
    {
        SE = GetComponent<AudioSource>();
    }
    public void Sel()
    {
        SE.PlayOneShot(Select);
    }

    public void Dec()
    {
        SE.PlayOneShot(Decision);
    }

    public void Star()
    {
        SE.PlayOneShot(StarSE);
    }
}
