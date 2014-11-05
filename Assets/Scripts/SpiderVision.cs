//######################################################################################################################
// SpiderEars
// * Controla a visao da aranha
//######################################################################################################################

using UnityEngine;
using System.Collections;

public class SpiderVision : MonoBehaviour {

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			transform.parent.gameObject.GetComponent<Aranha>().playerInSight = !obj.GetComponent<HideSkillModule>().trulyHiding;
			if(!obj.GetComponent<HideSkillModule>().trulyHiding){
				transform.parent.gameObject.GetComponent<Aranha>().playerInSight = true;	
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			transform.parent.gameObject.GetComponent<Aranha>().playerInSight = false;
		}
	}

	void Start(){
		GetComponent<SpriteRenderer>().color = Color.white;
	}
}
