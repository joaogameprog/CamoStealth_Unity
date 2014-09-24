using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(TapGesture))]
[RequireComponent (typeof(TextMesh))]
public class EditableText : MonoBehaviour {
	#region Fields
	[SerializeField]
	int id;

	static int focusedId;
	public static int FocusedId{
		get
		{
			return focusedId;
		}
		set
		{
			focusedId = value;
			//if(value != id)
			{
				//.isSelected = false;
				//this.SetInfo(this.isSelected);
			}
		}

	}
	[SerializeField]
	EditableText nextText;

	[SerializeField]
	Color normalColor = Color.white;
	[SerializeField]
	Color selectedColor = Color.blue;

	TapGesture tapGesture;

	TextMesh textMesh;

	[SerializeField]
	string text;

	public string Text
	{ 
		get
		{
			return text;
		}
		set
		{
			text = value;
			textMesh.text = value;
		}
	}

	[SerializeField]
	string defaultText;

	bool showIndexer;
	float indexerTime = 0.5f;
	float indexerTimer = 0f;

	bool isSelected;
	#endregion

	#region Behaviours
	// Use this for initialization
	void Awake () {
		tapGesture = GetComponent<TapGesture> ();
		textMesh = GetComponent<TextMesh> ();
		if(string.IsNullOrEmpty(defaultText))
		{
			defaultText = textMesh.text;
		}
	}
	
	// Update is called once per frame
	void OnGUI () {
		if(isSelected)
		{
			Event e = Event.current;
			if(e != null && e.type == EventType.KeyDown)
			{

				Debug.Log("   Key = " + e.keyCode + "   char = " + e.character);
				if(e.keyCode != KeyCode.None){
					if(e.keyCode == KeyCode.Backspace && Text.Length > 0){
						Text = Text.Substring(0,Text.Length - 1);
					}else if(e.keyCode == KeyCode.Return)
					{
						Selected(this, System.EventArgs.Empty);
					}
					else if(e.keyCode == KeyCode.Tab)
					{
						Selected(this, System.EventArgs.Empty);
						if(nextText != null)
						{
							nextText.Selected(this, System.EventArgs.Empty);
						}
					}
					e.Use();
				}
				else if(e.character != '\t')
				{
					Text += e.character;
				}
				/*
				if(Event.current != null){
					if(Event.current.isKey){
						Text = Event.current.keyCode.ToString();
					}
				}
				*/
			}
		}
	}

	void Update()
	{
		if(isSelected)
		{
			indexerTimer -= Time.deltaTime;

			if(indexerTimer < 0)
			{
				showIndexer = !showIndexer;
				indexerTimer = indexerTime;
				textMesh.text = Text + (showIndexer ? "|" : "");
			}
		}
	}

	void OnEnable()
	{
		tapGesture.Tapped += Selected;

	}

	void OnDisable()
	{
		tapGesture.Tapped -= Selected;
		
	}




	#endregion

	#region Events
	public void Selected (object sender, System.EventArgs e)
	{
		isSelected = !isSelected;
		SetInfo (isSelected);
	}


	void SetInfo (bool selected)
	{
		if (selected) {
			textMesh.color = selectedColor;
		}
		else {
			textMesh.color = normalColor;
			textMesh.text = Text;
			if (string.IsNullOrEmpty (textMesh.text)) {
				textMesh.text = defaultText;
			}
		}
	}
	#endregion

}
