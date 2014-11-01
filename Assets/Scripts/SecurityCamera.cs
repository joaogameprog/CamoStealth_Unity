//######################################################################################################################
// Security Camera
// * Script que controla a camera de segurança
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class SecurityCamera : MonoBehaviour {
	// Atalhos
	SpriteRenderer sr;                               // SpriteRenderer do campo de visao da camera

	// Aranhas
	public GameObject[] aranhas;                     // Aranhas que sao alertadas por esta camera

	// Esconderijos
	public GameObject[] hideSpots;                   //Hide Spots que sao desabilidatos caso a camera entre em alerta

	// Rotaçao
	public int rotationTime = 800;                   // Tempo em frames que a camera demora para rotacionar
	public int waitTime = 10;                        // Tempo em frames que a camera fica estacionada
	public float[] positions;                        // Posiçoes, definidas em graus, que a camera assume quando parada 
	public int waitCount = 0;                        // Contador de interpolaçao do tempo de rotaçao da camera
	private int currentPosition = 0;                 // Indice da posiçao em que a camera estah
	private Transform pivot;                         // posiçao do gameObject que serve como pivo de rotaçao

	// Detecçao
	public float detectionSpeed = 0.005f;            // Velocidade de detecçao da camera, quanto maior o numero, mais rapido ela detecta o personagem ao contato
	public float undetectionSpeed = 0.001f;			 // Velocidade de "des-detecçao" camera, quanto maior o numero, mais rapido ela perde o foco no personagem
	private float detectionCount = 0;	 			 // Quando este valor atinge 1.0f o personagem eh detectado
	private bool playerInSight = false;                  // Informaçao se o jogador esta dentro do campo de visao da camera
	public bool playerDetected = false;              // Informaçao se o jogador foi detectado ou nao

	// Follow
	public float followMaxSpeed = 0.3f;              // Quantidade maxima de graus que a camera move por frame para focar no personagem

	// Estado
	public SecCamState state = SecCamState.waiting; // Estado da camera

	//------------------------------------------------------------------------------------------------------------------
	// Seta os valores inciais da camera
	//------------------------------------------------------------------------------------------------------------------
	void Awake(){
		sr = GetComponent<SpriteRenderer>();
		pivot = transform.parent.parent;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Detecçao do personagem - Entrar
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){ // Detecçao do personagem
			playerInSight = !Player.player.hide.trulyHiding;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Detecçao do personagem - Sair
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == "Player"){ // Detecçao do personagem
			playerInSight = false;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Alerta as aranhas atreladas a esta camera
	//------------------------------------------------------------------------------------------------------------------
	void callSpiders(){
		foreach(GameObject aranha in aranhas){
			aranha.GetComponent<Aranha>().setState(SpiderState.searching);
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Desabilita o poder de camuflagem dos hidespots associados a esta camera
	//------------------------------------------------------------------------------------------------------------------
	void disableHidespots(){
		foreach(GameObject hidespot in hideSpots){
			hidespot.GetComponent<HideSpot>().playerDetectedByLocalCamera = true;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Atualizaçao da comera
	//------------------------------------------------------------------------------------------------------------------
	void FixedUpdate(){
		if(positions == null) return;

		switch(state){ // Comportamento normal
		case SecCamState.rotating:
			commandRotate();
			break;
		default:
			commandWait();
			break;
		}

		if(playerDetected) return;

		if(playerInSight)
			CommandDetect();
		else
			CommandUndetect();
	}

	//------------------------------------------------------------------------------------------------------------------
	// A camera fica parada no mesmo ponto esperando o tempo para trocar de posiçao
	//------------------------------------------------------------------------------------------------------------------
	void commandWait(){
		if(playerDetected || playerInSight) setState(SecCamState.rotating);
		if(++waitCount < waitTime) return;

		setState(SecCamState.rotating);
		waitCount = 0;
		if(++currentPosition >= positions.Length) currentPosition = 0;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Rotaciona a camera
	// Possui dois comportamentos diferentes:
	//      * Estado normal: caso o jogador nao tenha sido detectado ainda, a camera troca a sua rotaçao para a
	// proxima posiçao definida na lista
	//      * Estado de alerta: caso o jogador tenha sido detectado, ajusta sua rotaçao para olhar para o jogador, caso
	// o jogador esteja fora do campo de visao da camera, ela executa seu comportamento normal
	//------------------------------------------------------------------------------------------------------------------
	void commandRotate(){
		if(playerDetected && playerInSight)
			lookingAtPlayer();
		else if(changeAngle(positions[currentPosition])) 
			setState(SecCamState.waiting);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Aumenta o valor de detecçao desta camera
	//------------------------------------------------------------------------------------------------------------------
	void CommandDetect(){
		changeAngle(positions[currentPosition]);
		if((detectionCount += detectionSpeed) > 1.0f){ // Detectado
			detectionCount = 1.0f;
			callSpiders();
			disableHidespots();
			playerDetected = true;
		}else{ // Detectando
			sr.color = new Color(1,1,1-detectionCount);
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Diminue o valor de detecçao desta camera
	//------------------------------------------------------------------------------------------------------------------
	void CommandUndetect(){
		bool hasReturned = changeAngle(positions[currentPosition]);

		detectionCount -= undetectionSpeed;

		if((detectionCount <= 0.0f)  && hasReturned){
			detectionCount = 0.0f;
			setState(SecCamState.waiting);
		}else{
			sr.color = new Color(1,1,1-detectionCount);
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Rotaciona a camera para a posiçao onde esta o jogador
	//------------------------------------------------------------------------------------------------------------------
	bool lookingAtPlayer(){
		Vector2 distance = (Vector2)Player.player.transform.position - (Vector2)pivot.transform.position; // Distancia da camera ao jogador
		float newAngle = Mathf.Atan2(-distance.y,distance.x) *  Mathf.Rad2Deg - 30;                       // rotaçao da camera em relaçao ao jogador
		return changeAngle(-newAngle);																	  // Posiciona a camera olhando para o jogador
	}

	//------------------------------------------------------------------------------------------------------------------
	// Realoca a rotaçao da camera para o angulo designado. Retorna TRUE caso a camera ja esteja no local correto
	//------------------------------------------------------------------------------------------------------------------
	bool changeAngle(float destinationAngle){
		float currentAngle = pivot.transform.rotation.eulerAngles.z;

		// Calcula os dois arcos entre o ponto incial ate o final e utiliza o menor para traçar a rota
		float distanceArc = destinationAngle - currentAngle;

		float alternativeDestinationAngle = (360 - Mathf.Abs(destinationAngle)) * -Mathf.Sign(distanceArc);
		float alternativeDistanceArc = alternativeDestinationAngle - currentAngle;

		// Checa se camera ja esta na posiçao correta
		if(distanceArc == 0 || distanceArc == 360 || alternativeDistanceArc == 0 || alternativeDistanceArc == 360) return true; 

		// Move a camera para o ponto final, obedecendo o limite de velocidade de rotaçao estabelicido
		float limit = followMaxSpeed * (playerDetected?3:1); // Multipla a velocidade da camera por 3 caso o personagem tenha sido detectado
		float increment = Mathf.Clamp((Mathf.Abs(distanceArc)<Mathf.Abs (alternativeDistanceArc))?distanceArc:alternativeDistanceArc,-limit,limit);

		pivot.rotation = Quaternion.identity;
		pivot.Rotate(
			new Vector3(0,0,currentAngle + increment)
		);

		return false;
	}



	//------------------------------------------------------------------------------------------------------------------
	// Troca o estado da camera, fazendo os ajustes necessarios
	//------------------------------------------------------------------------------------------------------------------
	public void setState(SecCamState newState){
		if(state == newState) return;
		switch(newState){
		case SecCamState.rotating:
			break;
		default: // Waiting
			//waitCount = 0;
			break;
		}
		state = newState;
	}
}

// Enum que controla os estados da camera
public enum SecCamState{
	rotating,			// Camera se movendo para entao ficar parada novamente
	waiting,			// Camera parada esperando o tempo para se mover novamente
}