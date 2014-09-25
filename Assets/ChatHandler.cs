using UnityEngine;
using System.Collections;

public class ChatHandler : MonoBehaviour {
	public EditableText text;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		text.OnEnterPressed += SendMessageToDebug;
	}

	void SendMessageToDebug ()
	{
		Debug.LogWarning (text.Text);
		text.Text = "";
	}
}
