using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acusutim : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer EnemySMR = null;
    [SerializeField]
    float Alpha = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Color col = EnemySMR.material.color;
        col.a = Alpha;
        EnemySMR.material.color = col;
    }
}
