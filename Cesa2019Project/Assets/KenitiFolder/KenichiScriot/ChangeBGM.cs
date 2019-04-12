using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    GameObject Wave = null;
    BGM BGM = null;
    // Start is called before the first frame update
    void Start()
    {
        Wave = GameObject.Find("WaveGenerator");
        BGM = Wave.GetComponent<BGM>();
        BGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
