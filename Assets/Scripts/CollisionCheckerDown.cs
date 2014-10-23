using UnityEngine;
using System.Collections;

public class CollisionCheckerDown : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D obj){
		if(obj.tag == "Platform" && transform.parent.gameObject.GetComponent<WallSkillModule>() != null){
			transform.parent.gameObject.GetComponent<WallSkillModule>().groundColliding = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Platform" && transform.parent != null && transform.parent.gameObject.GetComponent<WallSkillModule>() != null){
			transform.parent.gameObject.GetComponent<WallSkillModule>().groundColliding = false;
			JumpSkillModule jump = transform.parent.gameObject.GetComponent<JumpSkillModule>();
			++jump.airJumpCount;
			if(jump.dashJumping) ++transform.parent.gameObject.GetComponent<DashSkillModule>().airDashCount;
		}
	}
}
