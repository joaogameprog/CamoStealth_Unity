using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class OptionsScreenManager : MonoBehaviour {
	#region Fields

	float volume;

	[SerializeField]
	SpriteRenderer [] volumeMeter = null;

	[SerializeField]
	BoxCollider2D volumeCollider = null;

	[SerializeField]
	BoxCollider2D languagesCollider = null;

	[SerializeField]
	TapGesture btnSave = null;

	[SerializeField]
	AudioSource music = null;
	#endregion
	#region Behaviours
	void OnEnable () {
		volumeCollider.GetComponent<TapGesture> ().Tapped += Volume_Tapped;
		volumeCollider.GetComponent<PanGesture> ().Panned += Volume_Tapped;
		languagesCollider.GetComponent<TapGesture>().Tapped += Language_Tapped;
		btnSave.Tapped += Save_Tapped;
		SetVolume (ApplicationModel.SaveData.Volume);
	}

	void OnDisable () {
		volumeCollider.GetComponent<TapGesture> ().Tapped += Volume_Tapped;
		volumeCollider.GetComponent<PanGesture> ().Panned += Volume_Tapped;
		languagesCollider.GetComponent<TapGesture>().Tapped += Language_Tapped;
		btnSave.Tapped += Save_Tapped;
	}
#endregion

	#region Events
	void Save_Tapped (object sender, System.EventArgs e)
	{
		ApplicationModel.SaveData.Volume = volume;
		ApplicationModel.SaveData.Language = TranslationsLanguages.ActiveLanguage;
		ApplicationModel.Save.Save ();
	}

	void Language_Tapped (object sender, System.EventArgs e)
	{
		TranslationsLanguages.ActiveLanguage ++;
	}

	void Volume_Tapped (object sender, System.EventArgs e)
	{
		var tap = sender as Gesture;
		var pos = Camera.main.ScreenToWorldPoint(tap.PreviousScreenPosition);
		var center = volumeCollider.bounds.center.x;
		var extent = volumeCollider.bounds.extents.x;
		var newVolume = Mathf.InverseLerp (center - extent, center + extent, pos.x);

		SetVolume (newVolume);
	}


	void SetVolume (float newVolume)
	{
		var fullAlpha = 1f / volumeMeter.Length;
		for (int i = 0; i < volumeMeter.Length; i++) {
			volumeMeter [i].color = new Color (1f, 1f, 1f, 0.3f + 0.7f * Mathf.InverseLerp (fullAlpha * i, fullAlpha * (i + 1), newVolume));
		}
		volume = newVolume;
		music.volume = volume;
	}
	#endregion

}
