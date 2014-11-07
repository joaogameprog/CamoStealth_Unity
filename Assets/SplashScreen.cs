using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {
	void Awake()
	{
		Screen.SetResolution (800, 480, true);
	}
	public void CallMainMenu()
	{
		Application.LoadLevel("MainMenuScreen");
	}
}
