using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoMenu : MonoBehaviour
{

    public List<Toggle> handTextureToggleList = new List<Toggle>();

    //toggleGroup
    public ToggleGroup handTextureToggleGroup;


    public List<Material> handTextureList = new List<Material>();


    public SkinnedMeshRenderer targetMeshRenderer;



    public List<Toggle> musicToggleList = new List<Toggle>();

    //toggleGroup
    public ToggleGroup musicToggleGroup;

    //midi player
    public MidiPlayer midiPlayer;

    

    public Light light1;


    private float light1Intensity;


    public Light light2;


    private float light2Intensity;


    public Toggle light1Toggle;


    public Toggle light2Toggle;


    public Slider light1Slider;


    public Slider light2Slider;

    private void Start() {

        BindHandToggleGroup();

        BindMusicToggleGroup();

        InitLight();
    }



    void BindHandToggleGroup()
    {

        handTextureToggleGroup.allowSwitchOff = true;

        for(int i=0;i<handTextureToggleList.Count;i++)
        {
            var item = handTextureToggleList[i];
            int index = i;
            handTextureToggleList[i].onValueChanged.AddListener((bool value) => {
                if (value)
                {
                    targetMeshRenderer.material = handTextureList[index];
                }
            });
            item.group = handTextureToggleGroup;
        }
    }


    void BindMusicToggleGroup()
    {

        musicToggleGroup.allowSwitchOff = true;

        for(int i=0;i<musicToggleList.Count;i++)
        {
            var item = musicToggleList[i];
            int index = i;
            musicToggleList[i].onValueChanged.AddListener((bool value) => {
                if (value)
                {
                    midiPlayer.MidiIndex = index;
                    midiPlayer.PlayCurrentMIDI();
                }
            });
            item.group = musicToggleGroup;
        }
        {

            
        }
    }


    void InitLight()
    {
        light1Intensity = light1.intensity;
        light2Intensity = light2.intensity;

        //toggle
        light1Toggle.onValueChanged.AddListener((bool value) => {
            if (value)
            {
                light1.intensity = light1Intensity;
            }
            else
            {
                light1.intensity = 0;
            }
        });

        light2Toggle.onValueChanged.AddListener((bool value) => {
            if (value)
            {
                light2.intensity = light2Intensity;
            }
            else
            {
                light2.intensity = 0;
            }
        });

        //slider
        light1Slider.onValueChanged.AddListener((float value) => {
            light1.intensity = light1Intensity*2 * value;
        });

        light2Slider.onValueChanged.AddListener((float value) => {
            light2.intensity = light2Intensity*2 * value;
        });
    }
}