using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class MainMenuManager : MonoBehaviour {
	#region Fields

	/// <summary>
	/// The button play.
	/// </summary>
	[SerializeField]
	TapGesture btnPlay;

	/// <summary>
	/// The button continue.
	/// </summary>
	[SerializeField]
	TapGesture btnAbout;

	/// <summary>
	/// The button options.
	/// </summary>
	[SerializeField]
	TapGesture btnOptions;

	/// <summary>
	/// The button help.
	/// </summary>
	[SerializeField]
	TapGesture btnHelp;

	/// <summary>
	/// The button credits.
	/// </summary>
	[SerializeField]
	TapGesture btnCredits;

	/// <summary>
	/// The options screen.
	/// </summary>
	[SerializeField]
	GameObject OptionsScreen;

	/// <summary>
	/// The help screen.
	/// </summary>
	[SerializeField]
	GameObject HelpScreen;
	/// <summary>
	/// The credits screen.
	/// </summary>
	[SerializeField]
	GameObject CreditsScreen;

	/// <summary>
	/// The about screen.
	/// </summary>
	[SerializeField]
	GameObject AboutScreen;

	#endregion

	#region Behaviours
	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		btnPlay.Tapped += btnPlay_Tapped;
		btnAbout.Tapped += btnAbout_Tapped;
		btnOptions.Tapped += btnOptions_Tapped;
		btnHelp.Tapped += btnHelp_Tapped;
		btnCredits.Tapped += btnCredits_Tapped;
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		btnPlay.Tapped -= btnPlay_Tapped;
		btnAbout.Tapped -= btnAbout_Tapped;
		btnOptions.Tapped -= btnOptions_Tapped;
		btnHelp.Tapped -= btnHelp_Tapped;
		btnCredits.Tapped -= btnCredits_Tapped;
	}




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	#endregion

	#region Handlers
	void btnPlay_Tapped (object sender, System.EventArgs e)
	{
		Application.LoadLevel("level1");
	}
	
	void btnAbout_Tapped (object sender, System.EventArgs e)
	{
		AboutScreen.SetActive (true);
	}
	
	void btnOptions_Tapped (object sender, System.EventArgs e)
	{
		OptionsScreen.SetActive (true);
	}

	void btnHelp_Tapped (object sender, System.EventArgs e)
	{
		HelpScreen.SetActive (true);
	}
	
	void btnCredits_Tapped (object sender, System.EventArgs e)
	{
		CreditsScreen.SetActive (true);
	}

	#endregion

}
