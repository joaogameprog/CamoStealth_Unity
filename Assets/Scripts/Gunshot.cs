using UnityEngine;
using System.Collections;

public class Gunshot : MonoBehaviour {

	private SpriteRenderer sr;

	public float Damage = 10;
	public float speed  = 1;
	public Color color  = Color.blue;

	void Awake(){
		sr = GetComponent<SpriteRenderer>();
		sr.color = color;
	}
	// Use this for initialization
	void Start () {
		Destroy(gameObject,2);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += new Vector3(speed,0,0);
	}
}
