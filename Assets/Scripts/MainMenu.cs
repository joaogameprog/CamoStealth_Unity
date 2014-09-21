using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	int option = 0;
	int coolDown = 0;
	public GameObject titleScreen;
	public Material englishScreen;
	public Material portugueseScreen;

	bool Credits = false;
	bool Options = false;
	public static bool English = true;
	void Awake(){
		updatePosition();
		updateScreen();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		--coolDown;
		if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Keypad5)|| Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.KeypadEnter)|| Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.L)){
			if(Input.GetKey(KeyCode.S) && !Credits && !Options){
				if(coolDown <= 0){
					if(++option > 3) option = 0;
				}
				updatePosition();
			}
			if(Input.GetKey(KeyCode.W) && !Credits && !Options){
				if(coolDown <=0){
					if(--option < 0) option = 3;
				}
				updatePosition();
			}
			if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Keypad5)|| Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.KeypadEnter)){
				if(coolDown <=0){
					action();
				}
			}

			if((Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Backspace)) && Options) Application.Quit();

			if(Input.GetKey(KeyCode.L) && Options){ English = !English; Options = false; updateScreen();}

			coolDown = 40;

		}else{
			coolDown = 0;
		}
	}

	void updatePosition(){
		if(option == 0) transform.position = new Vector3(-2.2208f,0.09111f,0);
		if(option == 1) transform.position = new Vector3(-1.2186f,-0.8086f,0);
		if(option == 2) transform.position = new Vector3(-0.36445f,-1.7197f,0);
		if(option == 3) transform.position = new Vector3(0.44415f,-2.5396f,0);
	}

	void updateScreen(){
		if(English){
			titleScreen.GetComponent<Renderer>().material = englishScreen;
		}else{
			titleScreen.GetComponent<Renderer>().material = portugueseScreen;
		}
	}

	void action(){
		if(option == 0) Application.LoadLevel("scene1");
		if(option == 1) transform.position = new Vector3(-1.2186f,-0.8086f,0);
		if(option == 2) Credits = !Credits;
		if(option == 3) Options = !Options;
	}
	Rect windowRect = new Rect(0,0,Screen.width,Screen.height);
	void OnGUI() {
		// Register the window. Notice the 3rd parameter 
		if(Credits)
			windowRect = GUI.Window (0, windowRect, windowCredits, English?"Credits":"Creditos");
		else if(Options)
			windowRect = GUI.Window (1, windowRect, windowOptions, English?"Options":"Opçoes");
	}
	void windowCredits (int windowID) {
		if(!English)
		GUI.TextArea(windowRect,"\n" +
			"Camo Stealth \n" +
			"\n " +
			"Objetivo do jogo: Fugir da base das aranhas \n\n" +
			"Como jogar:\n" +
			"    A: Andar para esquerda\n" +
			"    D: Andar para direita\n" +
			"    W: Esconder-se\n" +
			"    Espaço: Pular\n" +
			"    Shift: Correr\n" +
			"\n" +
			"Jogo desenvolvido como pre-projeto para o TCC de Jogos Digitais da FATEC SCS\n" +
			"\n" +
			"Membros do grupo:\n" +
			"    Alexandre Petrassi\n" +
			"    Joao Roberto\n" +
			"    Pablo Tadeu\n" +
			"    Rodrigo Premazzi"
		);
		else
		GUI.TextArea(windowRect, "\n" +
			         "Camo Stealth \n" +
		             "\n " +
		             "Game's Objective: Escape from the Spider's base \n\n" +
		             "How to play:\n" +
		             "    A: Walk Left\n" +
		             "    D: Walk Right\n" +
		             "    W: Hide\n" +
		             "    Space: Jump\n" +
		             "    Shift: Dash\n" +
		             "\n" +
			             "Game developed as a Digital Games Degree Conclusion Work's pre-project of FATEC SCS\n" +
		             "\n" +
		             "Group members:\n" +
		             "    Alexandre Petrassi\n" +
		             "    Joao Roberto\n" +
		             "    Pablo Tadeu\n" +
		             "    Rodrigo Premazzi"
		);

	}
	void windowOptions (int windowID) {
		if(!English)
			GUI.TextArea(windowRect,"\n" +
			             "\n " +
			             "Aperte a tecla L para trocar para o Idioma Ingles \n" +
			             "Aperte ESC para sair do jogo"
			             );
		else
			GUI.TextArea(windowRect,"\n" +
			             "\n " +
			             "Press L key to switch to Portuguese Language \n" +
			             "Press ESC to exit the game"
			             );
		
	}
}
