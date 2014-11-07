using UnityEngine;
using System.Collections;

public class SetVolume : MonoBehaviour {

	// Use this for initialization
	void Start () {
		audio.volume = ApplicationModel.SaveData.Volume;
	}
}
