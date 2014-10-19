using UnityEngine;
using System.Collections;

public class CollisionCheckerDown : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D obj){
		if(obj.tag == "Platform" && transform.parent.gameObject.GetComponent<WallSkillModule>() != null){
			transform.parent.gameObject.GetComponent<WallSkillModule>().groundColliding = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Platform" && transform.parent.gameObject.GetComponent<WallSkillModule>() != null){
			transform.parent.gameObject.GetComponent<WallSkillModule>().groundColliding = false;
			++transform.parent.gameObject.GetComponent<JumpSkillModule>().airJumpCount;
		}
	}
}
