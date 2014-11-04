//######################################################################################################################
// Lampada
// * Script que controla as lampadas que bloqueam o personagem de esconder-se
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class Lamp : MonoBehaviour {

	SpriteRenderer sr;

	public Sprite TurnedON;
	public Sprite TurnedOFF;

	public bool isTurned = true;
	public bool isChangingState = true;

	private int pairTimeCount = 0;
	private int currentStatePair = 0;

	public lampStatePair[] stateList;

	//------------------------------------------------------------------------------------------------------------------
	// Detecçao do personagem - Entrar
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			Player.player.hide.iluminated = isTurned;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Detecçao do personagem - Sair
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){
			Player.player.hide.iluminated = false;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Inicializa os valores
	//------------------------------------------------------------------------------------------------------------------
	void Awake () {
		sr = GetComponent<SpriteRenderer>();
		setSprite();
	}

	//------------------------------------------------------------------------------------------------------------------
	// update da lampada
	//------------------------------------------------------------------------------------------------------------------
	void FixedUpdate(){
		if(isChangingState == false || stateList.Length == 0) return;
		incrementCount();
		updateState();
		setSprite();
	}

	//------------------------------------------------------------------------------------------------------------------
	// muda a sprite da lampada para acessa ou apagada
	//------------------------------------------------------------------------------------------------------------------
	private void setSprite(){
		if(isTurned && sr.sprite != TurnedON)
			sr.sprite = TurnedON;
		if(!isTurned && sr.sprite != TurnedOFF)
			sr.sprite = TurnedOFF;
	}

	//------------------------------------------------------------------------------------------------------------------
	// incrementa o contador da animaçao para transiçao de estado
	//------------------------------------------------------------------------------------------------------------------
	private void incrementCount(){
		if((pairTimeCount = ++pairTimeCount % stateList[currentStatePair].time) == 0)
			currentStatePair = ++currentStatePair % stateList.Length;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Troca o estado da lampada
	//------------------------------------------------------------------------------------------------------------------
	private void updateState(){
		switch(stateList[currentStatePair].state){
		case lampState.on:
			isTurned = true;
			break;
		case lampState.off:
			isTurned = false;
			break;
		default:
			isTurned = Time.frameCount%3==0;//!isTurned;
			break;
		}
	}
}

//Enum que controla os estado da camera
public enum lampState{
	on,
	off,
	flashing
}

// Classe que possibilita a criaçao de um array dinamico no inspector para gerenciar a transiçao de estados
[System.Serializable]
public class lampStatePair
{
	public lampState state;
	public int time;
}

