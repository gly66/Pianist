using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;


public class PianoPlayer : MonoBehaviour {
    
    [Header("References")]
	public PianoKeyController PianoKeyDetector;

    public List<Transform> fingerList = new List<Transform>();


    public List<float> fingerOffsetXList = new List<float>();


    private List<float> fingerAngleList = new List<float>();

    public List<Transform> elbowList = new List<Transform>();


    List<KeyCode> keyList = new List<KeyCode>();


    public int currentPianoKeyGroup = 5;


    public float velocity = 127f;


    public float duration = 0.3f;


    public float handDistance = 0.3f;

    private readonly string[] _keyIndex = new string[10] { "C", "D", "E", "F","C#", "D#","G", "A", "B" , "F#"};

    void Start() {

        for (int i = 0; i < fingerList.Count; i++)
        {
            fingerAngleList.Add(fingerList[i].localEulerAngles.y);
        }


        for (int i = 0; i < fingerList.Count; i++)
        {
            if( i != 4 && i != 5 )
                fingerList[i].localEulerAngles = new Vector3(fingerList[i].localEulerAngles.x, 0, fingerList[i].localEulerAngles.z);
            else 
                fingerList[i].localEulerAngles = new Vector3(fingerList[i].localEulerAngles.x, 80, fingerList[i].localEulerAngles.z);
        }


        //q w e r v b u i o p
        keyList.Add(KeyCode.Q);
        keyList.Add(KeyCode.W);
        keyList.Add(KeyCode.E);
        keyList.Add(KeyCode.R);
        keyList.Add(KeyCode.V);
        keyList.Add(KeyCode.B);
        keyList.Add(KeyCode.U);
        keyList.Add(KeyCode.I);
        keyList.Add(KeyCode.O);
        keyList.Add(KeyCode.P);
        

    }

    void Update()
    {

        for (int i = 0; i < keyList.Count; i++)
        {
            if (Input.GetKeyDown(keyList[i]))
            {
                fingerList[i].localEulerAngles = new Vector3(fingerList[i].localEulerAngles.x, fingerAngleList[i], fingerList[i].localEulerAngles.z);

                string note = _keyIndex[i] + currentPianoKeyGroup;

                PianoKeyDetector.PianoNotes[note].Play(velocity, duration, PianoKeyDetector.MidiPlayer.GlobalSpeed);

                float pianoX = PianoKeyDetector.PianoNotes[note].gameObject.transform.position.x;

                if (i < 5)
                {
                    elbowList[0].position = new Vector3(pianoX + fingerOffsetXList[i], elbowList[0].position.y, elbowList[0].position.z);

                    elbowList[1].position = new Vector3(pianoX + fingerOffsetXList[i] + handDistance, elbowList[1].position.y, elbowList[1].position.z);
                }
                else
                {
                    elbowList[1].position = new Vector3(pianoX + fingerOffsetXList[i], elbowList[1].position.y, elbowList[1].position.z);


                    elbowList[0].position = new Vector3(pianoX + fingerOffsetXList[i] - handDistance, elbowList[1].position.y, elbowList[1].position.z);
                }

            }
            if (Input.GetKeyUp(keyList[i]))
            {
                if (i != 4 && i != 5)
                    fingerList[i].localEulerAngles = new Vector3(fingerList[i].localEulerAngles.x, 0, fingerList[i].localEulerAngles.z);
                else
                    fingerList[i].localEulerAngles = new Vector3(fingerList[i].localEulerAngles.x, 80, fingerList[i].localEulerAngles.z);
            }
        }
    }

} 