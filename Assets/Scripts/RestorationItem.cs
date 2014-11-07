using UnityEngine;
using System.Collections;

public class RestorationItem : MonoBehaviour {

	public AudioClip restoreAudio;
	public bool used = false;

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D obj){
		if(used) return;
		if(obj.tag == "Player"){
			Player.player.dmg.Health = Player.player.dmg.maxHealth;
			if(audio.clip != restoreAudio) audio.clip = restoreAudio;
			audio.pitch = 2.0f;
			audio.volume = ApplicationModel.SaveData.Volume;
			audio.Play();
			this.GetComponent<SpriteRenderer>().color = Color.clear;
			used = true;
			Destroy(gameObject,0.7f);
		}
	}
}
