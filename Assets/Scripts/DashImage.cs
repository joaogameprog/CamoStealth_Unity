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
		sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,sr.color.a-0.05f);
		if(sr.color.a <= 0) Destroy(gameObject);
	}
}
