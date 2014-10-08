using UnityEngine;
using System.Collections;

public class SpiderVision : MonoBehaviour {

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			transform.parent.gameObject.GetComponent<Aranha>().playerDetected = !obj.GetComponent<Mover>().trulyHiding;
			if(!obj.GetComponent<Mover>().trulyHiding){
				obj.GetComponent<Mover>().death = true;	
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			//transform.parent.gameObject.GetComponent<Aranha>().playerDetected = false;
		}
	}
}
