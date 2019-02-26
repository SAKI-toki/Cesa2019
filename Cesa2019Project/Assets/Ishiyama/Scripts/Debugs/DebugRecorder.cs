using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugRecorder : MonoBehaviour
{
    [SerializeField]
    Text RimText = null;
    [SerializeField]
    Renderer[] RimRenderer = null;
    float RimValue = 0;
    void Start()
    {
        UpdateRim();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateRim();
        }
    }

    void UpdateRim()
    {
        RimValue += 0.5f;
        foreach (var ren in RimRenderer)
        {
            ren.material.SetFloat("AddRim", RimValue);
        }
        RimText.text = "RimValue = " + RimValue;

    }
}
