using UnityEngine;
using System.Collections;

public class SecretPassage : MonoBehaviour {

	public static bool playerInside = false;

	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			playerInside = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			playerInside = false;
		}
	}

	void Update(){
		if(renderer.material.shader.name == "Diffuse" && renderer.material.color.a < 1)
			renderer.material.shader = Shader.Find("Transparent/Diffuse");
		if(renderer.material.shader.name == "Transparent/Diffuse" && renderer.material.color.a >= 1)
			renderer.material.shader = Shader.Find("Diffuse");

		if(playerInside)
			if(renderer.material.color.a > 0.5f)
				renderer.material.color -= new Color(0,0,0,0.05f);
			else return;
		else
			if(renderer.material.color.a < 1.0f)
				renderer.material.color += new Color(0,0,0,0.05f);
			else return;
	}
}
