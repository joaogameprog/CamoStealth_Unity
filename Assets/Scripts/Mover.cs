using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	/*
	//Atalhos
	Animator animator;
	Rigidbody2D rb;
	SpriteRenderer sr;

	//Botoes da HUD
	private Rect left;
	private Rect up;
	private Rect right;
	private Rect jumpy;
	private Rect dash;

	void Awake(){
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		Rect left  = new Rect(                  0, Screen.height*0.9f, Screen.width*0.1f, Screen.height);
		Rect up    = new Rect(Screen.width*0.101f, Screen.height*0.9f, Screen.width*0.1f, Screen.height);
		Rect right = new Rect(Screen.width*0.201f, Screen.height*0.9f, Screen.width*0.1f, Screen.height);
		Rect jumpy  = new Rect(Screen.width*0.751f, Screen.height*0.9f, Screen.width*0.1f, Screen.height);
		Rect dash  = new Rect(Screen.width*0.851f, Screen.height*0.9f, Screen.width*0.1f, Screen.height);
	}

	// Use this for initialization
	void Start () {

	}

	public bool platformColliding = false;  // variavel de controle
	public bool pressed = false;  // variavel de controle
	public bool pressedShift = false;  // variavel de controle
	public float  horizontal;  // variavel de controle
	public int  dashing = -1000; // variavel de controle
	public bool trulyHiding = false; // variavel de controle
	public bool continueDashing = false; // variavel de controle
	public bool death = false; // variavel de controle
	public int  deathTime = 0; // variavel de controle

	public int   dashTime = 15; 			 // Tempo de duraçao do dash
	public float jump = 6;				     // Força do salto
	public float speed = 1.8f;				 // Velocidade de movimento
	public float dashVelocity = 3;           // Intesidade do Dash
	public float wallGlideVelocity = -0.05f; // Intasidade da força do deslizamento na parede

	public int dashEffectTime = 12;
	public GameObject dashImage;
	public GameObject CollisionChecker;

	public int wallJumpCooldown;

	private enum TouchState_Directional{
		nothing,up,left,right
	}

	private enum TouchState_Action{
		nothing,dash,jump
	}


	// Update is called once per frame
	void FixedUpdate () {
		TouchState_Directional touchState_d = getTouchStatus_d();
		TouchState_Action touchState_a = getTouchStatus_a();
		if(Exit.victory) return;
		if(death){transform.DetachChildren(); animator.SetBool("Death",true); rigidbody2D.isKinematic = true; die(); return;} // morrendo

		// Escondendo-se
		if((Input.GetAxis("Vertical") > 0f || touchState_d == TouchState_Directional.up) && (animator.GetCurrentAnimatorStateInfo(0).IsTag("Iddle") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hiding"))){
			animator.SetBool("Hiding",true);
			if(trulyHiding) sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,sr.color.a + ((sr.color.a>0.3)?-0.05f:0));
			sr.sortingOrder = -2;
		}else{
			// Desescondendo-se
			animator.SetBool("Hiding",false);
			if(sr.color.a < 1) sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,sr.color.a + 0.1f);
			sr.sortingOrder = 0;

			// Checa se o personagem esta colidindo contra uma parede, e aplica a velocidade de deslizamento ---------------
			if(platformColliding && horizontal != 0) rb.velocity = new Vector2(rb.velocity.x, wallGlideVelocity);

			//Dash ---------------------------------------------------------------------------------------------------------
			if(Input.GetAxis("Fire2") != 0 || Input.GetKey(KeyCode.Keypad6) || touchState_a == TouchState_Action.dash){
				// Inicia o Dash
				if(dashing <=0 && animator.GetCurrentAnimatorStateInfo(0).IsTag("Iddle") && !pressedShift) 
				{
					rb.velocity = new Vector2(dashVelocity * Mathf.Sign(transform.localScale.x),rb.velocity.y);
					rb.gravityScale = 0;
					dashing = dashTime;
					continueDashing = true;
				}else{
					// Finliza o dash
					if(--dashing <= 0){
						rb.velocity = new Vector2(0,rb.velocity.y);
						if(dashing > 0){
							dashing = 0;
						}
						if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Iddle") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Wall"))
							continueDashing = false;
						rb.gravityScale = 1;
					}
				}
				pressedShift = true;
			}else{ 
				if(--dashing > 0 && dashing < dashTime/2){ // Interrompe o dash ( dash so pode ser interrompido depois da metade
					rb.velocity = new Vector2(0,rb.velocity.y);
					dashing = 0;
					rb.gravityScale = 1;
				}
				// Reabilita o dash depois que um dash foi finalizado e a tecla shift foi solta
				if(pressedShift && dashing <= 0) pressedShift = false;
				if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Iddle") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Wall"))
					continueDashing = false;
			}

			// Checagens de salto ------------------------------------------------------------------------------------------
			if(Input.GetAxis("Jump") != 0 || Input.GetKey(KeyCode.Keypad5) || touchState_a == TouchState_Action.jump){
				//WallJump
				if(!pressed && animator.GetCurrentAnimatorStateInfo(0).IsTag("Wall")) 
				{
					wallJumpCooldown = 5;
					rb.velocity = new Vector2(jump*Mathf.Sign(transform.localScale.x)/-2,jump);
				}
				// Salto Normal
				if(!pressed) 
				{
					if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Iddle"))
						rb.velocity = new Vector2(0,jump);
					//else if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Dash"))
						//rb.velocity = new Vector2(rb.velocity.x,jump);

				}
				pressed = true;
			}else{ // Permite que o personagem salte novamente
				if(pressed && rb.velocity.y>0) rb.velocity = new Vector2(rb.velocity.x,0);
				pressed = false;

			}
		}
			// Movimentaçao horizontal -------------------------------------------------------------------------------------
		// Esta condiçao bloqueia por alguns frames a movimentaçao horizontal devido ao WallJump ou ao dash
		if(wallJumpCooldown <= 0 && dashing <= 0 && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Hiding")){ 
			horizontal =  (speed* Input.GetAxis("Horizontal"));//(((Input.GetKey(KeyCode.A) || touchState_d == TouchState_Directional.left)?-1:0) + ((Input.GetKey(KeyCode.D) || touchState_d == TouchState_Directional.right)?1:0));
			//float x = speed*horizontal;
			//if(horizontal != 0 && continueDashing)
				//x = (Mathf.Abs(x)>Mathf.Abs(rb.velocity.x))?x:rb.velocity.x;
			rb.velocity = new Vector2(horizontal,rb.velocity.y);
		}else{ // Diminue gradativamente o Cooldown para que o personagem possa se movimentar novamente
			--wallJumpCooldown;
			horizontal = 0;
		}


		//--------------------------------------------------------------------------------------------------------------
		animator.SetInteger("HorizontalAxis",(int)horizontal);
		animator.SetInteger("Dashing",(int)dashing);
		animator.SetBool("PlatformColliding",platformColliding);

		if(horizontal > 0 && transform.localScale.x < 0){
			transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
		}

		if(horizontal < 0 && transform.localScale.x > 0){ 
			transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
		}

		// Efeito de dash
		if((dashing > -dashEffectTime || continueDashing) && !(animator.GetCurrentAnimatorStateInfo(0).IsTag("Hiding"))){
			GameObject image = Instantiate(dashImage,this.transform.position,this.transform.rotation) as GameObject;
			image.transform.localScale = transform.localScale;
		}

	}
	
	void LateUpdate(){
		animator.SetInteger("SpeedY",(int)(rigidbody2D.velocity.y*100));
		animator.SetInteger("SpeedX",(int)(rigidbody2D.velocity.x*100));
	}

	void die(){
		//float r = 0.2f;
		//float g = 0;
		//float b = 0;
		//float a = 0.01f;
		//sr.color += new Color(r,g,b);
		//sr.color -= new Color(g,g,b,a);
		sr.color = new Color(sr.color.r + 0.1f,sr.color.g - 0.1f,sr.color.b - 0.1f,sr.color.a -0.01f);
		float x = (deathTime+150)/1500.0f;
		transform.position += new Vector3(x/10*-(Mathf.Sign(transform.localScale.x)),x/10,0);
		if(--deathTime <=-150)
			Application.LoadLevel("MainMenuScreen");
	}
	
	private TouchState_Directional getTouchStatus_d(){
		if (Input.touchCount == 0) return TouchState_Directional.nothing;
		for(int i = 0; i < Input.touchCount;++i){
			Vector2 position = Input.GetTouch(i).position;
			Rect pos = new Rect(position.x,position.y,position.x,position.y);
			if(up.Overlaps(pos,true)) return TouchState_Directional.up;
			if(left.Overlaps(pos,true)) return TouchState_Directional.left;
			if(right.Overlaps(pos,true)) return TouchState_Directional.right;
		}
		return TouchState_Directional.nothing;
	}

	private TouchState_Action getTouchStatus_a(){
		if (Input.touchCount == 0)  return TouchState_Action.nothing;
		for(int i = 0; i < Input.touchCount;++i){
			Vector2 position = Input.GetTouch(i).position;
			Rect pos = new Rect(position.x,position.y,position.x,position.y);
			if(jumpy.Overlaps(pos,true)) return TouchState_Action.jump;
			if(dash.Overlaps(pos,true)) return TouchState_Action.dash;
		}
		return TouchState_Action.nothing;
	}*/
}
