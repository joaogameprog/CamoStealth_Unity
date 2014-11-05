//######################################################################################################################
// SpiderEars
// * Controla a audiçao da aranha
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class SpiderEars : MonoBehaviour {
	// Referencias
	Aranha aranha;

	//------------------------------------------------------------------------------------------------------------------
	// Seta os valores inciais
	//------------------------------------------------------------------------------------------------------------------
	void Start(){
		aranha = transform.parent.GetComponent<Aranha>();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Detecçao do personagem - Entrar
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){ // Detecçao do personagem
			aranha.hearing = Mathf.Pow(Player.player.GetComponent<Rigidbody2D>().velocity.magnitude/10, aranha.hearingAmplifier)/100.0f ;
			if(aranha.hearing > 0) aranha.lastPlayerHeardPosition = Player.player.transform.position;
		}
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Detecçao do personagem - Sair
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){ // Detecçao do personagem
			aranha.hearing = 0;
		}
	}
}
