using UnityEngine;
using System.Collections;

public class DashSkillModule : PlayerCommand {

	//Flags
	protected bool wasPressed = false;           // Contem a informaçao de botao de açao estava sendo pressionado na iteraçao anterior
	public int     dashing = -1000;              // controle do dash. Quando for menor que zero, o dash termina.

	// Dash
	public int   dashTime = 15; 		         // Tempo de duraçao do dash
	public float speedMultiplyer = 3.0f;         // Velocidade de movimento

	// Air Dash
	public  bool enableAirDash = true;           // habilita o dash no ar
	public  int  airDashMax = 2;                 // Quantidade maxima de vezes que o personagem pode realizar um airDash
	private int  airDashCount = 0;               // contador de vezes que o airDash foi realizado

	// Efeito visual
	public GameObject dashImage;                 // GameObject do rastro do dash
	public int dashEffectExtaTime = 12;          // Quantidade extra de frames que o efeito de dash dura depois do fim do dash propriamente dito
	public bool continueDashing = false;         // flag que indica que o personagem ainda deve ter o rastro do dash presente mesmo apos o dash
	
	//------------------------------------------------------------------------------------------------------------------
	// Tenta executar o comando de personagem ou interrompe-lo
	//------------------------------------------------------------------------------------------------------------------
	override public void runCommand() {
		base.runCommand();
		setGravity();
		dashEffect();
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para dash sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Hiding")) return false; // Bloqueia o comando caso o personagem esteja se escondendo
		if(!enableAirDash && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Iddle")) return false; // Bloqueia o airDash caso o personagem nao esteja no chao
		if(dashing > 0 || wasPressed) return false; // Bloqueia o dash caso o dash ja esteja sendo realizado
		if(airDashCount >= airDashMax) return false; // Bloqueia o dash caso o limite de dashes seja atingido
		return isKeyPressed();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se a tecla do comando esta pressionada
	//------------------------------------------------------------------------------------------------------------------
	private bool isKeyPressed() {
		return (Input.GetAxisRaw("Fire2") != 0);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Inicia o Dash
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		dashing = dashTime;
		jump.dashJumping = false; // Interrompe um dashJump caso um airDash seja executado
		continueDashing = true; // Controlador para o efeito visual de dash
		++airDashCount;
		rb.velocity = new Vector2(rb.velocity.x,0);
		wasPressed = true;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Interrompe o dash atual e reabilita o incio de um dash depois que tecla shift for solta
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		if(wall.platformColliding || wall.groundColliding){ // Checa se o personagem tocou alguma plataforma
			airDashCount = 0; // Reabilita o airDash caso o personagem esteja em contato com uma plataforma
			continueDashing = false; // Interrompe o efeito visual do dash
		}
		if(wall.platformColliding || !isKeyPressed()) dashing = -dashEffectExtaTime; // Interrompe o dash subtamente
		else --dashing; // Interrompe o dash gradativamente
		wasPressed = isKeyPressed();
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Elimina a gravidade enquanto esta realizando o dash, e volta a gravidade ao normal quando o dash eh interrompido
	//------------------------------------------------------------------------------------------------------------------
	void setGravity(){
		if(dashing > 0 && !jump.dashJumping) rb.gravityScale = 0;
		else rb.gravityScale = 1;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Cria o efeito de dash
	//------------------------------------------------------------------------------------------------------------------
	void dashEffect(){
		if((dashing > -dashEffectExtaTime || continueDashing || jump.dashJumping)){
			GameObject image = Instantiate(dashImage,this.transform.position,this.transform.rotation) as GameObject;
			image.transform.localScale = transform.localScale;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Retorna true caso o personagem ainda esteja no meio da execuçao de um dash
	//------------------------------------------------------------------------------------------------------------------
	public bool isDashing(){
		return dashing > 0;
	}
}
