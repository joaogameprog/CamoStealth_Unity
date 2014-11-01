//######################################################################################################################
// SpiderArm
// * Controla a rotaçao dos braços das aranhas
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class SpiderArm : MonoBehaviour {
	// Referencias
	Aranha aranha;
	Animator[] animators;

	//------------------------------------------------------------------------------------------------------------------
	// Seta os valores inciais
	//------------------------------------------------------------------------------------------------------------------
	void Awake(){
		animators = GetComponentsInChildren<Animator>();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Seta os valores inciais
	//------------------------------------------------------------------------------------------------------------------
	void Start(){
		aranha = transform.parent.GetComponent<Aranha>();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Gira os braços de acordo com a posiçao do jogador e spawna os tiros na ponta da arma
	//------------------------------------------------------------------------------------------------------------------
	void FixedUpdate(){
		SetAnimationFlags();
		if(!aranha.playerDetected) return;
		rotateArms();
	}

	void rotateArms(){
		Vector2 distance = (Vector2)Player.player.transform.position - (Vector2)transform.position;       // Distancia da camera ao jogador
		float newAngle = Mathf.Atan2(-distance.y,distance.x) *  Mathf.Rad2Deg;                            // rotaçao da camera em relaçao ao jogador

		if(aranha.transform.localScale.x < 0) newAngle = Mathf.Abs(newAngle - 180); 	                  // Corrige o angulo caso a aranha esteja invertida

		float destinationAngle = -newAngle;																  // Posiciona a camera olhando para o jogador
		
		float currentAngle = transform.rotation.eulerAngles.z;

		newAngle = Mathf.MoveTowardsAngle(currentAngle,destinationAngle,aranha.ArmMaxRotationSpeed);

		float clampedAngle = clampAngle(newAngle);

		transform.rotation = Quaternion.identity;
		transform.Rotate(
			new Vector3(0,0,clampedAngle)
		);

		fixArmPosition(clampedAngle);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Limita o valor de rotaçao dos braços em ate 30 graus para baixo ou para cima
	//------------------------------------------------------------------------------------------------------------------
	float clampAngle(float angle){
	
			if(angle < 180 && angle >= 30) return 30;
			if(angle >= 180 && angle <= 330) return 330;
		return angle;

	}

	//------------------------------------------------------------------------------------------------------------------
	// Ajusta a posiçao do braço em relaçao a rotaçao
	//------------------------------------------------------------------------------------------------------------------
	void fixArmPosition(float angle){
		if(angle >= 180) angle -=360;
		transform.localPosition = new Vector3(Mathf.Abs(angle)/200.0f + 0.6f,0.8f - (Mathf.Abs( Mathf.Clamp(angle,0,15)/400.0f)),0);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Seta as flags de animaçao nos dois braços
	//------------------------------------------------------------------------------------------------------------------
	void SetAnimationFlags(){
		animators[0].SetBool("Shooting",aranha.playerDetected);
		animators[1].SetBool("Shooting",aranha.playerDetected);
	}
}
