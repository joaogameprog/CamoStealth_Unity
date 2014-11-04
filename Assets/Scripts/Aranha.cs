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
	public int initialXPosition;                           // Posiçao incial da aranha
	public int resting = 0;                                // Contador do tempo que a aranha esta parada

	//Detecçao
	public bool playerDetected = false;                    // Detecçao do personagem
	public SpiderState state = SpiderState.resting;        // Estado da aranha

	// Ataque
	public GameObject bulletType = null;                   // GameObject de bala disparado pela aranha
	public float shotCooldown = 12;                        // Intervalo de frames em que a aranha executa um tiro
	private float shotCooldownCount = 0;                   // Contador do tempo de cooldown para que a aranha atire novamente
	public float ArmMaxRotationSpeed = 1.0f;			   // Velocidade maxima de rotaçao dos braços
	public GameObject bulletSpawner;            	       // Objeto onde as balas spawnam

	//som
	public AudioClip shootingAudio;                        // Som dos tiros da aranha
	public AudioClip roarAudio;                            // Som de morte da aranha
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
		initialXPosition = (int)transform.position.x;
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
	void FixedUpdate () {
		if(Exit.victory) return;
		if(!playerDetected){
			normalInstance();
		}else{
			attackInstance();
		}
		playSound();
		animator.SetInteger("Resting",resting);
		animator.SetBool("PlayerDetected",playerDetected);
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
		}
		soundToPlay = SpiderSound.nothing;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Instancia normal da aranha, fazendo patrulha
	//------------------------------------------------------------------------------------------------------------------
	void normalInstance(){
		shotCooldownCount = 0;
		if(resting > 0){
			if(--resting == 0){
				transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
			}
		}else{
			transform.position += new Vector3(speed*Mathf.Sign(transform.localScale.x),0,0);
			if((int)transform.position.x == initialXPosition+(Mathf.Sign(transform.localScale.x)>=0?walkRange:0))
				resting = restTime;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Instancia de ataque da aranha, atirando no jogador
	//------------------------------------------------------------------------------------------------------------------
	void attackInstance(){
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
	// Troca o estado da aranha, fazendo os ajustes necessarios
	//------------------------------------------------------------------------------------------------------------------
	public void setState(SpiderState newState){
		if(state == newState) return;
		switch(newState){
		case SpiderState.searching:
			visionSprite.color = Color.yellow;
			break;
		case SpiderState.following:
			visionSprite.color = Color.red;
			break;
		case SpiderState.shooting:
			visionSprite.color = Color.red;
			break;
		case SpiderState.walking:
			visionSprite.color = Color.white;
			break;
		default:
			visionSprite.color = Color.white;
			break;
		}
		state = newState;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Replica a cor desse renderer para os renderers filhos
	//------------------------------------------------------------------------------------------------------------------
	void setColorOnChildren(){
		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer renderer in renderers)
			renderer.color = sr.color;
	}

}

// Enum que indica qual som deve ser tocado
public enum SpiderSound{
	nothing,
	gunshot,
	death
}

// Enum que indica o estado da aranha
public enum SpiderState{
	resting,
	walking,
	searching,
	following,
	shooting
}
