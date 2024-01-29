using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLight : MonoBehaviour {
    public Transform LightPosition;
    public Color LightColor = Color.white;
    public Transform LightDir;
    public Color DirColor = Color.white;
    public bool PtLightOn = true;
    public bool DirLightOn = true;
    void Update()
    {
        Shader.SetGlobalVector("LightPosition", LightPosition.localPosition);
        if(PtLightOn)
            Shader.SetGlobalColor("LightColor", LightColor);
        else
            Shader.SetGlobalColor("LightColor", Color.black);
        Shader.SetGlobalVector("DirectionalLight", LightDir.up);
        if(DirLightOn)
            Shader.SetGlobalColor("DirColor", DirColor);
        else
            Shader.SetGlobalColor("DirColor", Color.black);
    }
}
