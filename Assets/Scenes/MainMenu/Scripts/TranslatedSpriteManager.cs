using UnityEngine;
using System.Collections;
using System;
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class TranslatedSpriteManager : MonoBehaviour {

	public Sprite [] sprites;
	SpriteRenderer _spriteRenderer;
	void Awake()
	{
		TranslationsLanguages.LanguageChanged += SetValue;
		_spriteRenderer = GetComponent<SpriteRenderer>();
		if(sprites.Length > TranslationsLanguages.ActiveLanguage)
			_spriteRenderer.sprite = sprites[TranslationsLanguages.ActiveLanguage];

	}

	void OnDestroy()
	{
		TranslationsLanguages.LanguageChanged -= SetValue;

	}
	void Reset(){
		_spriteRenderer = GetComponent<SpriteRenderer>();
		sprites = new Sprite[TranslationsLanguages.LanguagesCount];
		var LanguageNames = Enum.GetNames(typeof(TranslationsLanguages.Languages));
		if(_spriteRenderer.sprite){
			for (int i = 0; i < LanguageNames.Length; i++) {
				sprites[i] = _spriteRenderer.sprite;
				sprites[i].name = LanguageNames[i] + " " + sprites[i].texture.name;
			}
		}
	}
	void OnValidate()
	{


		var LanguageNames = Enum.GetNames(typeof(TranslationsLanguages.Languages));
		for (int i = 0; i < LanguageNames.Length; i++) {
			sprites[i].name = LanguageNames[i] + " " + sprites[i].texture.name;
		}
	}

	void SetValue (int value)
	{
		if(sprites.Length > TranslationsLanguages.ActiveLanguage)
			_spriteRenderer.sprite = sprites[TranslationsLanguages.ActiveLanguage];
	}
}
