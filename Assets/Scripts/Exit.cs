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
		if(enterElevator == true){
			fakeLeon_sr.color += new Color(1,1,1,0.05f);
			frontDoor_sr.color = new Color(1,1,1,1);
			if(fakeLeon_sr.color.a >= 1){
				GetComponent<Animator>().SetBool("Open",true);
				frontDoor.GetComponent<Animator>().SetBool("Close",true);
			}
		}
	}

	/*Rect windowRect = new Rect(Screen.width/10 * 4,Screen.height/20 *8,Screen.width/10 *2,Screen.height/20 *2);
	void OnGUI() {
		// Register the window. Notice the 3rd parameter 
		if(victory){
			windowRect = GUI.Window (0, windowRect, windowFinnish, MainMenu.English?"End":"Fim");
			if((Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Space)|| Input.GetKey(KeyCode.KeypadEnter)))
				Application.LoadLevel("MainMenuScreen");
		}
	}

	void windowFinnish(int windowId){}*/
}
