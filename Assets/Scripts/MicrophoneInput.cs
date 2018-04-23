using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic; // So we can use List<>

// With reference to MyLittleUnity3D's tutorial
// https://github.com/patrickhimes/microphone-demo/blob/master/Assets/Scripts
// To do add sensitivity adjustment


[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour {
	public float minThreshold = 0;
	public float myFrequency = 0.0f;
	public int audioSampleRate = 44100;
	public string microphone;
	public FFTWindow fftWindow;

	private List<string> options = new List<string>();
	private int samples = 8192; 
	private AudioSource audioSource;

	public Queue<string> noteBuffer = new Queue<string>();
	public string prominentNote = "";

	void Start() {
		
		//get components you'll need
		audioSource = GetComponent<AudioSource> ();

		// get all available microphones
		foreach (string device in Microphone.devices) {
			if (microphone == null) {
				//set default mic to first mic found.
				microphone = device;
			}
			options.Add(device);
		}
		microphone = options[PlayerPrefsManager.GetMicrophone ()];
		minThreshold = PlayerPrefsManager.GetThreshold ();


		//initialize input with default mic
		UpdateMicrophone ();
	}

	void Update()
	{
		float volume = GetAveragedVolume();

		ProcessBuffer();


		//  minThreshold already exists, but I also want to have silence in the buffer
		// Probably not the best way to do it
		if (volume > 0.005f)
		{
			string note = FrequencyToPitch.FTP(GetFundamentalFrequency());
			//platformSpawner.GetComponent<SpawnPlatform>().SpawnBlockFromNote(note);

			// Add note to the buffer
			noteBuffer.Enqueue(note);
		}
		else
		{
			noteBuffer.Enqueue("");
		}

		// dequeue to keep noteBuffer at count 12
		while (noteBuffer.Count > 12)
		{
			noteBuffer.Dequeue();
		}
	}

	// Check buffer of notes heard and find the most prominent (mode)
	// This removes noise a.k.a stray notes
	void ProcessBuffer()
	{
		List<string> tempBuffer = noteBuffer.ToList();
		prominentNote = tempBuffer.GroupBy(n=> n).OrderByDescending(g=> g.Count()).Select(g => g.Key).FirstOrDefault(); 
		
	}


	void UpdateMicrophone(){
		audioSource.Stop(); 
		//Start recording to audioclip from the mic
		audioSource.clip = Microphone.Start(microphone, true, 10, audioSampleRate);
		audioSource.loop = true; 
		// Mute the sound with an Audio Mixer group becuase we don't want the player to hear it
		Debug.Log(Microphone.IsRecording(microphone).ToString());

		if (Microphone.IsRecording (microphone)) { //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
			while (!(Microphone.GetPosition (microphone) > 0)) {
			} // Wait until the recording has started. 
		
			Debug.Log ("recording started with " + microphone);
			// Start playing the audio source
			audioSource.Play (); 
		} else {
			//microphone doesn't work for some reason

			Debug.Log (microphone + " doesn't work!");
			GameObject.FindGameObjectWithTag("Warning").SetActive(true);
		}
	}

	
	public float GetAveragedVolume()
	{ 
		float[] data = new float[256];
		float a = 0;
		audioSource.GetOutputData(data,0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/256;
	}
	
	public float GetFundamentalFrequency()
	{
		float fundamentalFrequency = 0.0f;
		float[] data = new float[samples];
		audioSource.GetSpectrumData(data,0,fftWindow);
		float s = 0.0f;
		int i = 0;
		for (int j = 1; j < samples; j++)
		{
			if(data[j] > minThreshold) // volume must meet min threshold
			{
				if ( s < data[j] )
				{
					s = data[j];
					i = j;
				}
			}
		}
		fundamentalFrequency = i * audioSampleRate / samples;
		myFrequency = fundamentalFrequency;
		// print (myFrequency);
		return fundamentalFrequency;
	}
}