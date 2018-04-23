using UnityEngine;

public class FrequencyToPitch : MonoBehaviour {

	public static string FTP (float frequency)
	{
		float pitch = 69 + 12 * Mathf.Log(frequency / 440, 2);
		int generalisedPitch = Mathf.RoundToInt(pitch) % 12;

		switch (generalisedPitch)
		{
			case 0:
				return "C";
			case 1:
				return "C#";
			case 2:
				return "D";
			case 3:
				return "D#";
			case 4:
				return "E";
			case 5:
				return "F";
			case 6:
				return "F#";
			case 7:
				return "G";
			case 8:
				return "G#";
			case 9:
				return "A";
			case 10:
				return "A#";
			case 11:
				return "B#";
			default:
				return "";
			
		}
	}
	
}
