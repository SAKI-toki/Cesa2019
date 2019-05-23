using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSE : MonoBehaviour
{
    //選択音
    [SerializeField, Header("選択音")]
    public AudioClip Select = null;
    //決定音
    [SerializeField, Header("決定音")]
    public AudioClip Decision = null;
    //星設置音
    [SerializeField, Header("星の設置音")]
    public AudioClip StarSE = null;

    AudioSource SE;

    private void Start()
    {
        SE = GetComponent<AudioSource>();
    }

    /// <summary>
    /// メニューの選択音
    /// </summary>
    public void Sel()
    {
        SE.PlayOneShot(Select);
    }

    /// <summary>
    /// メニュー決定音
    /// </summary>
    public void Dec()
    {
        SE.PlayOneShot(Decision);
    }

    /// <summary>
    /// 星設置音
    /// </summary>
    public void Star()
    {
        SE.PlayOneShot(StarSE);
    }
}
