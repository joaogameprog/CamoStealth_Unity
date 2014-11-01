//######################################################################################################################
// Player
// * Script responsavel por controlar as açoes do jogador. Eh atrelado diretamente varios modulos que controlam 
// partes especificas do jogador, como salto, dash, esconder, dano, etc
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class Player : PlayerBehaviour {

	new public static Player player;

	public GameObject CollisionCheckerFront;
	public GameObject CollisionCheckerDown;

	//------------------------------------------------------------------------------------------------------------------
	// Inicializaçao do personagem
	//------------------------------------------------------------------------------------------------------------------
	void Awake(){
		player = this; // Referencia global ao personagem
	}

	//------------------------------------------------------------------------------------------------------------------
	// Update do personagem - Checa todos os comandos e açoes
	//------------------------------------------------------------------------------------------------------------------
	void FixedUpdate () {
		if(Exit.victory) return;
		dmg.runCommand();

		hide.runCommand();
		wall.runCommand();
		dash.runCommand();
		jump.runCommand();
		move.runCommand();

		sound.runCommand();
		updateAnimationFlags();
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Atualiza as flags dos estados de animaçao do personagem
	//------------------------------------------------------------------------------------------------------------------
	void updateAnimationFlags(){
		anim.SetInteger("HorizontalAxis",(int)move.horizontal);
		anim.SetInteger("Dashing",(int)GetComponent<DashSkillModule>().dashing);
		anim.SetBool("PlatformColliding",wall.platformColliding);
		anim.SetBool("Damage",dmg.damageTimeCount>0 && !dmg.death);
		anim.SetBool("Death",dmg.death);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Late Update
	//------------------------------------------------------------------------------------------------------------------
	void LateUpdate(){
		anim.SetInteger("SpeedY",(int)(rigidbody2D.velocity.y*100));
		anim.SetInteger("SpeedX",(int)(rigidbody2D.velocity.x*100));
	}

} 