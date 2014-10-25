using UnityEngine;
using System.Collections;

public class Aranha : Damager {

	Animator animator;

	public float speed = 0.02f;
	public float walkRange = 6;
	public int restTime = 300;

	public int initialXPosition;
	public int resting = 0;

	public bool playerDetected = false;

	public GameObject bulletType = null;
	public float shotCooldown = 12;
	private float shotCooldownCount = 0;
	public AudioClip shootingAudio;
	public AudioClip roarAudio;
	public SpiderSound soundToPlay = SpiderSound.nothing;

	void Awake(){
		animator = GetComponent<Animator>();
		initialXPosition = (int)transform.position.x;
		if(restTime <= 0) restTime = 1;
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
			int direction = (int)Mathf.Sign(transform.localScale.x);
			GameObject bullet = Instantiate(bulletType,transform.position + new Vector3(0.56f * direction,0.24f,0),Quaternion.identity) as GameObject;
			bullet.GetComponent<Gunshot>().speed *= direction;
			bullet.transform.localScale = new Vector3(bullet.transform.localScale.x*direction,bullet.transform.localScale.y,bullet.transform.localScale.z);
			soundToPlay = SpiderSound.gunshot;
		}
	}
}

// Enum que indica qual som deve ser tocado
public enum SpiderSound{
	nothing,
	gunshot,
	death
}
