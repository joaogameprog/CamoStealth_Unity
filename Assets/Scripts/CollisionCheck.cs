using UnityEngine;
using System.Collections;

public class CollisionCheck : MonoBehaviour {

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Platform" && transform.parent.gameObject.GetComponent<WallSkillModule>() != null){
			transform.parent.gameObject.GetComponent<WallSkillModule>().platformColliding = true;
		}
		if(obj.tag == "Spider" && Player.player.punch.isPunching){
			obj.GetComponent<Aranha>().die();
		}

	}

	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Platform" && transform.parent.gameObject.GetComponent<WallSkillModule>() != null){
			transform.parent.gameObject.GetComponent<WallSkillModule>().platformColliding = false;
		}
	}

	void Update(){
		if(Player.player.punch.isPunching)
			this.transform.localPosition = new Vector3(0.85f,0,0);
		else
			this.transform.localPosition = new Vector3(0.35f,0,0);
	}
}
