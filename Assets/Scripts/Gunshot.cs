//######################################################################################################################
// Gunshot
// * Componente que controla os tiros das armas das aranhas
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class Gunshot : Damager {

	//private SpriteRenderer sr;

	public float speed  = 1;

	void Awake(){
		//sr = GetComponent<SpriteRenderer>();
	}
	// Use this for initialization
	void Start () {
		Destroy(gameObject,60);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += new Vector3(speed,0,0);
	}
}
