using UnityEngine;
using System.Collections;
using System;
[ExecuteInEditMode]
public class TranslationsLanguages : MonoBehaviour {
	public static Action<int> LanguageChanged;
	public static readonly int LanguagesCount = Enum.GetNames(typeof(Languages)).Length;
	public enum Languages
	{
		English,
		Portuguese
	}

	void Awake()
	{
		if (ForceLanguage) {
						ActiveLanguage = (int)LanguageForced;
				} else {
						try {
								ActiveLanguage = ApplicationModel.SaveData.Language; //(int)Enum.Parse(typeof(Languages), Application.systemLanguage.ToString());
						} catch (Exception e) {
								ActiveLanguage = (int)Enum.Parse (typeof(Languages), Application.systemLanguage.ToString ());
						}
				}
	}
	[SerializeField]
	private bool ForceLanguage;
	[SerializeField]
	private Languages LanguageForced;
	private static int _activeLanguage;
	public static int ActiveLanguage 
	{ 
			get {
					return _activeLanguage;
			}
			set {
					_activeLanguage = value % LanguagesCount;
					if(LanguageChanged != null)
					{
				LanguageChanged(value % LanguagesCount);
					}
			}
	}
}
