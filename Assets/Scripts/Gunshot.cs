//######################################################################################################################
// Gunshot
// * Componente que controla os tiros das armas das aranhas
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class Gunshot : Damager {

	public float speed  = 1; // Velocidade da bala
	public Vector2 rotation; // Direçao da bala (Vetor direçao)

	//------------------------------------------------------------------------------------------------------------------
	// Seta os valores iniciais
	//------------------------------------------------------------------------------------------------------------------
	void Awake(){
		//sr = GetComponent<SpriteRenderer>();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Programa a bala para desaparecer apos um tempo
	//------------------------------------------------------------------------------------------------------------------
	void Start () {
		Destroy(gameObject,60);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Update da bala. Incrementa a posiçao dela de acordo com a velocidade, rotaçao e sentido
	//------------------------------------------------------------------------------------------------------------------
	void Update () {
		this.transform.position += new Vector3(speed*rotation.x,speed*rotation.y,0);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Instancia esta bala trocando a posiçao, sentido e direçao
	//------------------------------------------------------------------------------------------------------------------
	public void copyBullet(Vector3 position, float rotation, bool revertDirection){
		Gunshot bullet = ((GameObject)Instantiate(gameObject,position,Quaternion.identity)).GetComponent<Gunshot>();
		//velocidade da bala
		bullet.speed *= revertDirection?-1:1;

		// Sentido da bala
		bullet.rotation = new Vector2(
			Mathf.Cos(rotation* Mathf.Deg2Rad),
			Mathf.Sin(rotation* Mathf.Deg2Rad)*(revertDirection?-1:1)
		);

		// Inverte a sprite da bala caso ela esteja indo para tras
		bullet.transform.localScale = new Vector3(
			bullet.transform.localScale.x*(revertDirection?-1:1),
			bullet.transform.localScale.y,
			bullet.transform.localScale.z
		);

		// Ajusta a rotaçao da bala de acordo com o sentido e direçao da bala
		bullet.transform.rotation = Quaternion.identity;
		bullet.transform.Rotate(
			new Vector3(
				0,
				0,
				rotation*(revertDirection?-1.0f:1.0f) //Inverte a rotaçao caso a direçao da bala seja reversa
			)
		);
	}
}
