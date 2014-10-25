using UnityEngine;
using System.Collections;

public class HideSpot : MonoBehaviour {

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			obj.GetComponent<HideSkillModule>().trulyHiding = obj.GetComponent<Player>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Hiding");
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			obj.GetComponent<HideSkillModule>().trulyHiding = false;
		}
	}
}
