using UnityEngine;
using System.Collections;

public class CollisionCheck : MonoBehaviour {

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Platform" && transform.parent.gameObject.GetComponent<WallSkillModule>() != null){
			transform.parent.gameObject.GetComponent<WallSkillModule>().platformColliding = true;
		}
	}

	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Platform" && transform.parent.gameObject.GetComponent<WallSkillModule>() != null){
			transform.parent.gameObject.GetComponent<WallSkillModule>().platformColliding = false;
		}
	}
}
