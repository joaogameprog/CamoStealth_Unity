using UnityEngine;
using System.Collections;

public class DashImage : MonoBehaviour {
	SpriteRenderer sr;

	void Awake(){
		sr = this.GetComponent<SpriteRenderer>();
		GameObject player = ((GameObject)(GameObject.FindGameObjectsWithTag("Player")[0]));
		sr.sprite = player.GetComponent<SpriteRenderer>().sprite;
	}
	// Update is called once per frame
	void FixedUpdate () {
		float r = 97;
		float g = 0;
		float b = 128;
		float a = sr.color.a;
		sr.color = new Color(r,g,b,a-0.1f);
		//this.renderer.material.SetColor(1, new Color(r,g,b,a));
		if(sr.color.a <= 0) Destroy(gameObject);
	}
}
