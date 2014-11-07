using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
	[SerializeField]
	TextMesh pressKey;
	float alphaChange = 0.02f;
	float targetAlpha = 0;
	// Update is called once per frame
	void FixedUpdate () {
		var color = pressKey.color;
		color.a = Mathf.MoveTowards (color.a, targetAlpha, alphaChange);
		pressKey.color = color;
		if (color.a == 1) {
			targetAlpha = 0;
		} else if (color.a == 0) {
			targetAlpha = 1;
		}
		if (Input.anyKey)
			Application.LoadLevel("MainMenuScreen");
	}
}
