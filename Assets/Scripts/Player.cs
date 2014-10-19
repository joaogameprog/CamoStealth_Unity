using UnityEngine;
using System.Collections;

public class Player : PlayerBehaviour {

	public bool death = false; // variavel de controle
	public int  deathTime = 0; // variavel de controle

	public float health = 100;               // Pontos de vida do personagem

	public GameObject CollisionChecker;
	
	GUIStyle gs = new GUIStyle();
	
	void OnGUI(){
		GUI.Label(new Rect(100,100,100,100),health.ToString() + "\n" + rb.velocity.ToString(),gs);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Update do personagem - Checa todos os comandos e açoes
	//------------------------------------------------------------------------------------------------------------------
	void FixedUpdate () {
		if(Exit.victory) return;
		if(death){ die(); return;}
		
		hide.runCommand();
		wall.runCommand();
		dash.runCommand();
		jump.runCommand();
		move.runCommand();

		updateAnimationFlags();
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Atualiza as flags dos estados de animaçao do personagem
	//------------------------------------------------------------------------------------------------------------------
	void updateAnimationFlags(){
		anim.SetInteger("HorizontalAxis",(int)move.horizontal);
		anim.SetInteger("Dashing",(int)GetComponent<DashSkillModule>().dashing);
		anim.SetBool("PlatformColliding",wall.platformColliding);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Late Update
	//------------------------------------------------------------------------------------------------------------------
	void LateUpdate(){
		anim.SetInteger("SpeedY",(int)(rigidbody2D.velocity.y*100));
		anim.SetInteger("SpeedX",(int)(rigidbody2D.velocity.x*100));
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Metodo que chama a animaçao de morte do personagem, e em seguida a tela de titulo
	//------------------------------------------------------------------------------------------------------------------
	void die(){
		transform.DetachChildren();
		anim.SetBool("Death",true);
		rigidbody2D.isKinematic = true;
		
		sr.color = new Color(sr.color.r + 0.1f,sr.color.g - 0.1f,sr.color.b - 0.1f,sr.color.a -0.01f);
		float x = (deathTime+150)/1500.0f;
		transform.position += new Vector3(x/10*-(Mathf.Sign(transform.localScale.x)),x/10,0);
		if(--deathTime <=-150)
			Application.LoadLevel("MainMenuScreen");
	}
	
}