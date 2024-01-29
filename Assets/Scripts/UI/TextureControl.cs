using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TextureControl : MonoBehaviour
{
    public int FLAG = 0;
    public Vector2 Offset = Vector2.zero;
    public Vector2 Scale = Vector2.one;

    public UnityEngine.UI.Toggle Toggle1 = null;
    public UnityEngine.UI.Toggle Toggle2 = null;

    public UnityEngine.UI.Slider Slider1 = null;
    public UnityEngine.UI.Slider Slider2 = null;
    public UnityEngine.UI.Slider Slider3 = null;
    public UnityEngine.UI.Slider Slider4 = null;
    // Start is called before the first frame update
    void Start()
    {
        Toggle1.onValueChanged.AddListener(Texture1);
        Toggle2.onValueChanged.AddListener(Texture2);
        Slider1.onValueChanged.AddListener(OffsetX);
        Slider2.onValueChanged.AddListener(OffsetY);
        Slider3.onValueChanged.AddListener(ScaleX);
        Slider4.onValueChanged.AddListener(ScaleY);
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalInt("FLAG", FLAG);
        Shader.SetGlobalFloat("MyTexOffset_X", Offset.x);
        Shader.SetGlobalFloat("MyTexOffset_Y", Offset.y);

        Shader.SetGlobalFloat("MyTexScale_X", Scale.x);
        Shader.SetGlobalFloat("MyTexScale_Y", Scale.y);
    }

    void Texture1(bool t)
    {
        if(t)
        {
            FLAG = 0;
        }
    }
    void Texture2(bool t)
    {
        if (t)
        {
            FLAG = 1;
        }
    }

    void OffsetX(float a)
    {
        Offset.x = a;
    }
    void OffsetY(float a)
    {
        Offset.y = a;
    }

    void ScaleX(float a)
    {
        Scale.x = a;
    }
    void ScaleY(float a)
    {
        Scale.y = a;
    }
}
