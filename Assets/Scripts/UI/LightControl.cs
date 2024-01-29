using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightControl : MonoBehaviour
{
    public Toggle Toggle1 = null;
    public Toggle Toggle2 = null;
    public LoadLight LoadLight = null;
    // Start is called before the first frame update
    void Start()
    {
        Toggle1.onValueChanged.AddListener(ChangePointLight);
        Toggle2.onValueChanged.AddListener(ChangeDirLight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangePointLight(bool t)
    {
        LoadLight.PtLightOn = t;
    }
    void ChangeDirLight(bool t)
    {
        LoadLight.DirLightOn = t;
    }
}
