using UnityEngine;
using System.Collections;

public class SpiderCollisionChecker : MonoBehaviour {

	public GameObject aranha;
	Aranha script;

	void Start(){
		script = aranha.GetComponent<Aranha>();
	}

	void OnTriggerEnter2D(Collider2D obj){
		if(obj.tag == "Platform"){
			script.platformColliding = true;
		}
	}

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Platform"){
			script.platformColliding = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Platform"){
			script.platformColliding = false;
		}
	}
}
