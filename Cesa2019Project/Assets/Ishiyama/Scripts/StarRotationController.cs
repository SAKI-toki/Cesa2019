using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotationController : MonoBehaviour
{
    void Start()
    {
        transform.eulerAngles = new Vector3(-90, 0, 0);
    }
}
