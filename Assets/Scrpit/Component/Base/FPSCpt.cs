using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCpt : MonoBehaviour {

    public int fps = 60;
    private void Awake()
    {
        Screen.SetResolution(1280, 800, false);	
        Application.targetFrameRate = fps;//此处限定60帧
    }
}
