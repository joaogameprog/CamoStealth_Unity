using UnityEngine;
using System.Collections;
using System;
[ExecuteInEditMode]
[Serializable]
[RequireComponent(typeof(TextMesh))]
public class TranslatedTextManager : MonoBehaviour {

	[SerializeField]
	public string [] texts;
	TextMesh _textMesh;


	void Awake(){
				TranslationsLanguages.LanguageChanged += SetValue;
				_textMesh = GetComponent<TextMesh> ();
				if (texts.Length > TranslationsLanguages.ActiveLanguage)
						_textMesh.text = texts [TranslationsLanguages.ActiveLanguage];
		}
	void OnDestroy(){
		TranslationsLanguages.LanguageChanged -= SetValue;
	}
	void Reset(){
		_textMesh = GetComponent<TextMesh>();
		texts = new string[TranslationsLanguages.LanguagesCount];
		var LanguageNames = Enum.GetNames(typeof(TranslationsLanguages.Languages));
			for (int i = 0; i < LanguageNames.Length; i++) {
				texts[i] = _textMesh.text;
			}
	}

	void SetValue (int value)
	{
		if(texts.Length > TranslationsLanguages.ActiveLanguage)
			_textMesh.text = texts[TranslationsLanguages.ActiveLanguage];
	}
}
