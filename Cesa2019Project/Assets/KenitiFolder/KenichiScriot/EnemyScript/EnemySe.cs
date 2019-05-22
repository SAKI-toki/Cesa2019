using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySe : MonoBehaviour
{
    [SerializeField]
    AudioClip AttackSE = null;
    [SerializeField]
    AudioClip DamageSE = null;
    [SerializeField]
    AudioClip WalkSE = null;

    [HideInInspector]
    public AudioSource AudioSource;

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

    public void WalkSES()
    {
        AudioSource.clip = WalkSE;
        AudioSource.Play();
    }
}
