using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

[RequireComponent(typeof(TapGesture))]
public class BackButtom : MonoBehaviour {
	[SerializeField]
	GameObject root;

	void OnEnable(){
		GetComponent<TapGesture>().Tapped += HandleTapped;
	}



	void OnDisable()
	{
		GetComponent<TapGesture>().Tapped -= HandleTapped;
	}

	void HandleTapped (object sender, System.EventArgs e)
	{
		root.SetActive (false);
	}
}
