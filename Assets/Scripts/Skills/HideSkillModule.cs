using UnityEngine;
using System.Collections;

public class HideSkillModule : PlayerCommand {

	public bool trulyHiding = false; // variavel de controle

	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para esconder-se sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		bool isButtonPressed = (Input.GetAxisRaw("Vertical") > 0f);
		bool isIddleOrHiding = (anim.GetCurrentAnimatorStateInfo(0).IsTag("Iddle") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Hiding"));
		return isButtonPressed && isIddleOrHiding;
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Torna o personagem invisivel
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		anim.SetBool("Hiding",true);
		if(trulyHiding) sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,sr.color.a + ((sr.color.a>0.3)?-0.05f:0));
		sr.sortingOrder = -2;
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Torna o personagem visivel novamente
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		anim.SetBool("Hiding",false);
		if(sr.color.a < 1) sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,sr.color.a + 0.1f);
		sr.sortingOrder = 0;
	}

}
