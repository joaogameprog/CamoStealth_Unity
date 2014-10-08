using UnityEngine;
using System.Collections;

public class HideSpot : MonoBehaviour {

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			obj.GetComponent<Mover>().trulyHiding = obj.GetComponent<Mover>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Hiding");
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			obj.GetComponent<Mover>().trulyHiding = false;
		}
	}
}
