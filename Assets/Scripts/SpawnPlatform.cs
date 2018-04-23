using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour {

	public int platformSize = 2;

	public GameObject platformPrefab;
	public string lastNote = "";
	public MicrophoneInput microphone;

	[SerializeField]
	Color colorC;
	[SerializeField]
	Color colorCSharp;
	[SerializeField]
	Color colorD;
	[SerializeField]
	Color colorDSharp;
	[SerializeField]
	Color colorE;
	[SerializeField]
	Color colorF;
	[SerializeField]
	Color colorFSharp;
	[SerializeField]
	Color colorG;
	[SerializeField]
	Color colorGSharp;
	[SerializeField]
	Color colorA;
	[SerializeField]
	Color colorASharp;
	[SerializeField]
	Color colorB;
	

	public GameObject currentPlatform;

	// Function that spawns the platform blocks
	public void SpawnBlockFromNote (string note) {
		float block_x = 0;
		Color block_color;
		switch (note)
		{
			case "C":
				block_x -= platformSize / 2f * 11;
				block_color = colorC;
				break;
			case "C#":
				block_x -= platformSize / 2f * 9;
				block_color = colorCSharp;
				break;
			case "D":
				block_x -= platformSize / 2f * 7;
				block_color = colorD;
				break;
			case "D#":
				block_x -= platformSize / 2f * 5;
				block_color = colorDSharp;
				break;
			case "E":
				block_x -= platformSize / 2f * 3;
				block_color = colorE;
				break;
			case "F":
				block_x -= platformSize / 2f;
				block_color = colorF;
				break;
			case "F#":
				block_x += platformSize / 2f;
				block_color = colorFSharp;
				break;
			case "G":
				block_x += platformSize / 2f * 3;
				block_color = colorG;
				break;
			case "G#":
				block_x += platformSize / 2f * 5;
				block_color = colorGSharp;
				break;
			case "A":
				block_x += platformSize / 2f * 7;
				block_color = colorA;
				break;
			case "A#":
				block_x += platformSize / 2f * 9;
				block_color = colorASharp;
				break;
			case "B#":
				block_x += platformSize / 2f * 11;
				block_color = colorB;
				break;
			default:
				return;
			
		}

		currentPlatform = Instantiate(platformPrefab, new Vector2(block_x, transform.position.y), new Quaternion(0,0,0,0));
		currentPlatform.GetComponentInChildren<SpriteRenderer>().color = block_color;
	}
	
	void Update()
	{
		string currentNote = microphone.prominentNote;
		
		if (currentNote != lastNote)
		{
			SpawnBlockFromNote(currentNote);
		}
		else if (currentNote != "")
		{
			currentPlatform.GetComponentInChildren<PlatformMovement>().isCurrentNote = 1f;
		}

		lastNote = currentNote;
	}

}
