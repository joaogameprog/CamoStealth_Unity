using UnityEngine;
using System.Collections;

public class SpiderVision : MonoBehaviour {

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			transform.parent.gameObject.GetComponent<Aranha>().playerDetected = !obj.GetComponent<HideSkillModule>().trulyHiding;
			if(!obj.GetComponent<HideSkillModule>().trulyHiding){
				transform.parent.gameObject.GetComponent<Aranha>().playerDetected = true;	
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			transform.parent.gameObject.GetComponent<Aranha>().playerDetected = false;
		}
	}
}
