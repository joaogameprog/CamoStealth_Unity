using UnityEngine;
using System.Collections;

public class MoveSkillModule : PlayerCommand {

	public float speed = 1.8f; // Velocidade de movimento
	public float horizontal;   // Velocidade horizonatal do personagem
	//------------------------------------------------------------------------------------------------------------------
	// Tenta executar o comando de personagem ou interrompe-lo
	//------------------------------------------------------------------------------------------------------------------
	override public void runCommand() {
		base.runCommand();

		fixFacingDirection();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para movimentaçao sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Hiding")) return false;
		return (jump.wallJumpCooldown <= 0);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Movimentaçao horizontal 
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		horizontal = GetHorizontalSpeed();
		rb.velocity = new Vector2(horizontal,rb.velocity.y);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Retorna a velocidade horizontal apropriada para o estado do personagem (Andando, correndo, etc
	//------------------------------------------------------------------------------------------------------------------
	float GetHorizontalSpeed(){
		if(jump.dashJumping)
			return speed * dash.speedMultiplyer * Input.GetAxisRaw("Horizontal"); // Velocidade de dashJumping
		if(dash.isDashing())
			return speed * dash.speedMultiplyer * Mathf.Sign(transform.localScale.x); // Velocidade de dash
		return speed * Input.GetAxisRaw("Horizontal"); // Velocidade normal
	}
	
	//------------------------------------------------------------------------------------------------------------------
	//faz com que o personagem pare de se mover
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		horizontal = 0;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Inverte a sprite do personagem para a direçao que ele esta olhando
	//------------------------------------------------------------------------------------------------------------------
	void fixFacingDirection(){
		if(horizontal > 0 && transform.localScale.x < 0){
			transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
		}
		if(horizontal < 0 && transform.localScale.x > 0){ 
			transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
		}
	}
}
