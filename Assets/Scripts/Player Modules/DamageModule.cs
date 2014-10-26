//######################################################################################################################
// DamageModule
// * Modulo do jogador responsavel por controlar o dano sofrido pelo jogador
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class DamageModule : PlayerCommand {
	
	//Vida
	public float maxHealth = 100;                          // vida maxima do jogador
	private float health = 100;                             // vida do jogador

	/// <summary>
	/// Gets or sets the health.
	/// </summary>
	/// <value>The health.</value>
	public float Health 
	{ 
		get 
		{ 
			return health; 
		} 
		set 
		{ 
			health = Mathf.Clamp(value, 0, maxHealth); 
			if(hudManager != null)
			{
				hudManager.Life = health / maxHealth;
			}else{
				hudManager = GameObject.FindObjectOfType<HudManager>();
			}
		} 
	}                             
	public float regen = 0.1f;                             // vida que o jogador regenera por frame
	public float armor = 1;                                // armadura do jogador
	public float knockbackResist = 0;                      // porcentagem que eh ignoradado do knockback quando o personagem eh atingido (Varia de 0 a 1)
	public float invencibilityTime = 10;                   // Tempo total de invecibilidade pos-dano
	[HideInInspector] public float invencibilityCount = 0; // contador do tempo de invencibilidade
	public float damageTime = 5;                           // Tempo em que o personagem fica paralizado pelo dano
	[HideInInspector] public float damageTimeCount = 0;    // contador do tempo da animaçao de dano do personagem

	//Morte
	[HideInInspector] public bool death = false;           // informaçao se o jogador esta vivo ou morto
	[HideInInspector] public int  deathTime = 0;           // animaçao de morte do jogador

	//(Eu podia usar uma pilha de Damager aqui, mas ja que o personagem soh pode ser atingido por um objeto de cada vez, vai ficar assim mesmo)
	[HideInInspector] public Damager damager = null;       // Referencia ao objeto causador de dano 
	HudManager hudManager;
	GUIStyle gs = new GUIStyle();
	
	void OnGUI(){
		GUI.Label(new Rect(100,100,100,100),Health.ToString() + "\n" + rb.velocity.ToString(),gs);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Adciona o metodo que chama a animaçao de invencibilidade ao metodo principal
	//------------------------------------------------------------------------------------------------------------------
	public override void runCommand(){
		invencibilityEffect();
		base.runCommand();
		damager = null;

	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se o personagem esta em contato com um objeto danoso
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(death) return false; // bloqueia o comando caso o personagem esteja morto
		if(--damageTimeCount>0) return false;
		if(--invencibilityCount>0) return false;
		if(hide.trulyHiding) return false;
		return damager != null;
	}

	//------------------------------------------------------------------------------------------------------------------
	// aplica o dano
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		Health -= damager.damage - armor;
		death = Health <= 0;
		damageTimeCount = damageTime;
		invencibilityCount = invencibilityTime;
		rb.velocity = new Vector2(getKnockback(),0);
		sound.soundToPlay = LeonSound.damage;
	}

	//------------------------------------------------------------------------------------------------------------------
	// aplica o efeito de imunidade temporaria ou regeneraçao de vida
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		if(dmg.death) {
			die();
		}else{
			float result = Health + regen;
			Health = result>maxHealth? maxHealth : result;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Metodo que chama a animaçao de morte do personagem, e em seguida a tela de titulo
	//------------------------------------------------------------------------------------------------------------------
	public void die(){
		transform.DetachChildren();
		anim.SetBool("Death",true);
		rigidbody2D.isKinematic = true;

		sound.soundToPlay = LeonSound.death;
		
		sr.color = new Color(sr.color.r + 0.1f,sr.color.g - 0.1f,sr.color.b - 0.1f,sr.color.a -0.01f);
		float x = (deathTime+150)/1500.0f;
		transform.position += new Vector3(x/10*-(Mathf.Sign(transform.localScale.x)),x/10,0);

		if(--deathTime <=-150)
			Application.LoadLevel("MainMenuScreen");
	}

	//------------------------------------------------------------------------------------------------------------------
	// Animaçao do personagem piscando quando ele esta invisivel
	//------------------------------------------------------------------------------------------------------------------
	private void invencibilityEffect(){
		if(isInvisible())
			sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,0);
	}

	//------------------------------------------------------------------------------------------------------------------
	// retorna verdadeiro caso seja o frame onde personagem esta invisivel devido a invecibilidade
	//------------------------------------------------------------------------------------------------------------------
	public bool isInvisible(){
		return Time.frameCount/2%2 == 0 && damageTimeCount<=0 && invencibilityCount>0 && !death;
	}

	//------------------------------------------------------------------------------------------------------------------
	// retorna o valor do knockaback recebi pelo jogador baseado na força do damager atual
	//------------------------------------------------------------------------------------------------------------------
	private float getKnockback(){
		if(damager == null) return 0;
		knockbackResist = Mathf.Clamp(knockbackResist,0,1); // força que o valor da resistencia esteja entre 0 e 1
		return (-Mathf.Sign(transform.localScale.x) * damager.knockbackPower) * -(knockbackResist -1);
	}

	//------------------------------------------------------------------------------------------------------------------
	// força que o valor da resistencia esteja entre 0 e 1
	//------------------------------------------------------------------------------------------------------------------
	void OnDrawGizmos(){
		knockbackResist = Mathf.Clamp(knockbackResist,0,1);
	}
}
