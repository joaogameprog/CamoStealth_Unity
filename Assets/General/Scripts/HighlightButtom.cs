using UnityEngine;
using System.Collections;

public class HighlightButtom : MonoBehaviour {

	#region Fields
	[SerializeField]
	TextMesh child;

	#endregion

	#region Behaviours
	void OnMouseEnter(){
		child.gameObject.SetActive(true);
	}


	void OnMouseExit(){
		child.gameObject.SetActive(false);
	}
	#endregion
}
