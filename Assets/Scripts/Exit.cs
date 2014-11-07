using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	public static bool victory = false;
	public static GameObject exit;
	public GameObject fakeLeon;
	public GameObject frontDoor;
	private SpriteRenderer fakeLeon_sr;
	private SpriteRenderer frontDoor_sr;

	public bool enterElevator = false;
	[SerializeField]
	GameObject gameOverScreen;
	bool shownGameOver;

	float waitToGameOver = 3.5f;

	void Awake(){
		exit = gameObject;
		fakeLeon_sr = fakeLeon.GetComponent<SpriteRenderer>();
		frontDoor_sr = frontDoor.GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			victory = true;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(!shownGameOver && enterElevator == true){
			fakeLeon_sr.color += new Color(1,1,1,0.05f);
			frontDoor_sr.color = new Color(1,1,1,1);
			if(fakeLeon_sr.color.a >= 1){
				GetComponent<Animator>().SetBool("Open",true);
				frontDoor.GetComponent<Animator>().SetBool("Close",true);
				waitToGameOver -= Time.deltaTime;
				if(waitToGameOver < 0)
				{
					gameOverScreen.SetActive(true);
					shownGameOver = true;
				}
			}
		}

	}
}
