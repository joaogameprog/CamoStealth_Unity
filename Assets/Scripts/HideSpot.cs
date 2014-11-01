using UnityEngine;
using System.Collections;

public class HideSpot : MonoBehaviour {
	
	public bool playerDetectedByLocalCamera = false; // Bloqueia o estado de HIDE do jogador caso ele tenha sido detectado

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			Player.player.hide.trulyHiding = Player.player.anim.GetCurrentAnimatorStateInfo(0).IsTag("Hiding") && !playerDetectedByLocalCamera;
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			Player.player.hide.trulyHiding = false;
		}
	}
}
