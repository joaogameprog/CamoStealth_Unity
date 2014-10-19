using UnityEngine;
using System.Collections;

public class JumpSkillModule : PlayerCommand {

	//Flags
	public bool wasPressed = false;              // Contem a informaçao de botao de açao estava sendo pressionado na iteraçao anterior
	public bool dashJumping = false;             // Indica se o personagem esta realizando um dashJump

	// Jump
	public float jumpPower = 6;		             // Força do salto

	//Wall Jump
	public float wallJumpCooldown = 0;           // cooldown do wallJump
	public float wallJumpTime = 8;               // Tempo inicial do coolDown do wallJump

	// Air Jump
	public float airJumpMax = 2;                 // Maximo do saltos sem tocar o chao
	public float airJumpCount = 0;               // Quantidade de saltos realizados sem tocar o chao

	//------------------------------------------------------------------------------------------------------------------
	// Sobrecarga do comando principal
	//------------------------------------------------------------------------------------------------------------------
	public override void runCommand(){
		checkCollision();
		base.runCommand();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para salto sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Hiding")) return false; // Salto bloquado enquanto se esconde
		return isKeyPressed(); // Tecla necessaria para realizar a açao
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se a tecla do comando esta pressionada
	//------------------------------------------------------------------------------------------------------------------
	private bool isKeyPressed() {
		return (Input.GetAxisRaw("Jump") != 0);
	}

	//------------------------------------------------------------------------------------------------------------------
	// tenta realizar um salto
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		--wallJumpCooldown; // Diminue gradativamente o Cooldown para que o personagem possa se movimentar novamente
		if(!wasPressed){
			if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Wall"))
				wallJump();
			else if(airJumpCount < airJumpMax)
				normalJump();
		}
		wasPressed = true;
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Realiza um WallJump
	//------------------------------------------------------------------------------------------------------------------
	void wallJump(){
		dashJumping = (Input.GetAxisRaw("Fire2") != 0);
		wallJumpCooldown = wallJumpTime;
		float horizontal = jumpPower*-Mathf.Sign(transform.localScale.x)/(dashJumping?1:2);
		rb.velocity = new Vector2(horizontal,jumpPower);
		++airJumpCount;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Realiza um Salto normal
	//------------------------------------------------------------------------------------------------------------------
	void normalJump(){
		if(!(anim.GetCurrentAnimatorStateInfo(0).IsTag("Iddle"))) ++airJumpCount; // Apenas adciona +1 no contador caso o personagem esteja no chao, pois esse +1 eh adcionado automaticamente na checagem de colisao com o chao quando o personagem cai de uma plataforma
		dashJumping = dash.isDashing() && wall.groundColliding; // dashJump so pode ser realizado se o personagem estiver em meio a um dash e colidindo com o chao
		float horizontal = dashJumping?rb.velocity.x:0;
		rb.velocity = new Vector2(horizontal,jumpPower);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Zera a velocidade vertical, e reabilita o salto do personagem
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		if(wasPressed && rb.velocity.y>0) rb.velocity = new Vector2(rb.velocity.x,0);
		wasPressed = false;
		wallJumpCooldown = 0; // zera o o Cooldown para que o personagem possa se movimentar novamente
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se o personagem colidiu com plataformas, para resetar a quantidade de saltos que ele pode executar
	//------------------------------------------------------------------------------------------------------------------
	private void checkCollision(){
		if((wall.platformColliding && anim.GetCurrentAnimatorStateInfo(0).IsTag("Wall")) || wall.groundColliding){
			airJumpCount = 0; //Reabilita o airJump caso o personagem toque o chao ou uma plataforma
			dashJumping = false;
		}
	}
}
