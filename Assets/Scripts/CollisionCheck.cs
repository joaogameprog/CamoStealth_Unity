using UnityEngine;
using System.Collections;

public class CollisionCheck : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D obj){
		if(obj.tag == "Platform"){
			transform.parent.gameObject.GetComponent<Mover>().platformColliding = true;
		}
	}

	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Platform"){
			transform.parent.gameObject.GetComponent<Mover>().platformColliding = false;
		}
	}
}
