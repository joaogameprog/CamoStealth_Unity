//######################################################################################################################
// Gunshot
// * Componente que controla os tiros das armas das aranhas
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class Gunshot : Damager {

	//private SpriteRenderer sr;

	public float speed  = 1;
	public Vector2 rotation;//= new Vector2(1,0);

	void Awake(){
		//sr = GetComponent<SpriteRenderer>();
	}
	// Use this for initialization
	void Start () {
		Destroy(gameObject,60);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += new Vector3(speed*rotation.x,speed*rotation.y,0);
	}
}
