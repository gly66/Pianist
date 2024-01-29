using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLogic : MonoBehaviour
{
    public PianoKeyController PianoKeyDetector;
    List<KeyCode> keyList = new List<KeyCode>();

    private int currentPianoKeyGroup = 5;

    private int minGroup = 1;

    private int maxGroup = 9;

    bool IsUp = true;


    public float velocity = 127f;


    public float duration = 0.3f;

    private readonly string[] _keyIndex = new string[10] { "C", "D", "E", "F", "C#", "D#", "G", "A", "B", "F#" };

    public float offset = 0.165f;

    public float startX = 1.755f;

    public GameObject range;

    public GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
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

        range.transform.position = new Vector3( startX + offset * currentPianoKeyGroup , range.transform.position.y, range.transform.position.z);
        mainCamera.transform.position = new Vector3(startX + offset * currentPianoKeyGroup, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < keyList.Count; i++)
        {
            if (Input.GetKeyDown(keyList[i]))
            {
                /*fingerList[i].localEulerAngles = new Vector3(fingerList[i].localEulerAngles.x, fingerAngleList[i], fingerList[i].localEulerAngles.z);*/

                string note = _keyIndex[i] + currentPianoKeyGroup;

                if (!PianoKeyDetector.PianoNotes.ContainsKey(note))
                    continue;

                Debug.Log(note + PianoKeyDetector.PianoNotes[note].gameObject.name);

                PianoKeyDetector.PianoNotes[note].Play(Color.black,velocity, duration, PianoKeyDetector.MidiPlayer.GlobalSpeed);


                float pianoX = PianoKeyDetector.PianoNotes[note].gameObject.transform.position.x;

                Vector3 temp = Vector3.zero;
                temp = transform.localPosition;
                temp.x = pianoX;
                transform.localPosition = temp;

                if (IsUp) 
                { 
                    Quaternion q = Quaternion.AngleAxis(20f, Vector3.right);
                    transform.localRotation = transform.localRotation * q;
                    IsUp = false;   
                }

                

            }
            if (Input.GetKeyUp(keyList[i]))
            {
                if (!IsUp)
                {
                    Quaternion q = Quaternion.AngleAxis(-20f, Vector3.right);
                    transform.localRotation = transform.localRotation * q;
                    IsUp = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentPianoKeyGroup--;
            if (currentPianoKeyGroup < minGroup)
            {
                currentPianoKeyGroup = minGroup;

            }

            range.transform.position = new Vector3(startX + offset * currentPianoKeyGroup, range.transform.position.y, range.transform.position.z);
            mainCamera.transform.position = new Vector3(startX + offset * currentPianoKeyGroup, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentPianoKeyGroup++;
            if (currentPianoKeyGroup > maxGroup)
            {
                currentPianoKeyGroup = maxGroup;
            }

            range.transform.position = new Vector3(startX + offset * currentPianoKeyGroup, range.transform.position.y, range.transform.position.z);
            mainCamera.transform.position = new Vector3(startX + offset * currentPianoKeyGroup, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
    }
}
