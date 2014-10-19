using UnityEngine;
using System.Collections;

public class WallSkillModule : PlayerCommand {

	public bool platformColliding = false;  // true caso o personagem esteja tocando uma parede
	public bool groundColliding = false; // True caso o personagem esteja tocando o chao
	public float wallGlideVelocity = -0.05f; // Intensidade da força do deslizamento na parede

	override protected bool commandCondition(){
		return platformColliding && move.horizontal != 0;
	}

	override protected void startCommand(){
		rb.velocity = new Vector2(rb.velocity.x, wallGlideVelocity);
	}

	override protected void endCommand(){
		return;
	}
}
