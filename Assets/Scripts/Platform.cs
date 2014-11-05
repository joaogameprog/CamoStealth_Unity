//######################################################################################################################
// Platform
// * Controla as plataformas
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D obj){
		if(obj.tag == "Bullet"){
			Destroy(obj.gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Bullet"){
			Destroy(obj.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Bullet"){
			Destroy(obj.gameObject);
		}
	}
}
