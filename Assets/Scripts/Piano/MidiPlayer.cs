using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// midi player
/// </summary>
public class MidiPlayer : MonoBehaviour
{
	[Header("References")]
	public PianoKeyController PianoKeyDetector;

	[Header("Properties")]
	public float GlobalSpeed = 1;
	public RepeatType RepeatType;

	public KeyMode KeyMode;
	public bool ShowMIDIChannelColours;
	public Color[] MIDIChannelColours;

	[Header("Ensure Song Name is filled for builds")]
	public MidiSong[] MIDISongs;

	[HideInInspector]
	public MidiNote[] MidiNotes;
	public UnityEvent OnPlayTrack { get; set; }

	MidiFileInspector _midi;

	string _path;
	string[] _keyIndex;
	int _noteIndex = 0;
	int _midiIndex;
	float _timer = 0;
	[SerializeField, HideInInspector]
	bool _preset = false;

    public int MidiIndex {
        get {
            return _midiIndex;
        }
        set {
            _midiIndex = value;
        }
    }

	void Start ()
	{
		OnPlayTrack = new UnityEvent();
	}

	void Update ()
	{
		if (MIDISongs.Length <= 0)
			enabled = false;
		
		if (_midi != null && MidiNotes.Length > 0 && _noteIndex < MidiNotes.Length)
		{
			_timer += Time.deltaTime * GlobalSpeed * (float)MidiNotes[_noteIndex].Tempo;

			while (_noteIndex < MidiNotes.Length && MidiNotes[_noteIndex].StartTime < _timer)
			{
				if (PianoKeyDetector.PianoNotes.ContainsKey(MidiNotes[_noteIndex].Note))
				{
					Debug.LogFormat("{0} {1} {2}",MidiNotes[_noteIndex].Note, MidiNotes[_noteIndex].Velocity, MidiNotes[_noteIndex].Length);
					if (ShowMIDIChannelColours)
					{
						PianoKeyDetector.PianoNotes[MidiNotes[_noteIndex].Note].Mark(MIDIChannelColours[MidiNotes[_noteIndex].Channel],
																				MidiNotes[_noteIndex].Velocity, 
																				MidiNotes[_noteIndex].Length, 
																				PianoKeyDetector.MidiPlayer.GlobalSpeed * MIDISongs[_midiIndex].Speed);
					}
					// else
					// 	PianoKeyDetector.PianoNotes[MidiNotes[_noteIndex].Note].Play(MidiNotes[_noteIndex].Velocity, 
					// 															MidiNotes[_noteIndex].Length, 
					// 															PianoKeyDetector.MidiPlayer.GlobalSpeed * MIDISongs[_midiIndex].Speed);
				}

				_noteIndex++;
			}
		}
	}

	void SetupNextMIDI()
	{
		if (_midiIndex >= MIDISongs.Length - 1)
		{
			if (RepeatType != RepeatType.NoRepeat)
				_midiIndex = 0;
			else
			{
				_midi = null;
				return;
			}
		}
		else
		{
			if (RepeatType != RepeatType.RepeatOne)
				_midiIndex++;
		}

		PlayCurrentMIDI();
	}

    /// <summary>

    /// </summary>
	public void PlayCurrentMIDI()
	{
		_timer = 0;

		_path = string.Format("{0}/MIDI/{1}.mid", Application.streamingAssetsPath, MIDISongs[_midiIndex].MIDIFile.name);

		//_path = string.Format("{0}/MIDI/{1}.mid", Application.streamingAssetsPath, MIDISongs[_midiIndex].SongFileName);

		_midi = new MidiFileInspector(_path);
		MidiNotes = _midi.GetNotes();
		_noteIndex = 0;

		OnPlayTrack.Invoke();
	}






}

public enum RepeatType { NoRepeat, RepeatLoop, RepeatOne }
public enum KeyMode { Physical, ForShow }

[Serializable]
public class MidiSong
{

	public UnityEngine.Object MIDIFile;
	public string SongFileName;
	public float Speed = 1;
	[TextArea]
	public string Details;
}