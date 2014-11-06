//######################################################################################################################
// Aranha
// * Script que controla o comportamento das aranhas
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class Aranha : Damager {
	// Atalhos
	Animator animator;
	SpiderArm spiderArm;
	SpriteRenderer sr;

	// Movimento
	public float speed = 0.02f;                            // Velocidade de movimento da aranha
	public float walkRange = 6;                            // Distancia percorrida pela aranha quando ela esta andando
	public int restTime = 300;                             // Tempo que aranha fica parada
	public float initialXPosition;                         // Posiçao incial da aranha
	public int restCount = 0;                              // Contador do tempo que a aranha esta parada
	public int returningDelayTime = 20;                    // Tempo que a aranha fica olhando pro nada depois que perde o jogador de vista
	private int returningDelayCount = 0;                   // Contador do tempo de delay de retorno

	// Colisao com plataforma
	public bool platformColliding = false;                 // Checa se a aranha esta colidindo com uma parede                

	//Estados
	public SpiderState state; 						       // Estado da aranha
	public Vector3 lastPosition;                           // Ultima posiçao da aranha

	// Detecçao por visao
	public bool playerInSight = false;                     // Checa se o personagem esta dentro do campo de visao da aranha
	public float playerDetectionCount = 0;                 // Contador do estado de detecçao do jogador
	public float detectionSpeed = 0.1f;                    // Velocidade de detecçao
	public float undetectionSpeed = 0.005f;                // Velocidade em que a aranha perde os pontos de detecçao caso o personagem nao tenha sido totalmente detectado

	//Detecçao por audiçao
	public float hearing = 0;                              // Intensidade de som que a aranha esta ouvindo, funciona como um detectionSpeed alternativo
	public float hearingAmplifier = 2;                     // Intensidade da audiçao da aranha. O valor captado pelo hearing eh elevado a este valor
	public Vector3 lastPlayerHeardPosition;                // Ultima posiçao do jogador que a aranha percebeu

	// Ataque
	public GameObject bulletType = null;                   // GameObject de bala disparado pela aranha
	public float shotCooldown = 12;                        // Intervalo de frames em que a aranha executa um tiro
	private float shotCooldownCount = 0;                   // Contador do tempo de cooldown para que a aranha atire novamente
	public float ArmMaxRotationSpeed = 1.0f;			   // Velocidade maxima de rotaçao dos braços
	public GameObject bulletSpawner;            	       // Objeto onde as balas spawnam
	
	//Emoticons
	public GameObject wut;                                 // Interrogaçao
	private bool wuted = false;                            // Bloqueio do emoticon Interrogaçao
	public GameObject hey;                                 // Exclamaçao
	private bool heyed = false;                            // Bloqueio do emoticon exclamaçao

	//som
	public AudioClip shootingAudio;                        // Som dos tiros da aranha
	public AudioClip roarAudio;                            // Som de morte da aranha
	public AudioClip emoticonAudio;                        // Som das exclamaçoes e interrogaçoes
	public SpiderSound soundToPlay = SpiderSound.nothing;  // Som que deve ser tocado no frame seguinte

	// Campo de visao
	private SpiderVision vision;                           // Campo de visao da aranha
	private SpriteRenderer visionSprite;                   // SpriteRenderer do compo de visao da aranha

	//------------------------------------------------------------------------------------------------------------------
	// Seta os valores inciais da aranha
	//------------------------------------------------------------------------------------------------------------------
	void Awake(){
		animator = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		setColorOnChildren();
		initialXPosition = transform.position.x;
		if(restTime <= 0) restTime = 1;
		vision = GetComponentInChildren<SpiderVision>();
		visionSprite = vision.gameObject.GetComponent<SpriteRenderer>();
	}

	void Start(){
		spiderArm = GetComponentInChildren<SpiderArm>();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Update da aranha - Checa todos os comandos e açoes
	//------------------------------------------------------------------------------------------------------------------
	void LateUpdate () {
		if(Exit.victory) return;
		switch(state){
		case SpiderState.normal:
			returningDelayCount = 0;
			normalInstance();
			break;
		case SpiderState.alert:
			alertInstance();
			break;
		case SpiderState.returning:
			returningDelayCount = 0;
			returningInstance();
			break;
		}
		incrementDetectionCount();
		playSound();
		setSightColor();
		animator.SetBool("Resting",lastPosition == transform.position);//state == SpiderState.returning ? false : state == SpiderState.alert? ((playerDetectionCount <= 0 && state != SpiderState.normal)?true:false) : restCount != 0);
		animator.SetBool("PlayerDetected",state == SpiderState.alert && playerInSight);
		lastPosition = transform.position;
	}

	//------------------------------------------------------------------------------------------------------------------
	// incrementa o valor de detecçao
	//------------------------------------------------------------------------------------------------------------------
	void incrementDetectionCount(){
		float previousCount = playerDetectionCount;
		playerDetectionCount = Mathf.Clamp(playerDetectionCount + (playerInSight? detectionSpeed:0) + (hearing>0? hearing:0),0.0f,1.0f);
		if(playerDetectionCount >= 1){
			callHey();
			state = SpiderState.alert;
			returningDelayCount = returningDelayTime;
		}
		if(playerDetectionCount <= 0 && state != SpiderState.normal){
			heyed = false;
			callWut();
			if(--returningDelayCount <= 0){
				state = SpiderState.returning;
			}
		}

		if(previousCount != playerDetectionCount)
			setSightColor();
		else
			playerDetectionCount = Mathf.Clamp(playerDetectionCount - undetectionSpeed,0.0f,1.0f);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Toca o som pertinente ao momento
	//------------------------------------------------------------------------------------------------------------------
	void playSound(){
		switch(soundToPlay){
		case SpiderSound.gunshot:
			if(audio.clip != shootingAudio) audio.clip = shootingAudio;
			audio.pitch = Random.Range(1.0f,2.0f);
			audio.Play();
			break;
		case SpiderSound.hey:
			if(audio.clip != emoticonAudio) audio.clip = emoticonAudio;
			audio.pitch = Random.Range(2.0f,3.0f);
			audio.Play();
			break;
		case SpiderSound.wut:
			if(audio.clip != emoticonAudio) audio.clip = emoticonAudio;
			audio.pitch = Random.Range(0.5f,1.5f);
			audio.Play();
			break;
		}
		soundToPlay = SpiderSound.nothing;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Instancia normal da aranha, fazendo patrulha
	//------------------------------------------------------------------------------------------------------------------
	void normalInstance(){
		shotCooldownCount = 0;
		if(restCount > 0){
			if(--restCount == 0){
				transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
			}
		}else{
			float destination = initialXPosition + ((transform.localScale.x > 0 && walkRange > 0)?walkRange : 0);
			float distance = destination - transform.position.x;
			float increment = Mathf.Clamp(distance,-speed,speed);
			transform.position += new Vector3(increment,0,0);
			if(transform.position.x == destination)
				restCount = restTime;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Instancia de alerta da aranha
	//------------------------------------------------------------------------------------------------------------------
	void alertInstance(){
		// Faz a aranha olhar para o jogador
		if(lastPlayerHeardPosition.x > transform.position.x != transform.localScale.x > 0)
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);

		if(playerInSight && !Player.player.hide.trulyHiding)
			shootAtPlayer();
		else
			searchForPlayer();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Instancia de alerta da aranha
	//------------------------------------------------------------------------------------------------------------------
	void returningInstance(){
		// Faz a aranha olhar para a posiçao inicial
		if(lastPlayerHeardPosition.x < initialXPosition != transform.localScale.x > 0)
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);

		if(!platformColliding){
			float increment = Mathf.Clamp(initialXPosition - lastPlayerHeardPosition.x,-speed,speed);
			transform.position += new Vector3(increment,0,0);
		}
		if(transform.position.x == initialXPosition){
			Instantiate(wut,transform.position + new Vector3(0,0.5f,0),Quaternion.identity);
			state = SpiderState.normal;
			wuted = false;
			returningDelayCount = 0;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// A aranha atira no jogador
	//------------------------------------------------------------------------------------------------------------------
	void shootAtPlayer(){
		if(++shotCooldownCount >= shotCooldown) shotCooldownCount = 0;
		if(shotCooldownCount<= 0){
			bulletType.GetComponent<Gunshot>().copyBullet(
				bulletSpawner.transform.position,
				spiderArm.transform.localEulerAngles.z,
				(int)Mathf.Sign(transform.localScale.x)<0
				);
			soundToPlay = SpiderSound.gunshot;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// A aranha procura pelo jogador
	//------------------------------------------------------------------------------------------------------------------
	void searchForPlayer(){
		if(returningDelayCount > 0 && wuted || platformColliding) return;
		if(!(transform.position.x > lastPlayerHeardPosition.x-0.5f && transform.position.x < lastPlayerHeardPosition.x+0.5f))
			transform.position += new Vector3(speed*Mathf.Sign(transform.localScale.x),0,0);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Replica a cor desse renderer para os renderers filhos
	//------------------------------------------------------------------------------------------------------------------
	void setColorOnChildren(){
		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer renderer in renderers)
			renderer.color = sr.color;
	}

	//------------------------------------------------------------------------------------------------------------------
	// seta a cor do campo de visao
	//------------------------------------------------------------------------------------------------------------------
	void setSightColor(){
		visionSprite.color = new Color(1,1,1-playerDetectionCount);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Exibe o emoticon Hey
	//------------------------------------------------------------------------------------------------------------------
	void callHey(){
		if(heyed) return;
		soundToPlay = SpiderSound.hey;
		Instantiate(hey,transform.position + new Vector3(0,0.5f,0),Quaternion.identity);
		heyed = true;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Exibe o emoticon Wut
	//------------------------------------------------------------------------------------------------------------------
	void callWut(){
		if(wuted) return;
		soundToPlay = SpiderSound.wut;
		Instantiate(wut,transform.position + new Vector3(0,0.5f,0),Quaternion.identity);
		wuted = true;
	}
}

// Enum que indica qual som deve ser tocado
public enum SpiderSound{
	nothing,
	gunshot,
	hey,
	wut,
	death
}

// Enum que indica o estado da aranha
public enum SpiderState{
	normal,
	alert,
	returning
}
