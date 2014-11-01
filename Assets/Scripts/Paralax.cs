using UnityEngine;
using System.Collections;

public class Paralax : MonoBehaviour {

	Camera cam;
	Material mat;
	// Use this for initialization
	void Awake () {
		cam = Camera.main;
		mat = renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		mat.SetTextureOffset("_MainTex",new Vector2(-(((cam.transform.position.x*15)%100)/100),-(((cam.transform.position.y*15)%100)/100)));
	}
}
