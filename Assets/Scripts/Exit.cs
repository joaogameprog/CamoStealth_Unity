using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
	public static bool victory = false;

	// Use this for initialization
	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == "Player"){
			victory = true;
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	Rect windowRect = new Rect(Screen.width/10 * 4,Screen.height/20 *8,Screen.width/10 *2,Screen.height/20 *2);
	void OnGUI() {
		// Register the window. Notice the 3rd parameter 
		if(victory){
			windowRect = GUI.Window (0, windowRect, windowFinnish, MainMenu.English?"End":"Fim");
			if((Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Space)|| Input.GetKey(KeyCode.KeypadEnter)))
				Application.LoadLevel("MainMenuScreen");
		}
	}

	void windowFinnish(int windowId){}
}
