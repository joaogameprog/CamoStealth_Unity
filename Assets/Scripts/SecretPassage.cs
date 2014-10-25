using UnityEngine;
using System.Collections;

public class SecretPassage : MonoBehaviour {

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			renderer.material.color = new Color(1,1,1,0.5f);
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			renderer.material.color = new Color(1,1,1,1);
		}
	}
}
