//######################################################################################################################
// PunchSkillModule
// * Modulo do jogador responsavel por controlar a habilidade de soco
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class PunchSkillModule : PlayerCommand {

	[HideInInspector] public bool wasPressed = false;    // Contem a informaçao de botao de açao estava sendo pressionado na iteraçao anterior
	public bool isPunching = false;
	public int punchTime = 32;
	public int punchCount = 0;
	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para esconder-se sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Iddle")) return false; // Soco so funciona quando estiver no chao
		if(isPunching) return false;
		return isKeyPressed(); // Tecla necessaria para realizar a açao
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se a tecla do comando esta pressionada
	//------------------------------------------------------------------------------------------------------------------
	private bool isKeyPressed() {
		return (Input.GetAxisRaw("Punch") != 0);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Torna o personagem invisivel
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		isPunching = true;
		punchCount = punchTime;
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Torna o personagem visivel novamente
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		if(dmg.damageTimeCount > 0){
			isPunching = false;
			punchCount = 0;
			return;
		}

		if(!isPunching) return;
		if(--punchCount > 0) return;

		isPunching = false;
		punchCount = 0;

	}
}
